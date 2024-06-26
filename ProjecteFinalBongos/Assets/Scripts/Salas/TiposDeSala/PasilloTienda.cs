using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static SaveLoadGame.SaveGame;

public class PasilloTienda : TipoSala, ISaveableTiendasData
{
    [SerializeField]
    private GameObject m_Vendedor;
    private int m_PiccoloId;
    public int PiccoloId => m_PiccoloId;
    [Header("Variables tienda")]
    [SerializeField] private List<Consumable> m_ObjetosDisponibles = new();
    [SerializeField] private List<Equipable> m_EquipablesDisponibles = new();
    public List<Consumable> ObjetosDisponibles => m_ObjetosDisponibles;
    public List<Equipable> EquipablesDisponibles => m_EquipablesDisponibles;

    protected override void SpawnerSala()
    {
        GameObject vendedor = Instantiate(m_Vendedor, transform);
        vendedor.transform.localPosition = Vector2.zero;
        vendedor.GetComponent<PiccoloChadScript>().Init();
    }

    public void Init()
    {
        m_PiccoloId = LevelManager.Instance.GiveIdToPiccoloChad();
        m_ObjetosDisponibles = LevelManager.Instance.GetObjetosTienda();
        m_EquipablesDisponibles = LevelManager.Instance.GetEquipoTienda();
        m_CanOpenDoor = true;
        SpawnerSala();
    }

    public PasilloTiendaData Save()
    {
        string[] idObjetos = new string[m_ObjetosDisponibles.Count];
        string[] idEquipables = new string[m_EquipablesDisponibles.Count];
        PasilloTiendaData PiccoloChad = new PasilloTiendaData();

        PiccoloChad.m_PiccoloId = m_PiccoloId;
        PiccoloChad.m_SalaTransform = transform.position;
        for (int i = 0; i < m_ObjetosDisponibles.Count; i++)
        {
            idObjetos[i] = m_ObjetosDisponibles[i].id;
        }
        for (int i = 0; i < m_EquipablesDisponibles.Count; i++)
        {
            idEquipables[i] = m_EquipablesDisponibles[i].id;
        }
        PiccoloChad.m_ObjetosId = idObjetos;
        PiccoloChad.m_EquipablesId = idEquipables;

        return PiccoloChad;
    }

    public void Load(PasilloTiendaData _pasilloTiendaData)
    {
        m_PiccoloId = _pasilloTiendaData.m_PiccoloId;
        foreach (string objetoId in _pasilloTiendaData.m_ObjetosId)
        {
            m_ObjetosDisponibles.Add(LevelManager.Instance.ConsumableDataBase.GetItemByID(objetoId));
        }
        foreach (string equipableId in _pasilloTiendaData.m_EquipablesId)
        {
            if (LevelManager.Instance.EquipableDataBase.GetItemByID(equipableId) != null)
            {
                m_EquipablesDisponibles.Add(LevelManager.Instance.EquipableDataBase.GetItemByID(equipableId));
            }
        }
        m_CanOpenDoor = true;
        SpawnerSala();
    }
}
