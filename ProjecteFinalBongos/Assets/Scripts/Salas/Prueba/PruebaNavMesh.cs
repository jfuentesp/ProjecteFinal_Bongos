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
    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
        m_Mapa.onMapaFinalized += ConstruirMapa;

    }
    // Start is called before the first frame update
    void Start()
    {
        // Asegúrate de tener la superficie del NavMesh asignada en el inspector
        if (surface == null)
        {
            Debug.LogError("NavMeshSurface no está asignado en el inspector.");
            return;
        }
    }
    /* private void Update()
     {
         if (canUpdate)
         {
             print("Update");
             //BakeNavMesh();
         }
     }
 */
    private void ConstruirMapa()
    {
        // Haz el bake del NavMesh
        BakeNavMesh();
    }

    void BakeNavMesh()
    {
        // Borra cualquier NavMesh existente
        NavMesh.RemoveAllNavMeshData();

        // Haz el bake del NavMesh
        surface.BuildNavMesh();
    }
}