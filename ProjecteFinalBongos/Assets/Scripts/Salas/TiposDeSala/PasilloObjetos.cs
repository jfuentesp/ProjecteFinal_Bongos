using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasilloObjetos : TipoSala
{
    [SerializeField] List<LevelManager.ObjetosDisponibles> m_ObjetosDisponibles = new();
    protected override void SpawnerSala()
    {
        foreach (LevelManager.ObjetosDisponibles m_Objeto in m_ObjetosDisponibles)
        {
            //SpawnObjeto
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Init(LevelManager.Instance.GetObjetosSalaObjetos());
    }

    private void Init(List<LevelManager.ObjetosDisponibles> objetosDisponibles)
    {
        m_ObjetosDisponibles = objetosDisponibles;
    }
}
