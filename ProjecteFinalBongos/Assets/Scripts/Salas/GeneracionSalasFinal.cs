using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;
using static SaveGame;
using Random = UnityEngine.Random;

namespace GeneracionSalas
{
    public class GeneracionSalasFinal : MonoBehaviour, ISaveableSalaData
    {
        [Header("Parent Mundo")]
        [SerializeField] private Transform m_TransformParentMundo;

        [Header("Prefabs Paredes Sala")]
        [SerializeField] private GameObject m_ParedArribaSala;
        [SerializeField] private GameObject m_ParedDerechaSala;
        [SerializeField] private GameObject m_ParedAbajoSala;
        [SerializeField] private GameObject m_ParedIzquierdaSala;

        [Header("Prefabs Puertas Sala")]
        [SerializeField] private GameObject m_PuertaArribaSala;
        [SerializeField] private GameObject m_PuertaDerechaSala;
        [SerializeField] private GameObject m_PuertaAbajoSala;
        [SerializeField] private GameObject m_PuertaIzquierdaSala;

        [Header("Prefabs Paredes Pasillo")]
        [SerializeField] private GameObject m_ParedArribaPasillo;
        [SerializeField] private GameObject m_ParedDerechaPasillo;
        [SerializeField] private GameObject m_ParedAbajoPasillo;
        [SerializeField] private GameObject m_ParedIzquierdaPasillo;

        [Header("Prefabs Puertas Pasillo")]
        [SerializeField] private GameObject m_PuertaArribaPasillo;
        [SerializeField] private GameObject m_PuertaDerechaPasillo;
        [SerializeField] private GameObject m_PuertaAbajoPasillo;
        [SerializeField] private GameObject m_PuertaIzquierdaPasillo;

        [Header("GameObject Sala")]
        [SerializeField] GameObject m_SalaInicial;
        [SerializeField] private GameObject m_SalaBossInicial;
        [SerializeField] private GameObject m_SalaBoss;
        [SerializeField] private GameObject m_PasilloBichos;
        [SerializeField] private GameObject m_PasilloObjetos;
        [SerializeField] private GameObject m_PasilloTienda;

        private List<ListaSalas> m_ListaSalasPadre = new List<ListaSalas>();
        private List<ListaSalasConHijos> m_ListaSalasPadreConHijos = new();
        private List<ListaSalasConHijos> m_ListaPasillosConSalas = new();

        public Action onMapaFinalized;

        [Header("Numeros Sala")]
        [SerializeField] private int numSala;
        [SerializeField] private int numSalaMaxima;
        private int[,] matrix = new int[100, 100];

        [Header("Tiles y Tilemap")]
        [SerializeField] private Tilemap m_Tilemap;
        [SerializeField] private TileBase m_TileSalas;
        [SerializeField] private TileBase m_TilePared;
        private List<Vector2> m_TilesSala = new();
        private List<Vector2> m_TilesPared = new();

