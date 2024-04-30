using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static GeneracionSalas.GeneracionSalasMatriz;

namespace GeneracionSalas
{
    public class GeneracionSalaInstanciacion : MonoBehaviour
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

        [Header("Tiles y Tilemap")]
        [SerializeField] private Tilemap m_TilemapSuelo;
        [SerializeField] private Tilemap m_TilemapPared;
        [SerializeField] private TileBase m_TileSalas;
        [SerializeField] private TileBase m_TilePared;
        private List<Vector2> m_TilesSala = new();
        private List<Vector2> m_TilesPared = new();

        public Action onMapaFinalized;
        private List<ListaSalasConHijos> m_ListaSalasPadreConHijos = new();
        private List<ListaSalasConHijos> m_ListaPasillosConSalas = new();

        private int[,] matrix = new int[100, 100];

        public void InstanciarElMundo(int[,] _matrix, List<ListaSalasConHijos> _ListaSalasPadreConHijos, List<ListaSalasConHijos> _ListaPasillosConSalas)
        {
            matrix = _matrix;
            m_ListaSalasPadreConHijos = _ListaSalasPadreConHijos;
            m_ListaPasillosConSalas = _ListaPasillosConSalas;
            GenSalasBoss();
            GenPasillos();
            PintarTilemap();
            onMapaFinalized?.Invoke();
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
                Transform TransformPadre;
                InstanciarSalaJefeInicial(salaPadre.m_HabitacionPadre.x + 50, salaPadre.m_HabitacionPadre.y + 50, salaPadre.m_HabitacionesHijas, salaPadre, out TransformPadre);
                for (int i = 0; i < salaPadre.m_HabitacionesHijas.Count - 1; i++)
                {
                    InstanciarSalaJefes(salaPadre.m_HabitacionesHijas[i].x + 50, salaPadre.m_HabitacionesHijas[i].y + 50, salaPadre.m_HabitacionesHijas, TransformPadre);
                }
            }
        }

        private void InstanciarSalaJefeInicial(int x, int y, List<ListaSalas> salasHijas, ListaSalasConHijos salaPadre, out Transform TransformPadre)
        {
            float posicionX = (x - 50) * 20;
            float posicionY = (y - 50) * 20;
            GameObject sala = null;
            TransformPadre = null;

            switch (matrix[x, y])
            {
                case 1:
                    sala = Instantiate(m_SalaInicial, m_TransformParentMundo);
                    sala.transform.position = new Vector3(posicionX, posicionY, 0);
                    break;
                case 2:
                    sala = Instantiate(m_SalaBossInicial, m_TransformParentMundo);
                    sala.transform.position = new Vector3(posicionX, posicionY, 0);
                    if (GameManager.Instance.NuevaPartida)
                        sala.GetComponent<SalaBoss>().Init(salaPadre, LevelManager.Instance.GetAvailableBoss());
                    break;
            }
            if (sala != null)
            {
                TransformPadre = sala.transform;
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
                    if (GameManager.Instance.NuevaPartida)
                        sala.GetComponent<PasilloTienda>().Init();
                    break;
                case 5:
                    sala = Instantiate(m_PasilloObjetos, m_TransformParentMundo);
                    if (GameManager.Instance.NuevaPartida)
                        sala.GetComponent<PasilloObjetos>().Init(LevelManager.Instance.GetObjetosSalaObjetos());
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

        private void InstanciarSalaJefes(int x, int y, List<ListaSalas> salasHijas, Transform TransformPadre)
        {
            float posicionX = (x - 50) * 20;
            float posicionY = (y - 50) * 20;
            GameObject sala = null;

            switch (matrix[x, y])
            {
                case 9:
                    sala = Instantiate(m_SalaBoss, TransformPadre);
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
                for (int i = (int)posicionX - 10; i < posicionX + 10; i++)
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
                    for (int j = (int)posicionY - 10; j < posicionY + 10; j++)
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
                    m_TilesPared.Add(new Vector2(posMundoX + 9, posMundoY - 10 + i));
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
                m_TilesSala.Add(new Vector2(posMundoX + 9, posMundoY - 1));
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
                Vector3Int posicionInt = m_TilemapSuelo.WorldToCell(posicion);
                m_TilemapSuelo.SetTile(posicionInt, m_TileSalas);
            }
            foreach (Vector2 posicion in m_TilesPared)
            {
                Vector3Int posicionInt = m_TilemapSuelo.WorldToCell(posicion);
                m_TilemapPared.SetTile(posicionInt, m_TilePared);
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
    }
}