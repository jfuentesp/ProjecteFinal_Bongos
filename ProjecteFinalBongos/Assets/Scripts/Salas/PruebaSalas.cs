using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class PruebaSalas : MonoBehaviour
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

    private List<listaSalas> m_ListaSalasPadre = new List<listaSalas>();
    [SerializeField]
    private List<listaSalasConHijos> m_ListaSalasPadreConHijos = new List<listaSalasConHijos>();

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
            numSala = numSalaMaxima;
            RellenarMatriz();
            GenerarMapa(50, 50, 0);
            matrix[m_ListaSalasPadre[m_ListaSalasPadre.Count - 1].x + 50, m_ListaSalasPadre[m_ListaSalasPadre.Count - 1].y + 50] = 2;
            if (numSala != 0)
            {
                Start();
            }
            else
            {
                ImprimirListas();
                GenSala();
            }
        }
        catch (Exception)
        {
            Start();
        }
    }

    private void ImprimirListas()
    {
        string frase = "";
        foreach (listaSalas sala in m_ListaSalasPadre)
        {
            frase += $"[{sala.x}, {sala.y}], ";
        }
        print(frase);
    }

    private void RellenarMatriz()
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j] = 0;
            }
        }
    }

    private void GenerarMapa(int posX, int posY, int tipoSalaBoss)
    {
        if (numSala > 0)
        {

            if (numSala == numSalaMaxima)
            {
                matrix[posX, posY] = 1;
            }
            else if (numSala == 0)
            {
                matrix[posX, posY] = 5;
            }
            else { matrix[posX, posY] = tipoSalaBoss; }

            List<int> pasillosAlrededor;
            GetPasillosAlrededor(out pasillosAlrededor, posX, posY);
            CambiarMatriz(pasillosAlrededor, posX, posY);
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
            {
                pasillosAlrededor.Add(posiblesPasillosAlrededor[i]);
            }
        }
        else
        {
            for (int i = 0; i < numSala; i++)
            {
                pasillosAlrededor.Add(posiblesPasillosAlrededor[i]);
            }
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
    private void CambiarMatriz(List<int> puertasAlrededor, int posX, int posY)
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
                    SalaLateral(posX, posY, posX, posY - 1, posX, posY - 2, salaLado);
                    break;
                case 2:
                    SalaLateral(posX, posY, posX + 1, posY, posX + 2, posY, salaLado);
                    break;
                case 3:
                    SalaLateral(posX, posY, posX, posY + 1, posX, posY + 2, salaLado);
                    break;
                case 4:
                    SalaLateral(posX, posY, posX - 1, posY, posX - 2, posY, salaLado);
                    break;
            }
        }
    }

    private void SalaLateral(int posX1, int posY1, int posX2, int posY2, int posX3, int posY3, int salaLado)
    {
        if (2 == salaLado)
        {
            if (2 == matrix[posX1, posY1] || 9 == matrix[posX1, posY1])
            {
                AmpliarSala(posX2, posY2);
                //AddSalaPadre(posX2 - 50, posY2 - 50);
            }
        }
        else
        {
            if (numSala > 0 && matrix[posX2, posY2] == 0 && matrix[posX3, posY3] == 0)
            {
                matrix[posX2, posY2] = salaLado;

                listaSalas salaEncontrada = m_ListaSalasPadre.FirstOrDefault<listaSalas>(i => i.x == posX3 && i.y == posY3);
                if (salaEncontrada.x == 0 && salaEncontrada.y == 0)
                {
                    m_ListaSalasPadre.Add(new listaSalas(posX3 - 50, posY3 - 50));
                    listaSalas salaPadre = new listaSalas(posX3 - 50, posY3 - 50);
                    m_ListaSalasPadreConHijos.Add(new listaSalasConHijos(new listaSalas(posX3 - 50, posY3 - 50), new List<listaSalas>()));

                    PonerNumeroSala(posX3, posY3);
                }

            }
        }
    }
    /*
    private void AddSalaPadre(int posX2, int posY2)
    {
        listaSalasConHijos salaEncontrada = m_ListaSalasPadreConHijos.FirstOrDefault<listaSalasConHijos>(i => i.m_HabitacionPadre.x == salaPadre.x && i.m_HabitacionPadre.y == salaPadre.y);
        if (salaEncontrada.m_HabitacionPadre.x != 0 && salaEncontrada.m_HabitacionPadre.y != 0)
        {
            for (int i = 0; i < m_ListaSalasPadreConHijos.Count; i++)
            {
                if (m_ListaSalasPadreConHijos[i].m_HabitacionPadre.x == salaEncontrada.m_HabitacionPadre.x && m_ListaSalasPadreConHijos[i].m_HabitacionPadre.y == salaEncontrada.m_HabitacionPadre.y)
                {
                    m_ListaSalasPadreConHijos[i].m_HabitacionesHijas.Add(new listaSalas(posX2, posY2));
                }
            }
        }
    }*/

    private void AmpliarSala(int row, int col)
    {
        if (matrix[row, col] != 0)
            return;

        GenerarMapa(row, col, 9);
    }
    private void PonerNumeroSala(int row, int col)
    {
        if (matrix[row, col] != 0 && numSala > 0)
            return;

        numSala--;
        GenerarMapa(row, col, 2);
    }
    public void GenSala()
    {
        for (int x = 0; x < matrix.GetLength(0); x++)
        {
            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                if (matrix[x, y] != 0)
                    InstanciarSala(x, y, matrix[x, y]);
            }
        }
    }
    private void InstanciarSala(int x, int y, int tipoSala)
    {
        float posicionX = (x - 50) * 12;
        float posicionY = (y - 50) * 11;
        GameObject sala = null;

        switch (tipoSala)
        {
            case 1:
                sala = Instantiate(m_SalaInicial, m_TransformParentMundo);
                break;
            case 2:
                sala = Instantiate(m_SalaBossInicial, m_TransformParentMundo);
                break;
            case 3:
                sala = Instantiate(m_PasilloBichos, m_TransformParentMundo);
                break;
            case 4:
                sala = Instantiate(m_PasilloTienda, m_TransformParentMundo);
                break;
            case 5:
                sala = Instantiate(m_PasilloObjetos, m_TransformParentMundo);
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
            InstanciarParedesAndPuertas(x, y, sala.transform, tipoSala);
        }
    }

    private void InstanciarParedesAndPuertas(int posicionX, int posicionY, Transform transformSala, int tipoSala)
    {
        GameObject estructuraArriba;
        GameObject estructuraDerecha;
        GameObject estructuraAbajo;
        GameObject estructuraIzquierda;
        List<GameObject> estructuras = new List<GameObject>();
        if (matrix[posicionX, posicionY + 1] != 0)
        {
            estructuraArriba = Instantiate(m_PuertaArriba, transformSala);
            estructuras.Add(estructuraArriba);
        }
        else
        {
            estructuraArriba = Instantiate(m_ParedArriba, transformSala);
            estructuras.Add(estructuraArriba);
        }
        if (matrix[posicionX + 1, posicionY] != 0)
        {
            estructuraDerecha = Instantiate(m_PuertaDerecha, transformSala);
            estructuras.Add(estructuraDerecha);
        }
        else
        {
            estructuraDerecha = Instantiate(m_ParedDerecha, transformSala);
            estructuras.Add(estructuraDerecha);
        }
        if (matrix[posicionX, posicionY - 1] != 0)
        {
            estructuraAbajo = Instantiate(m_PuertaAbajo, transformSala);
            estructuras.Add(estructuraAbajo);
        }
        else
        {
            estructuraAbajo = Instantiate(m_ParedAbajo, transformSala);
            estructuras.Add(estructuraAbajo);
        }
        if (matrix[posicionX - 1, posicionY] != 0)
        {
            estructuraIzquierda = Instantiate(m_PuertaIzquierda, transformSala);
            estructuras.Add(estructuraIzquierda);
        }
        else
        {
            estructuraIzquierda = Instantiate(m_ParedIzquierda, transformSala);
            estructuras.Add(estructuraIzquierda);
        }
        PintarSalas(tipoSala, estructuras);
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
            {
                estructura.GetComponent<SpriteRenderer>().color = _ColorSala;
            }
        }
    }
    [Serializable]
    private struct listaSalas
    {
        public int x, y;

        public listaSalas(int X, int Y)
        {
            x = X; y = Y;
        }
    }
    [Serializable]
    private struct listaSalasConHijos
    {
        public listaSalas m_HabitacionPadre;
        public List<listaSalas> m_HabitacionesHijas;

        public listaSalasConHijos(listaSalas _HabitacionPadre, List<listaSalas> _HabitacionesHijas)
        {
            m_HabitacionPadre = _HabitacionPadre;
            m_HabitacionesHijas = _HabitacionesHijas;
        }
    }
}
