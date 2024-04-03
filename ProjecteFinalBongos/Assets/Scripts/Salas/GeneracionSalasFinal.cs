using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GeneracionSalasFinal : MonoBehaviour
{
    [SerializeField]
    private GameObject m_SalaInicial;
    [SerializeField]
    private GameObject m_SalaBoss;
    [SerializeField]
    private GameObject m_SalaBossInicial;
    [SerializeField]
    private GameObject puerta;
    [SerializeField]
    private GameObject m_PasilloTienda;
    [SerializeField]
    private GameObject m_PasilloBichos;
    [SerializeField]
    private GameObject m_PasilloObjetos;

    private List<listaSalas> m_ListaSalasPadre = new List<listaSalas>();
    private List<listaSalas> m_ListaPuertas = new List<listaSalas>();


    [SerializeField]
    private int numSala;
    [SerializeField]
    private int numSalaMaxima;
    private int[,] matrix = new int[100, 100];
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            m_ListaPuertas.Clear();
            m_ListaSalasPadre.Clear();
            numSala = numSalaMaxima;
            RellenarMatriz();
            GenerarMapa(20, 20, 0);
            if(numSala != 0)
            {
                Start();
            }
            matrix[m_ListaSalasPadre[m_ListaSalasPadre.Count - 1].x, m_ListaSalasPadre[m_ListaSalasPadre.Count - 1].y] = 2;
            PrintMapa();
            GenSala();
            ImprimirListas();
        }
        catch (Exception ex)
        {
            print(ex.ToString());
            Start();
        }

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
        int numeroPuertas = Random.Range(1, GetPosiblesPuertasAlrededor(posX, posY));

        do
        {
            int puerta = Random.Range(1, 5);
            int encontrado = pasillosAlrededor.FirstOrDefault<int>(n => n == puerta);

            if (encontrado == 0)
            {
                pasillosAlrededor.Add(puerta);
                numeroPuertas--;
            }
        } while (numeroPuertas > 0);

        pasillosAlrededor.Shuffle();
    }

    private int GetPosiblesPuertasAlrededor(int posX, int posY)
    {
        int puertasMax = 5;
        if (matrix[posX + 2, posY] != 0 || matrix[posX + 1, posY] != 0) { puertasMax--; }
        if (matrix[posX - 2, posY] != 0 || matrix[posX - 1, posY] != 0) { puertasMax--; }
        if (matrix[posX, posY + 2] != 0 || matrix[posX, posY + 1] != 0) { puertasMax--; }
        if (matrix[posX, posY - 2] != 0 || matrix[posX, posY - 1] != 0) { puertasMax--; }
        if (puertasMax > numSala)
        {
            print(numSala);
            return numSala;
        }
        else
        {
            print(puertasMax);
            return puertasMax;
        }
        
    }
    private void CambiarMatriz(List<int> puertasAlrededor, int posX, int posY)
    {
        int salaLado;

        foreach (int puerta in puertasAlrededor)
        {
            switch (puerta)
            {
                case 1:
                    salaLado = Random.Range(3, 5);
                    SalaLateral(posX, posY, posX, posY - 1, posX, posY - 2, salaLado);
                    break;
                case 2:
                    salaLado = Random.Range(3, 5);
                    SalaLateral(posX, posY, posX + 1, posY, posX + 2, posY, salaLado);
                    break;
                case 3:
                    salaLado = Random.Range(3, 5);
                    SalaLateral(posX, posY, posX, posY + 1, posX, posY + 2, salaLado);
                    break;
                case 4:
                    salaLado = Random.Range(3, 5);
                    SalaLateral(posX, posY, posX - 1, posY, posX - 2, posY, salaLado);
                    break;
            }
        }
    }

    private void SalaLateral(int posX1, int posY1, int posX2, int posY2, int posX3, int posY3, int salaLado)
    {
        if (salaLado == matrix[posX1, posY1])
        {
            print($"posicio inicial: {posX1}, {posY1}, posicio final: {posX2}, {posY2}");
            AmpliarSala(posX2, posY2);
        }
        else
        {
            if(numSala > 0)
            {
                print($"posicio inicial: {posX1}, {posY1}, posicio final: {posX3}, {posY3}");
                matrix[posX2, posY2] = salaLado;

                listaSalas salaEncontrada = m_ListaSalasPadre.FirstOrDefault<listaSalas>(i => i.x == posX3 && i.y == posY3);
                if (salaEncontrada.x == 0 && salaEncontrada.y == 0)
                    m_ListaSalasPadre.Add(new listaSalas(posX3, posY3));
                listaSalas puertaEncontrada = m_ListaSalasPadre.FirstOrDefault<listaSalas>(i => i.x == posX2 && i.y == posY2);
                if (puertaEncontrada.x == 0 && puertaEncontrada.y == 0)
                    m_ListaPuertas.Add(new listaSalas(posX2, posY2));

                PonerNumeroSala(posX3, posY3);
               
            }
        }
    }
    private void AmpliarSala(int row, int col)
    {
        //Debug.Log(numeroDeSala);
        if (matrix[row, col] != 0 && numSala > 0)
            return;

        GenerarMapa(row, col, 2);
    }
    private void PonerNumeroSala(int row, int col)
    {
        //Debug.Log(numeroDeSala);
        if (matrix[row, col] != 0)
            return;

        numSala--;
        GenerarMapa(row, col, 2);
    }

    private void PrintMapa()
    {
        string f = "";
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                f += matrix[j, i] + " ";
            }
            f += "\n";
        }
        Debug.Log(f);
    }

    private void ImprimirListas()
    {
        string listaSalas = "";
        string listaPuertas = "";

        foreach (listaSalas sala in m_ListaSalasPadre)
        {
            listaSalas += $" [{sala.x}, {sala.y}],";
        }
        foreach (listaSalas puerta in m_ListaPuertas)
        {
            listaPuertas += $" [{puerta.x}, {puerta.y}],";
        }
        print(listaSalas + "Total Salas: " + listaSalas.Length);
        print(listaPuertas + "Total Puertas: " + listaPuertas.Length);
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
        int posicionX = ((20 - x) * 1);
        int posicionY = ((20 - y) * 1);
        GameObject sala = null;

        switch (tipoSala)
        {
            case 1:
                sala = Instantiate(m_SalaInicial);
                break;
            case 2:
                listaSalas encontrado = m_ListaSalasPadre.FirstOrDefault<listaSalas>(i => i.x == x && i.y == y);
                if (encontrado.x != 0 && encontrado.y != 0)
                    sala = Instantiate(m_SalaBossInicial);
                else
                    sala = Instantiate(m_SalaBoss);
                break;
            case 3:
                sala = Instantiate(m_PasilloBichos);
                break;
            case 4:
                sala = Instantiate(m_PasilloTienda);
                break;
            case 5:
                sala = Instantiate(m_PasilloObjetos);
                break;
            case 9:
                sala = Instantiate(m_SalaBoss);
                break;
            default:
                break;

        }
        if (sala != null)
        {
            sala.transform.position = new Vector3(posicionX, posicionY, 0);
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
}
