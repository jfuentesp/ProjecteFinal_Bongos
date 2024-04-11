using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static SaveGame;
using Random = UnityEngine.Random;

namespace GeneracionSalas
{
    public class GeneracionSalasFinal : MonoBehaviour, ISaveableSalaData
    {
        [Header("Parent Mundo")]
        [SerializeField] private Transform m_TransformParentMundo;

        [Header("Prefabs Paredes")]
        [SerializeField] private GameObject m_ParedArriba;
        [SerializeField] private GameObject m_ParedDerecha;
        [SerializeField] private GameObject m_ParedAbajo;
        [SerializeField] private GameObject m_ParedIzquierda;

        [Header("Prefabs Puertas")]
        [SerializeField] private GameObject m_PuertaArriba;
        [SerializeField] private GameObject m_PuertaDerecha;
        [SerializeField] private GameObject m_PuertaAbajo;
        [SerializeField] private GameObject m_PuertaIzquierda;

        [Header("Color Sala")]
        [SerializeField] private Color m_SalaInicialColor;
        [SerializeField] private Color m_SalaBossInicialColor;
        [SerializeField] private Color m_SalaBossColor;
        [SerializeField] private Color m_PasilloBichosColor;
        [SerializeField] private Color m_PasilloObjetosColor;
        [SerializeField] private Color m_PasilloTiendaColor;

        [Header("GameObject Sala")]
        [SerializeField] GameObject m_SalaInicial;
        [SerializeField] private GameObject m_SalaBossInicial;
        [SerializeField] private GameObject m_SalaBoss;
        [SerializeField] private GameObject m_PasilloBichos;
        [SerializeField] private GameObject m_PasilloObjetos;
        [SerializeField] private GameObject m_PasilloTienda;

        private List<ListaSalas> m_ListaSalasPadre = new List<ListaSalas>();
        private List<ListaSalasConHijos> m_ListaSalasPadreConHijos = new List<ListaSalasConHijos>();
        private List<ListaSalasConHijos> m_ListaPasillosConSalas = new List<ListaSalasConHijos>();

        public Action onMapaFinalized;

