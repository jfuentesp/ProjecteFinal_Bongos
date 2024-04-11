using GeneracionSalas;
using NavMeshPlus.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshPrueba : MonoBehaviour
{
    [SerializeField]
    GeneracionSalasFinal m_Mapa;

    NavMeshSurface m_Surface;

    private void Awake()
    {
        m_Surface = GetComponent<NavMeshSurface>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Mapa.onMapaFinalized += GenNavMesh;
    }

    private void GenNavMesh()
    {
        NavMesh.RemoveAllNavMeshData();
        m_Surface.BuildNavMesh();
    }

}
