using GeneracionSalas;
using NavMeshPlus.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PruebaNavMesh : MonoBehaviour
{
    GeneracionSalaInstanciacion m_Mapa;
    public Action OnEndNavmesh;

    private NavMeshSurface surface;
    private void Awake()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        print("Awake del navmesh");
        surface = GetComponent<NavMeshSurface>();
        print(LevelManager.Instance == null);
        m_Mapa = LevelManager.Instance.GeneracionSalasInstanciacion;
        m_Mapa.onMapaFinalized += ConstruirMapa;
        if (surface == null)
        {
            Debug.LogError("NavMeshSurface no est� asignado en el inspector.");
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
        OnEndNavmesh?.Invoke();
    }
}
