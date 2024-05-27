using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SaveLoadGame.SaveGame;

public class PasilloObjetos : TipoSala, ISaveableObjetosData
{
    [SerializeField] List<Consumable> m_ObjetosDisponibles = new();
    [SerializeField] private GameObject m_CofrePrefab;
    [SerializeField] private bool m_Horizontal;
    protected override void SpawnerSala()
    {
        foreach (Consumable m_Objeto in m_ObjetosDisponibles)
        {
            GameObject Barril = Instantiate(m_CofrePrefab, transform);
            Barril.GetComponent<BarrelController>().SetConsumable(m_Objeto);
            Barril.transform.position = GetPosicionSPawnear(m_Horizontal);
        }
    }

    public void Init(List<Consumable> objetosDisponibles, bool horizontal)
    {
        m_Horizontal = horizontal;
        m_ObjetosDisponibles = objetosDisponibles;
        SpawnerSala();
    }

    PasilloObjetosData ISaveableObjetosData.Save()
    {
        string[] idObjetos = new string[m_ObjetosDisponibles.Count];
        PasilloObjetosData PiccoloChad = new PasilloObjetosData();

        PiccoloChad.m_SalaTransform = transform.position;
        for (int i = 0; i < m_ObjetosDisponibles.Count; i++)
        {
            idObjetos[i] = m_ObjetosDisponibles[i].id;
        }
        PiccoloChad.m_ObjetosId = idObjetos;

        return PiccoloChad;
    }

    public void Load(PasilloObjetosData _pasilloTiendaData)
    {
        foreach (string objetoId in _pasilloTiendaData.m_ObjetosId)
        {
            m_ObjetosDisponibles.Add(LevelManager.Instance.ConsumableDataBase.GetItemByID(objetoId));
        }
        m_CanOpenDoor = true;
        SpawnerSala();
    }
}
