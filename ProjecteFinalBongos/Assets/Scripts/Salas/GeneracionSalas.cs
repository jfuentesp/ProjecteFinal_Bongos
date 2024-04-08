using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Rendering.DebugUI.Table;
using Random = UnityEngine.Random;

public class GeneracionSalas : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Sala;
    [SerializeField]
    private GameObject m_Sala_1;
    [SerializeField]
    private GameObject m_Sala_2;
    [SerializeField]
    private GameObject m_Sala_3;
    [SerializeField]
    private GameObject m_Sala_4;
    [SerializeField]
    private GameObject puerta;
    [SerializeField]
    private Tilemap tilemap;

    private List<listaSalas> m_ListaSalasPadre = new List<listaSalas>();
    private List<listaSalas> m_ListaPuertas = new List<listaSalas>();

    public enum TipoSala
    {
        BOSS, INICIAL, LOOT, ENEMIGOS, TIENDA
    }

    private int[,] matrix = new int[100, 100];
    private int numeroDeSala = 0;
    [SerializeField]
    private int maxSala = 20;
    private bool acabado = false;
    // Start is called before the first frame update
    void Start()
    {
        RellenarMatriz();
        GenerarMapa(20, 20, 0);
        PrintMapa();
        GenSala();
        ImprimirListas();
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

    private void PrintMapa()
    {
        string f = "";
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                f += matrix[i, j] + " ";
            }
            f += "\n";
        }
        Debug.Log(f);
    }


    // Update is called once per frame


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

    private void GenerarMapa(int x, int y, int sala)
    {
        if (acabado == false)
        {
            if (maxSala > 0)
            {
                if (numeroDeSala == 0)
                {
                    matrix[x, y] = 1;
                    numeroDeSala++;
                    maxSala--;
                }

                else if (numeroDeSala == 19)
                {

                    matrix[x, y] = 5;
                    numeroDeSala++;
                    maxSala--;
                    acabado = true;
                }
                else
                {
                    matrix[x, y] = sala;
                }


                List<int> PuertasAlrededor;
                GetPuertasAlrededor(out PuertasAlrededor, x, y);
                CambiarMatriz(PuertasAlrededor, x, y);
            }
            else
            {
                Debug.Log("AAAAA");
                return;
            }
        }
       
    }

    private void CambiarMatriz(List<int> puertasAlrededor, int x, int y)
    {
        int salaLado;

        foreach (int i in puertasAlrededor)
        {
            if (maxSala > 0)
            {
                print(maxSala);
                switch (i)
                {
                    case 1:
                        salaLado = Random.Range(2, 5);

                        if (salaLado == matrix[x, y])
                        {

                            ponerSalaEnUno(x, y - 1, salaLado);

                        }
                        else
                        {
                            if (matrix[x, y - 1] == 0)
                            {
                                matrix[x, y - 1] = 9;
                                m_ListaPuertas.Add(new listaSalas(x, y - 1));
                                ponerSalaEnUno(x, y - 2, salaLado);
                                m_ListaSalasPadre.Add(new listaSalas(x, y - 2));
                                numeroDeSala++;
                                maxSala--;
                            }
                        }
                        break;
                    case 2:
                        salaLado = Random.Range(2, 5);
                        if (salaLado == matrix[x, y])
                        {
                            ponerSalaEnUno(x + 1, y, salaLado);
                        }
                        else
                        {
                            if (matrix[x + 1, y] == 0)
                            {
                                matrix[x + 1, y] = 9;
                                m_ListaPuertas.Add(new listaSalas(x + 1, y));
                                ponerSalaEnUno(x + 2, y, salaLado);
                                m_ListaSalasPadre.Add(new listaSalas(x + 2, y));
                                numeroDeSala++;
                                maxSala--;
                            }

                        }
                        break;
                    case 3:
                        salaLado = Random.Range(2, 5);
                        if (salaLado == matrix[x, y])
                        {
                            ponerSalaEnUno(x, y + 1, salaLado);

                        }
                        else
                        {
                            if (matrix[x, y + 1] == 0)
                            {
                                matrix[x, y + 1] = 9;
                                m_ListaPuertas.Add(new listaSalas(x, y + 1));
                                ponerSalaEnUno(x, y + 2, salaLado);
                                m_ListaSalasPadre.Add(new listaSalas(x, y + 2));
                                numeroDeSala++;
                                maxSala--;
                            }
                        }
                        break;
                    case 4:
                        salaLado = Random.Range(2, 5);
                        if (salaLado == matrix[x, y])
                        {
                            ponerSalaEnUno(x - 1, y, salaLado);

                        }
                        else
                        {
                            if (matrix[x - 1, y] == 0)
                            {
                                matrix[x - 1, y] = 9;
                                m_ListaPuertas.Add(new listaSalas(x - 1, y));
                                ponerSalaEnUno(x - 2, y, salaLado);
                                m_ListaSalasPadre.Add(new listaSalas(x - 2, y));
                                numeroDeSala++;
                                maxSala--;
                            }
                        }
                        break;

                }
            }
        }
    }
    private void ponerSalaEnUno(int row, int col, int salaLado)
    {
        //Debug.Log(numeroDeSala);
        if (matrix[row, col] != 0)
            return;

        else
        {
            GenerarMapa(row, col, salaLado);
        }
    }


    private void GetPuertasAlrededor(out List<int> puertas, int x, int y)
    {
        int puertasMax = 5;
        if (matrix[x + 2, y] != 0 || matrix[x + 1, y] != 0) { puertasMax--; }
        if (matrix[x - 2, y] != 0 || matrix[x - 1, y] != 0) { puertasMax--; }
        if (matrix[x, y + 2] != 0 || matrix[x, y + 1] != 0) { puertasMax--; }
        if (matrix[x, y - 2] != 0 || matrix[x, y - 1] != 0) { puertasMax--; }
        puertas = new List<int>();
        int numeroPuertas = Random.Range(1, puertasMax);

        do
        {
            int puerta = Random.Range(1, 5);
            int encontrado = puertas.FirstOrDefault<int>(n => n == puerta);

            if (encontrado == 0)
            {
                puertas.Add(puerta);
                numeroPuertas--;
            }

        } while (numeroPuertas > 0);

        puertas.Shuffle();
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
                sala = Instantiate(m_Sala_1);
                break;
            case 2:
                sala = Instantiate(m_Sala_2);
                break;
            case 3:
                sala = Instantiate(m_Sala_3);
                break;
            case 4:
                sala = Instantiate(m_Sala_4);
                break;
            case 5:
                sala = Instantiate(m_Sala);
                break;
            case 9:
                sala = Instantiate(puerta);
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
