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
    GeneracionSalasFinal m_Mapa;
    private NavMeshSurface surface;
    private bool canUpdate;
    private void Awake()
    {
        canUpdate = false;
        surface = GetComponent<NavMeshSurface>();
        //m_Mapa.onMapaFinalized += ConstruirMapa;

    }
    // Start is called before the first frame update
    void Start()
    {
        // Aseg�rate de tener la superficie del NavMesh asignada en el inspector
        if (surface == null)
        {
            Debug.LogError("NavMeshSurface no est� asignado en el inspector.");
            return;
        }
    }
    private void Update()
    {
        if (canUpdate)
        {
            print("Update");
            BakeNavMesh();
        }
    }
    private void ConstruirMapa()
    {
        print("eo");
        // Haz el bake del NavMesh
        canUpdate = true;
        BakeNavMesh();
    }

    void BakeNavMesh()
    {
        // Borra cualquier NavMesh existente
        //NavMesh.RemoveAllNavMeshData();

        // Haz el bake del NavMesh
        surface.BuildNavMesh();
    }
}