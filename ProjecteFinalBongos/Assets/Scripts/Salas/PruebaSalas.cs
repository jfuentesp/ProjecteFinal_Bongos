using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PruebaSalas : MonoBehaviour
{
    [SerializeField] private GameObject m_SalaInicial;
    [SerializeField] private GameObject m_SalaBoss;
    [SerializeField] private GameObject m_PasilloBichos;
    [SerializeField] private GameObject m_PasilloTienda;
    [SerializeField] private GameObject m_PasilloObjetos;

    private int[,] matriz = new int[100, 100];
    private List<Vector2Int> m_Salas = new List<Vector2Int>();
    private List<Vector2Int> m_Pasillos = new List<Vector2Int>();

    private const int SALA_INICIAL = 1;
    private const int SALA_BOSS = 2;
    private const int PASILLO_BICHOS = 3;
    private const int PASILLO_TIENDA = 4;
    private const int PASILLO_OBJETOS = 5;

    private void Start()
    {
        GenerarMapa(20, 20, SALA_INICIAL);
        InstanciarMapa();
    }

    private void GenerarMapa(int posX, int posY, int tipoSala)
    {
        if (tipoSala != SALA_INICIAL)
            matriz[posX, posY] = tipoSala;

        m_Salas.Add(new Vector2Int(posX, posY));

        List<int> posiblesPasillosAlrededor = ObtenerPasillosDisponibles(posX, posY);
        foreach (int pasillo in posiblesPasillosAlrededor)
        {
            int nuevoX = posX;
            int nuevoY = posY;

            switch (pasillo)
            {
                case 1:
                    nuevoY -= 1;
                    break;
                case 2:
                    nuevoX += 1;
                    break;
                case 3:
                    nuevoY += 1;
                    break;
                case 4:
                    nuevoX -= 1;
                    break;
            }

            if (matriz[nuevoX, nuevoY] == 0)
            {
                matriz[nuevoX, nuevoY] = ObtenerTipoPasillo();
                m_Pasillos.Add(new Vector2Int(nuevoX, nuevoY));
                GenerarMapa(nuevoX, nuevoY, ObtenerSiguienteTipoSala(tipoSala));
            }
        }
    }

    private List<int> ObtenerPasillosDisponibles(int posX, int posY)
    {
        List<int> pasillosDisponibles = new List<int>();

        if (posY > 0 && matriz[posX, posY - 1] == 0)
            pasillosDisponibles.Add(1);
        if (posX < matriz.GetLength(0) - 1 && matriz[posX + 1, posY] == 0)
            pasillosDisponibles.Add(2);
        if (posY < matriz.GetLength(1) - 1 && matriz[posX, posY + 1] == 0)
            pasillosDisponibles.Add(3);
        if (posX > 0 && matriz[posX - 1, posY] == 0)
            pasillosDisponibles.Add(4);

        return pasillosDisponibles;
    }

    private int ObtenerTipoPasillo()
    {
        return Random.Range(PASILLO_BICHOS, PASILLO_OBJETOS + 1);
    }

    private int ObtenerSiguienteTipoSala(int tipoSalaActual)
    {
        if (tipoSalaActual == SALA_INICIAL)
            return SALA_BOSS;
        else
            return Random.Range(PASILLO_BICHOS, PASILLO_OBJETOS + 1);
    }

    private void InstanciarMapa()
    {
        foreach (Vector2Int salaPos in m_Salas)
        {
            InstanciarSala(salaPos.x, salaPos.y, matriz[salaPos.x, salaPos.y]);
        }

        foreach (Vector2Int pasilloPos in m_Pasillos)
        {
            InstanciarPasillo(pasilloPos.x, pasilloPos.y, matriz[pasilloPos.x, pasilloPos.y]);
        }
    }

    private void InstanciarSala(int x, int y, int tipoSala)
    {
        GameObject salaPrefab = ObtenerPrefabPorTipo(tipoSala);
        if (salaPrefab != null)
        {
            Vector3 posicion = new Vector3(x, y, 0);
            Instantiate(salaPrefab, posicion, Quaternion.identity);
        }
    }

    private void InstanciarPasillo(int x, int y, int tipoPasillo)
    {
        GameObject pasilloPrefab = ObtenerPrefabPorTipo(tipoPasillo);
        if (pasilloPrefab != null)
        {
            Vector3 posicion = new Vector3(x, y, 0);
            Instantiate(pasilloPrefab, posicion, Quaternion.identity);
        }
    }

    private GameObject ObtenerPrefabPorTipo(int tipo)
    {
        switch (tipo)
        {
            case SALA_INICIAL:
                return m_SalaInicial;
            case SALA_BOSS:
                return m_SalaBoss;
            case PASILLO_BICHOS:
                return m_PasilloBichos;
            case PASILLO_TIENDA:
                return m_PasilloTienda;
            case PASILLO_OBJETOS:
                return m_PasilloObjetos;
            default:
                return null;
        }
    }
}
