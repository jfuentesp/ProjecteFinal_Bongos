using GeneracionSalas;
using NavMeshPlus.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PruebaNavMesh : MonoBehaviour
{
    [SerializeField]
    GeneracionSalaInstanciacion m_Mapa;
    private NavMeshSurface surface;
    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
        m_Mapa.onMapaFinalized += ConstruirMapa;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (surface == null)
        {
            Debug.LogError("NavMeshSurface no está asignado en el inspector.");
            return;
        }
    }

    private void ConstruirMapa()
    {
        BakeNavMesh();
    }

    void BakeNavMesh()
    {
        NavMesh.RemoveAllNavMeshData();
        surface.BuildNavMesh();
    }
}