using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static SaveLoadGame.SaveGame;
using Random = UnityEngine.Random;

namespace GeneracionSalas
{
    public class GeneracionSalasMatriz : MonoBehaviour, ISaveableSalasData
    {
        private List<ListaSalas> m_ListaSalasPadre = new();
        private List<ListaSalasConHijos> m_ListaSalasPadreConHijos = new();
        private List<ListaSalasConHijos> m_ListaPasillosConSalas = new();
        private bool m_MundoGenerado;

        [Header("Numeros Sala")]
        [SerializeField] private int numSala;
        [SerializeField] private int numSalaMaxima;
        private int[,] matrix = new int[100, 100];

        private GeneracionSalaInstanciacion m_GeneracionSalasInstanciacion;

        private void Awake()
        {
            m_GeneracionSalasInstanciacion = GetComponent<GeneracionSalaInstanciacion>();
            m_MundoGenerado = false;
        }
        private void Start()
        {
            StartCoroutine(IniciarMundo());
        }

        private IEnumerator IniciarMundo()
        {
            yield return new WaitForEndOfFrame();
            if (!m_MundoGenerado)
            {
                if (GameManager.Instance.NuevaPartida)
                {
                    try
                    {
                        print("Eyou");
                        m_ListaSalasPadre = new();
                        m_ListaSalasPadreConHijos = new();
                        m_ListaPasillosConSalas = new();
                        numSala = numSalaMaxima;
                        RellenarMatriz();
                        GenerarMapa(50, 50, 0, new ListaSalas(50, 50));
                        matrix[m_ListaSalasPadre[m_ListaSalasPadre.Count - 1].x + 50, m_ListaSalasPadre[m_ListaSalasPadre.Count - 1].y + 50] = 2;
                        if (numSala != 0)
                            Start();
                        else
                        {
                            m_MundoGenerado = true;
                            m_GeneracionSalasInstanciacion.InstanciarElMundo(matrix, m_ListaSalasPadreConHijos, m_ListaPasillosConSalas);
                            StopAllCoroutines();
                        }
                    }
                    catch (Exception)
                    {
                        Start();
                    }
                }
            }
        }

        public void Init()
        {
            if (GameManager.Instance.NuevaPartida)
            {
                GameManager.Instance.setTesting();
                Start();
            }
        }