        [Header("Numeros Sala")]
        [SerializeField] private int numSala;
        [SerializeField] private int numSalaMaxima;
        private int[,] matrix = new int[100, 100];
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
                InstanciarSalaJefes(salaPadre.m_HabitacionPadre.x + 50, salaPadre.m_HabitacionPadre.y + 50, salaPadre.m_HabitacionesHijas);
                foreach (ListaSalas salaHija in salaPadre.m_HabitacionesHijas)
                    InstanciarSalaJefes(salaHija.x + 50, salaHija.y + 50, salaPadre.m_HabitacionesHijas);
            }
        }
        private void PintarSalas(int tipoSala, List<GameObject> estructuras)
        {
            switch (tipoSala)
            {
                case 1:
                    PintaSalaConColor(estructuras, m_SalaInicialColor);
                    break;
                case 2:
                    PintaSalaConColor(estructuras, m_SalaBossInicialColor);
                    break;
                case 3:
                    PintaSalaConColor(estructuras, m_PasilloBichosColor);
                    break;
                case 4:
                    PintaSalaConColor(estructuras, m_PasilloTiendaColor);
                    break;
                case 5:
                    PintaSalaConColor(estructuras, m_PasilloObjetosColor);
                    break;
                case 9:
                    PintaSalaConColor(estructuras, m_SalaBossColor);
                    break;
            }
        }
        private void PintaSalaConColor(List<GameObject> estructuras, Color _ColorSala)
        {
            foreach (GameObject estructura in estructuras)
            {
                if (estructura.transform.childCount > 0)
                {
                    estructura.transform.GetChild(0).GetComponent<SpriteRenderer>().color = _ColorSala;
                    estructura.transform.GetChild(1).GetComponent<SpriteRenderer>().color = _ColorSala;
                }
                else
                    estructura.GetComponent<SpriteRenderer>().color = _ColorSala;
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
                InstanciarParedesAndPuertasPasillo(x, y, sala.transform, matrix[x, y], habitacionesHijas);
            }
        }
        private void InstanciarSalaJefes(int x, int y, List<ListaSalas> salasHijas)
        {
            float posicionX = (x - 50) * 20;
            float posicionY = (y - 50) * 20;
            GameObject sala = null;

            switch (matrix[x, y])
            {
                case 1:
                    sala = Instantiate(m_SalaInicial, m_TransformParentMundo);
                    break;
                case 2:
                    sala = Instantiate(m_SalaBossInicial, m_TransformParentMundo);
                    break;
                case 9:
                    sala = Instantiate(m_SalaBoss, m_TransformParentMundo);
                    break;
                default:
                    break;
            }
            if (sala != null)
            {
                sala.transform.position = new Vector3(posicionX, posicionY, 0);
                InstanciarParedesAndPuertasBoss(x, y, sala.transform, matrix[x, y], salasHijas);
            }
        }
        private void InstanciarParedesAndPuertasPasillo(int posicionX, int posicionY, Transform transformSala, int tipoSala, List<ListaSalas> salasHijas)
        {
            GameObject estructuraArriba;
            GameObject estructuraDerecha;
            GameObject estructuraAbajo;
            GameObject estructuraIzquierda;
            List<GameObject> estructuras = new List<GameObject>();
            if (!EstaENLaListaDeSalas(posicionX - 50, posicionY + 1 - 50, salasHijas))
            {
                estructuraArriba = Instantiate(m_ParedArriba, transformSala);
                estructuras.Add(estructuraArriba);
            }
            else
            {
                estructuraArriba = Instantiate(m_PuertaArriba, transformSala);
                estructuras.Add(estructuraArriba);
            }
            if (!EstaENLaListaDeSalas(posicionX + 1 - 50, posicionY - 50, salasHijas))
            {
                estructuraDerecha = Instantiate(m_ParedDerecha, transformSala);
                estructuras.Add(estructuraDerecha);
            }
            else
            {
                estructuraDerecha = Instantiate(m_PuertaDerecha, transformSala);
                estructuras.Add(estructuraDerecha);
            }
            if (!EstaENLaListaDeSalas(posicionX - 50, posicionY - 1 - 50, salasHijas))
            {
                estructuraAbajo = Instantiate(m_ParedAbajo, transformSala);
                estructuras.Add(estructuraAbajo);
            }
            else
            {
                estructuraAbajo = Instantiate(m_PuertaAbajo, transformSala);
                estructuras.Add(estructuraAbajo);
            }
            if (!EstaENLaListaDeSalas(posicionX - 1 - 50, posicionY - 50, salasHijas))
            {
                estructuraIzquierda = Instantiate(m_ParedIzquierda, transformSala);
                estructuras.Add(estructuraIzquierda);
            }
            else
            {
                estructuraIzquierda = Instantiate(m_PuertaIzquierda, transformSala);
                estructuras.Add(estructuraIzquierda);
            }
            PintarSalas(tipoSala, estructuras);
        }
        private void InstanciarParedesAndPuertasBoss(int posicionX, int posicionY, Transform transformSala, int tipoSala, List<ListaSalas> salasHijas)
        {
            GameObject estructuraArriba;
            GameObject estructuraDerecha;
            GameObject estructuraAbajo;
            GameObject estructuraIzquierda;
            List<GameObject> estructuras = new List<GameObject>();

            if (EstaEnLaListaDePasillosHijos(posicionX - 50, posicionY - 50, posicionX - 50, posicionY + 1 - 50))
            {
                estructuraArriba = Instantiate(m_PuertaArriba);
                estructuras.Add(estructuraArriba);
            }
            else if (!EstaENLaListaDeSalas(posicionX - 50, posicionY + 1 - 50, salasHijas))
            {
                estructuraArriba = Instantiate(m_ParedArriba);
                estructuras.Add(estructuraArriba);
            }

            if (EstaEnLaListaDePasillosHijos(posicionX - 50, posicionY - 50, posicionX + 1 - 50, posicionY - 50))
            {
                estructuraDerecha = Instantiate(m_PuertaDerecha);
                estructuras.Add(estructuraDerecha);
            }
            else if (!EstaENLaListaDeSalas(posicionX + 1 - 50, posicionY - 50, salasHijas))
            {
                estructuraDerecha = Instantiate(m_ParedDerecha);
                estructuras.Add(estructuraDerecha);
            }

            if (EstaEnLaListaDePasillosHijos(posicionX - 50, posicionY - 50, posicionX - 50, posicionY - 1 - 50))
            {
                estructuraAbajo = Instantiate(m_PuertaAbajo);
                estructuras.Add(estructuraAbajo);
            }
            else if (!EstaENLaListaDeSalas(posicionX - 50, posicionY - 1 - 50, salasHijas))
            {
                estructuraAbajo = Instantiate(m_ParedAbajo);
                estructuras.Add(estructuraAbajo);
            }

            if (EstaEnLaListaDePasillosHijos(posicionX - 50, posicionY - 50, posicionX - 1 - 50, posicionY - 50))
            {
                estructuraIzquierda = Instantiate(m_PuertaIzquierda);
                estructuras.Add(estructuraIzquierda);
            }
            else if (!EstaENLaListaDeSalas(posicionX - 1 - 50, posicionY - 50, salasHijas))
            {
                estructuraIzquierda = Instantiate(m_ParedIzquierda);
                estructuras.Add(estructuraIzquierda);
            }
            foreach(GameObject estructura in estructuras)
            {
                estructura.transform.parent = transformSala;
                estructura.transform.position = transformSala.position;
            }
            PintarSalas(tipoSala, estructuras);
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