        // Start is called before the first frame update
        void Start()
        {
            try
            {
                m_ListaSalasPadre.Clear();
                m_ListaSalasPadreConHijos.Clear();
                m_ListaPasillosConSalas.Clear();
                numSala = numSalaMaxima;
                RellenarMatriz();
                GenerarMapa(50, 50, 0, new ListaSalas(50, 50));
                matrix[m_ListaSalasPadre[m_ListaSalasPadre.Count - 1].x + 50, m_ListaSalasPadre[m_ListaSalasPadre.Count - 1].y + 50] = 2;
                if (numSala != 0)
                    Start();
                else
                {
                    GenSalasBoss();
                    GenPasillos();
                    PintarTilemap();
                    onMapaFinalized.Invoke();
                }
            }
            catch (Exception)
            {
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
        private void GenPasillos()
        {
            foreach (ListaSalasConHijos salaPasillo in m_ListaPasillosConSalas)
                InstanciarPasillo(salaPasillo.m_HabitacionPadre.x + 50, salaPasillo.m_HabitacionPadre.y + 50, salaPasillo.m_HabitacionesHijas);
        }
        private void GenSalasBoss()
        {
            foreach (ListaSalasConHijos salaPadre in m_ListaSalasPadreConHijos)
            {
                salaPadre.m_HabitacionesHijas.Add(salaPadre.m_HabitacionPadre);
                InstanciarSalaJefeInicial(salaPadre.m_HabitacionPadre.x + 50, salaPadre.m_HabitacionPadre.y + 50, salaPadre.m_HabitacionesHijas, salaPadre);
                for (int i = 0; i < salaPadre.m_HabitacionesHijas.Count - 1; i++)
                {
                    InstanciarSalaJefes(salaPadre.m_HabitacionesHijas[i].x + 50, salaPadre.m_HabitacionesHijas[i].y + 50, salaPadre.m_HabitacionesHijas);
                }
            }
        }

        private void InstanciarSalaJefeInicial(int x, int y, List<ListaSalas> salasHijas, ListaSalasConHijos salaPadre)
        {
            float posicionX = (x - 50) * 20;
            float posicionY = (y - 50) * 20;
            GameObject sala = null;

            switch (matrix[x, y])
            {
                case 1:
                    sala = Instantiate(m_SalaInicial, m_TransformParentMundo);
                    sala.transform.position = new Vector3(posicionX, posicionY, 0);
                    break;
                case 2:
                    sala = Instantiate(m_SalaBossInicial, m_TransformParentMundo);
                    sala.transform.position = new Vector3(posicionX, posicionY, 0);
                    sala.GetComponent<SalaBoss>().Init(salaPadre);
                    break;
            }
            if (sala != null)
            {
                PintarSalaBoss(posicionX, posicionY);
                InstanciarParedesAndPuertasBoss(x, y, sala.transform, matrix[x, y], salasHijas, posicionX, posicionY);
            }
        }

        private void InstanciarPasillo(int x, int y, List<ListaSalas> habitacionesHijas)
        {
            float posicionX = (x - 50) * 20;
            float posicionY = (y - 50) * 20;
            GameObject sala;

            switch (matrix[x, y])
            {
                case 3:
                    sala = Instantiate(m_PasilloBichos, m_TransformParentMundo);
                    break;
                case 4:
                    sala = Instantiate(m_PasilloTienda, m_TransformParentMundo);
                    break;
                case 5:
                    sala = Instantiate(m_PasilloObjetos, m_TransformParentMundo);
                    break;
                default:
                    sala = null;
                    break;
            }
            if (sala != null)
            {
                sala.transform.position = new Vector3(posicionX, posicionY, 0);
                
                InstanciarParedesAndPuertasPasillo(x, y, sala.transform, matrix[x, y], habitacionesHijas, posicionX, posicionY);
            }
        }


        private void InstanciarSalaJefes(int x, int y, List<ListaSalas> salasHijas)
        {
            float posicionX = (x - 50) * 20;
            float posicionY = (y - 50) * 20;
            GameObject sala = null;

            switch (matrix[x, y])
            {
                case 9:
                    sala = Instantiate(m_SalaBoss, m_TransformParentMundo);
                    break;
                default:
                    break;
            }
            if (sala != null)
            {
                sala.transform.position = new Vector3(posicionX, posicionY, 0);
                PintarSalaBoss(posicionX, posicionY);
                InstanciarParedesAndPuertasBoss(x, y, sala.transform, matrix[x, y], salasHijas, posicionX, posicionY);
            }
        }

        private void PintarSalaBoss(float posicionX, float posicionY)
        {
            for (int i = (int)posicionX - 10; i < posicionX + 10; i++)
            {
                for (int j = (int)posicionY - 10; j < posicionY + 10; j++)
                {
                    m_TilesSala.Add(new Vector2(i, j));
                }
            }
        }

        private void InstanciarParedesAndPuertasPasillo(int posicionX, int posicionY, Transform transformSala, int tipoSala, List<ListaSalas> salasHijas, float posMundoX, float posMundoY)
        {
            GameObject estructuraArriba;
            GameObject estructuraDerecha;
            GameObject estructuraAbajo;
            GameObject estructuraIzquierda;
            List<GameObject> estructuras = new();
            bool horizontal;

            if (!EstaENLaListaDeSalas(posicionX - 50, posicionY + 1 - 50, salasHijas))
            {
                estructuraArriba = Instantiate(m_ParedArribaPasillo, transformSala);
                estructuras.Add(estructuraArriba);
                for (int i = 0; i < 22; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 11 + i, posMundoY + 6));
                }
                for (int i = 0; i < 22; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 11 + i, posMundoY + 7));
                }
                horizontal = true;
            }
            else
            {
                estructuraArriba = Instantiate(m_PuertaArribaPasillo, transformSala);
                estructuras.Add(estructuraArriba);
                horizontal = false;
            }
            if (!EstaENLaListaDeSalas(posicionX + 1 - 50, posicionY - 50, salasHijas))
            {
                estructuraDerecha = Instantiate(m_ParedDerechaPasillo, transformSala);
                for (int i = 0; i < 20; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX + 7, posMundoY - 10 + i));
                }
                for (int i = 0; i < 20; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX + 6, posMundoY - 10 + i));
                }
                estructuras.Add(estructuraDerecha);
            }
            else
            {
                estructuraDerecha = Instantiate(m_PuertaDerechaPasillo, transformSala);
                estructuras.Add(estructuraDerecha);
            }
            if (!EstaENLaListaDeSalas(posicionX - 50, posicionY - 1 - 50, salasHijas))
            {
                estructuraAbajo = Instantiate(m_ParedAbajoPasillo, transformSala);
                estructuras.Add(estructuraAbajo);
                for (int i = 0; i < 22; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 11 + i, posMundoY - 7f));
                }
                for (int i = 0; i < 22; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 11 + i, posMundoY - 8f));
                }
            }
            else
            {
                estructuraAbajo = Instantiate(m_PuertaAbajoPasillo, transformSala);
                estructuras.Add(estructuraAbajo);
            }
            if (!EstaENLaListaDeSalas(posicionX - 1 - 50, posicionY - 50, salasHijas))
            {
                estructuraIzquierda = Instantiate(m_ParedIzquierdaPasillo, transformSala);
                for (int i = 0; i < 20; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 7, posMundoY - 10 + i));
                }
                for (int i = 0; i < 20; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 8, posMundoY - 10 + i));
                }
                estructuras.Add(estructuraIzquierda);
            }
            else
            {
                estructuraIzquierda = Instantiate(m_PuertaIzquierdaPasillo, transformSala);
                estructuras.Add(estructuraIzquierda);
            }
            PintarPasillo(posMundoX, posMundoY, horizontal);
        }
        private void PintarPasillo(float posicionX, float posicionY, bool horizontal)
        {
            if (horizontal)
            {
                for (int i = (int)posicionX - 9; i < posicionX + 9; i++)
                {
                    for (int j = (int)posicionY - 7; j < posicionY + 7; j++)
                    {
                        m_TilesSala.Add(new Vector2(i, j));
                    }
                }
                m_TilesSala.Add(new Vector2(posicionX + 9, posicionY - 1));
                m_TilesSala.Add(new Vector2(posicionX + 9, posicionY));
                m_TilesSala.Add(new Vector2(posicionX - 10, posicionY - 1));
                m_TilesSala.Add(new Vector2(posicionX - 10, posicionY));
            }
            else
            {
                for (int i = (int)posicionX - 7; i < posicionX + 7; i++)
                {
                    for (int j = (int)posicionY - 9; j < posicionY + 9; j++)
                    {
                        m_TilesSala.Add(new Vector2(i, j));
                    }
                }
                m_TilesSala.Add(new Vector2(posicionX - 1, posicionY + 9.5f));
                m_TilesSala.Add(new Vector2(posicionX, posicionY + 9.5f));
                m_TilesSala.Add(new Vector2(posicionX - 1, posicionY - 9.5f));
                m_TilesSala.Add(new Vector2(posicionX, posicionY - 9.5f));
            }
        }
        private void InstanciarParedesAndPuertasBoss(int posicionX, int posicionY, Transform transformSala, int tipoSala, List<ListaSalas> salasHijas, float posMundoX, float posMundoY)
        {
            GameObject estructuraArriba;
            GameObject estructuraDerecha;
            GameObject estructuraAbajo;
            GameObject estructuraIzquierda;
            List<GameObject> estructuras = new();

            if (EstaEnLaListaDePasillosHijos(posicionX - 50, posicionY - 50, posicionX - 50, posicionY + 1 - 50))
            {
                estructuraArriba = Instantiate(m_PuertaArribaSala, transformSala);
                for (int i = 0; i < 10; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 11 + i, posMundoY + 9.5f));
                }
                for (int i = 12; i < 22; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 11 + i, posMundoY + 9.5f));
                }
                for (int i = 0; i < 10; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 11 + i, posMundoY + 10.5f));
                }
                for (int i = 12; i < 22; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 11 + i, posMundoY + 10.5f));
                }
                m_TilesSala.Add(new Vector2(posMundoX - 1, posMundoY + 9.5f));
                m_TilesSala.Add(new Vector2(posMundoX, posMundoY + 9.5f));
                estructuras.Add(estructuraArriba);
            }
            else if (!EstaENLaListaDeSalas(posicionX - 50, posicionY + 1 - 50, salasHijas))
            {
                estructuraArriba = Instantiate(m_ParedArribaSala, transformSala);
                for (int i = 0; i < 22; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 11 + i, posMundoY + 9.5f));
                }
                for (int i = 0; i < 22; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 11 + i, posMundoY + 10.5f));
                }
                estructuras.Add(estructuraArriba);
            }
            else
            {
                for (int i = 0; i < 19; i++)
                {
                    m_TilesSala.Add(new Vector2(posMundoX - 9 + i, posMundoY + 9.5f));
                }
            }

            if (EstaEnLaListaDePasillosHijos(posicionX - 50, posicionY - 50, posicionX + 1 - 50, posicionY - 50))
            {
                estructuraDerecha = Instantiate(m_PuertaDerechaSala, transformSala);
                for (int i = 0; i < 9; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX + 9, posMundoY -10 +i));
                }
                for (int i = 11; i < 21; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX + 9, posMundoY - 10 + i));
                }
                for (int i = 0; i < 9; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX + 10, posMundoY - 10 + i));
                }
                for (int i = 11; i < 21; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX + 10, posMundoY - 10 + i));
                }
                m_TilesSala.Add(new Vector2(posMundoX + 9, posMundoY -1));
                m_TilesSala.Add(new Vector2(posMundoX + 9, posMundoY));
                estructuras.Add(estructuraDerecha);
            }
            else if (!EstaENLaListaDeSalas(posicionX + 1 - 50, posicionY - 50, salasHijas))
            {
                estructuraDerecha = Instantiate(m_ParedDerechaSala, transformSala);
                for (int i = 0; i < 21; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX + 10, posMundoY - 10 + i));
                }
                for (int i = 0; i < 21; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX + 9, posMundoY - 10 + i));
                }
                estructuras.Add(estructuraDerecha);
            }
            else
            {
                for (int i = 0; i < 19; i++)
                {
                    m_TilesSala.Add(new Vector2(posMundoX + 9.5f, posMundoY - 9 + i));
                }
            }

            if (EstaEnLaListaDePasillosHijos(posicionX - 50, posicionY - 50, posicionX - 50, posicionY - 1 - 50))
            {
                estructuraAbajo = Instantiate(m_PuertaAbajoSala, transformSala);
                for (int i = 0; i < 10; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 11 + i, posMundoY - 9.5f));
                }
                for (int i = 12; i < 22; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 11 + i, posMundoY - 9.5f));
                }
                for (int i = 0; i < 10; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 11 + i, posMundoY - 10.5f));
                }
                for (int i = 12; i < 22; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 11 + i, posMundoY - 10.5f));
                }
                m_TilesSala.Add(new Vector2(posMundoX - 1, posMundoY - 9.5f));
                m_TilesSala.Add(new Vector2(posMundoX, posMundoY - 9.5f));
                estructuras.Add(estructuraAbajo);
            }
            else if (!EstaENLaListaDeSalas(posicionX - 50, posicionY - 1 - 50, salasHijas))
            {
                estructuraAbajo = Instantiate(m_ParedAbajoSala, transformSala);
                for (int i = 0; i < 22; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 11 + i, posMundoY - 9.5f));
                }
                for (int i = 0; i < 22; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 11 + i, posMundoY - 10.5f));
                }
                estructuras.Add(estructuraAbajo);
            }
            else
            {
                for (int i = 0; i < 19; i++)
                {
                    m_TilesSala.Add(new Vector2(posMundoX - 9 + i, posMundoY - 9.5f));
                }
            }

            if (EstaEnLaListaDePasillosHijos(posicionX - 50, posicionY - 50, posicionX - 1 - 50, posicionY - 50))
            {
                estructuraIzquierda = Instantiate(m_PuertaIzquierdaSala, transformSala);
                for (int i = 0; i < 9; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 10, posMundoY - 10 + i));
                }
                for (int i = 11; i < 21; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 10, posMundoY - 10 + i));
                }
                for (int i = 0; i < 9; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 11, posMundoY - 10 + i));
                }
                for (int i = 11; i < 21; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 11, posMundoY - 10 + i));
                }
                m_TilesSala.Add(new Vector2(posMundoX - 10, posMundoY - 1));
                m_TilesSala.Add(new Vector2(posMundoX - 10, posMundoY));
                estructuras.Add(estructuraIzquierda);
            }
            else if (!EstaENLaListaDeSalas(posicionX - 1 - 50, posicionY - 50, salasHijas))
            {
                estructuraIzquierda = Instantiate(m_ParedIzquierdaSala, transformSala);
                for (int i = 0; i < 21; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 10, posMundoY - 10 + i));
                }
                for (int i = 0; i < 21; i++)
                {
                    m_TilesPared.Add(new Vector2(posMundoX - 11, posMundoY - 10 + i));
                }
                estructuras.Add(estructuraIzquierda);
            }
            else
            {
                for (int i = 0; i < 19; i++)
                {
                    m_TilesSala.Add(new Vector2(posMundoX - 9.5f, posMundoY - 9 + i));
                }
            }
        }

        private void PintarTilemap()
        {
            foreach (Vector2 posicion in m_TilesSala)
            {
                Vector3Int posicionInt = m_Tilemap.WorldToCell(posicion);
                m_Tilemap.SetTile(posicionInt, m_TileSalas);
            }
            foreach (Vector2 posicion in m_TilesPared)
            {
                Vector3Int posicionInt = m_Tilemap.WorldToCell(posicion);
                m_Tilemap.SetTile(posicionInt, m_TilePared);
            }
        }

        private bool EstaENLaListaDeSalas(int x, int y, List<ListaSalas> salasHijas)
        {
            for (int i = 0; i < salasHijas.Count; i++)
            {
                if (salasHijas[i].x == x && salasHijas[i].y == y)
                    return true;
            }
            return false;
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
        private bool EstaEnLaListaDePasillosHijos(int xSala, int ySala, int xPasillo, int yPasillo)
        {
            for (int i = 0; i < m_ListaPasillosConSalas.Count; i++)
            {
                if (m_ListaPasillosConSalas[i].m_HabitacionPadre.x == xPasillo && m_ListaPasillosConSalas[i].m_HabitacionPadre.y == yPasillo)
                {
                    for (int j = 0; j < m_ListaPasillosConSalas[i].m_HabitacionesHijas.Count; j++)
                    {
                        if (m_ListaPasillosConSalas[i].m_HabitacionesHijas[j].x == xSala && m_ListaPasillosConSalas[i].m_HabitacionesHijas[j].y == ySala)
                            return true;
                    }
                }
            }
            return false;
        }

        public SalasData Save()
        {

            return new SalasData(m_ListaSalasPadreConHijos.ToArray(), m_ListaPasillosConSalas.ToArray());
        }

        public void Load(SalasData _salaData)
        {
            throw new NotImplementedException();
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