        private void RellenarMatriz()
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                    matrix[i, j] = 0;
            }
        }

        private void GenerarMapa(int posX, int posY, int tipoSalaBoss, ListaSalas salaPadre)
        {
            if (numSala > 0)
            {
                if (numSala == numSalaMaxima)
                {
                    matrix[posX, posY] = 1;
                    m_ListaSalasPadreConHijos.Add(new ListaSalasConHijos(new ListaSalas(posX - 50, posY - 50), new List<ListaSalas>()));
                }
                else if (numSala == 0)
                    matrix[posX, posY] = 5;
                else
                {
                    if (tipoSalaBoss == 9)
                        AddSalaPadre(posX - 50, posY - 50, salaPadre);
                    matrix[posX, posY] = tipoSalaBoss;
                }

                List<int> pasillosAlrededor;
                GetPasillosAlrededor(out pasillosAlrededor, posX, posY);
                CambiarMatriz(pasillosAlrededor, posX, posY, salaPadre);
            }
        }

        private void GetPasillosAlrededor(out List<int> pasillosAlrededor, int posX, int posY)
        {
            pasillosAlrededor = new List<int>();
            List<int> posiblesPasillosAlrededor = GetPosiblesPuertasAlrededor(posX, posY);
            posiblesPasillosAlrededor.Shuffle();
            int numeroPuertas = Random.Range(1, 5);
            if (posiblesPasillosAlrededor.Count < numSala)
            {
                for (int i = 0; i < numeroPuertas; i++)
                    pasillosAlrededor.Add(posiblesPasillosAlrededor[i]);
            }
            else
            {
                for (int i = 0; i < numSala; i++)
                    pasillosAlrededor.Add(posiblesPasillosAlrededor[i]);
            }
        }

        private List<int> GetPosiblesPuertasAlrededor(int posX, int posY)
        {
            List<int> puertasDisponibles = new List<int>();

            if (matrix[posX + 2, posY] == 0 || matrix[posX + 1, posY] == 0) { puertasDisponibles.Add(1); }
            if (matrix[posX - 2, posY] == 0 || matrix[posX - 1, posY] == 0) { puertasDisponibles.Add(2); }
            if (matrix[posX, posY + 2] == 0 || matrix[posX, posY + 1] == 0) { puertasDisponibles.Add(3); }
            if (matrix[posX, posY - 2] == 0 || matrix[posX, posY - 1] == 0) { puertasDisponibles.Add(4); }

            return puertasDisponibles;
        }

        private void CambiarMatriz(List<int> puertasAlrededor, int posX, int posY, ListaSalas salaPadre)
        {
            int salaLado;

            foreach (int puerta in puertasAlrededor)
            {
                salaLado = Random.Range(0, 11);
                if (salaLado >= 0 && salaLado <= 6)
                    salaLado = 2;
                else if (salaLado > 6 && salaLado <= 8)
                    salaLado = 3;
                else if (salaLado > 8 && salaLado <= 9)
                    salaLado = 4;
                else
                    salaLado = 5;

                switch (puerta)
                {
                    case 1:
                        SalaLateral(posX, posY, posX, posY - 1, posX, posY - 2, salaLado, salaPadre);
                        break;
                    case 2:
                        SalaLateral(posX, posY, posX + 1, posY, posX + 2, posY, salaLado, salaPadre);
                        break;
                    case 3:
                        SalaLateral(posX, posY, posX, posY + 1, posX, posY + 2, salaLado, salaPadre);
                        break;
                    case 4:
                        SalaLateral(posX, posY, posX - 1, posY, posX - 2, posY, salaLado, salaPadre);
                        break;
                }
            }
        }

        private void SalaLateral(int posX1, int posY1, int posX2, int posY2, int posX3, int posY3, int salaLado, ListaSalas salaPadre)
        {
            if (2 == salaLado)
            {
                if (2 == matrix[posX1, posY1] || 9 == matrix[posX1, posY1])
                {
                    AmpliarSala(posX2, posY2, salaPadre);
                }
            }
            else
            {
                if (numSala > 0 && matrix[posX2, posY2] == 0 && matrix[posX3, posY3] == 0)
                {
                    matrix[posX2, posY2] = salaLado;

                    if (!EstaEnLaListaDePasillos(posX2 - 50, posY2 - 50))
                    {
                        m_ListaPasillosConSalas.Add(new ListaSalasConHijos(new ListaSalas(posX2 - 50, posY2 - 50), new List<ListaSalas>
                    {
                        new ListaSalas(posX1 - 50, posY1 - 50),
                        new ListaSalas(posX3 - 50, posY3 - 50)
                    }));
                    }

                    ListaSalas salaEncontrada = m_ListaSalasPadre.FirstOrDefault<ListaSalas>(i => i.x == posX3 && i.y == posY3);
                    if (salaEncontrada.x == 0 && salaEncontrada.y == 0)
                    {
                        m_ListaSalasPadre.Add(new ListaSalas(posX3 - 50, posY3 - 50));
                        salaPadre = new ListaSalas(posX3 - 50, posY3 - 50);
                        m_ListaSalasPadreConHijos.Add(new ListaSalasConHijos(new ListaSalas(posX3 - 50, posY3 - 50), new List<ListaSalas>()));
                        PonerNumeroSala(posX3, posY3, salaPadre);
                    }
                }
            }
        }

        private void AddSalaPadre(int posX2, int posY2, ListaSalas salaPadre)
        {

            if (EstaENLaListaDeSalas(salaPadre.x, salaPadre.y))
            {
                for (int i = 0; i < m_ListaSalasPadreConHijos.Count; i++)
                {
                    if (m_ListaSalasPadreConHijos[i].m_HabitacionPadre.x == salaPadre.x && m_ListaSalasPadreConHijos[i].m_HabitacionPadre.y == salaPadre.y)
                        m_ListaSalasPadreConHijos[i].m_HabitacionesHijas.Add(new ListaSalas(posX2, posY2));
                }
            }

        }

        private void AmpliarSala(int row, int col, ListaSalas salaPadre)
        {
            if (matrix[row, col] != 0)
                return;

            GenerarMapa(row, col, 9, salaPadre);
        }

        private void PonerNumeroSala(int row, int col, ListaSalas salaPadre)
        {
            if (matrix[row, col] != 0 && numSala > 0)
                return;

            numSala--;
            GenerarMapa(row, col, 2, salaPadre);
        }

        private bool EstaENLaListaDeSalas(int x, int y)
        {
            for (int i = 0; i < m_ListaSalasPadre.Count; i++)
            {
                if (m_ListaSalasPadre[i].x == x && m_ListaSalasPadre[i].y == y)
                    return true;
            }
            return false;
        }

        private bool EstaEnLaListaDePasillos(int x, int y)
        {
            for (int i = 0; i < m_ListaPasillosConSalas.Count; i++)
            {
                if (m_ListaPasillosConSalas[i].m_HabitacionPadre.x == x && m_ListaPasillosConSalas[i].m_HabitacionPadre.y == y)
                {
                    return true;
                }
            }
            return false;
        }

        public SalasData Save()
        {
            SalasData salasMapaData = new SalasData();
            salasMapaData.m_ListaPasillos = m_ListaPasillosConSalas.ToArray();
            salasMapaData.m_SalasBosses = m_ListaSalasPadreConHijos.ToArray();

            int[] matrixGuardar;
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            matrixGuardar = new int[rows * cols];

            int index = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrixGuardar[index] = matrix[i, j];
                    index++;
                }
            }

            salasMapaData.m_Matriz = matrixGuardar;

            return salasMapaData;
        }

        public void Load(SalasData _salaData)
        {
            m_ListaPasillosConSalas = _salaData.m_ListaPasillos.ToList();
            m_ListaSalasPadreConHijos = _salaData.m_SalasBosses.ToList();

            int[,] reconstructedMatrix = new int[100, 100];

            int index = 0;
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    reconstructedMatrix[i, j] = _salaData.m_Matriz[index];
                    index++;
                }
            }
            matrix = reconstructedMatrix;

            m_GeneracionSalasInstanciacion.InstanciarElMundo(matrix, m_ListaSalasPadreConHijos, m_ListaPasillosConSalas);
        }

        [Serializable]
        public struct ListaSalas
        {
            public int x, y;

            public ListaSalas(int X, int Y)
            {
                x = X; y = Y;
            }
        }

        [Serializable]
        public struct ListaSalasConHijos
        {
            public ListaSalas m_HabitacionPadre;
            public List<ListaSalas> m_HabitacionesHijas;

            public ListaSalasConHijos(ListaSalas _HabitacionPadre, List<ListaSalas> _HabitacionesHijas)
            {
                m_HabitacionPadre = _HabitacionPadre;
                m_HabitacionesHijas = _HabitacionesHijas;
            }
        }
    }
}