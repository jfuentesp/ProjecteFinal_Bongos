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
    private bool canUpdate;
    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
        canUpdate = false;
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
    private void Update()
    {
        if (canUpdate)
        {
            BakeNavMesh();
        }
    }
    private void ConstruirMapa()
    {
        canUpdate = true;
        BakeNavMesh();
    }

    void BakeNavMesh()
    {
        //NavMesh.RemoveAllNavMeshData();
        surface.BuildNavMesh();
    }
}