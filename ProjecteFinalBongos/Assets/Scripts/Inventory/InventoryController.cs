using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_InventoryHUD;

    [SerializeField]
    private GameObject m_InventorySlotPrefab;

    [SerializeField]
    private Backpack m_InventoryBackpack;

    [Header("For testing")]
    [SerializeField]
    private Consumable m_ConsumableToAdd;
    [SerializeField]
    private Consumable m_ConsumableToAdd2;
    

    [Header("Consumable Grid settings")]
    [SerializeField]
    private GridLayoutGroup m_ConsumableGrid;
    [SerializeField]
    private int m_ConsumableGridColumns;
    [SerializeField]
    private int m_ConsumableGridRows;

    private GameObject m_SelectedSlot;

    private void Start()
    {
        //testing purposes
        m_InventoryBackpack.AddConsumable(m_ConsumableToAdd);
        m_InventoryBackpack.AddConsumable(m_ConsumableToAdd);
        m_InventoryBackpack.AddConsumable(m_ConsumableToAdd2);
        RefreshInventoryGUI();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            m_InventoryHUD.SetActive(!m_InventoryHUD.activeSelf);
        }
    }

    private void RefreshInventoryGUI()
    {
        RefreshConsumableGUI();
    }

    private bool CheckIfIsAlreadyAssignedInSlot(Consumable item)
    {
        for (int slots = 0; slots < m_ConsumableGrid.transform.childCount; slots++)
        {
            GridSlotBehaviour slot = m_ConsumableGrid.transform.GetChild(slots).GetComponentInChildren<GridSlotBehaviour>();
            if (slot.AssignedConsumable == item)
                return true;
        }
        return false;
    }

    private void RefreshConsumableGUI()
    {
        for(int items = 0; items < m_InventoryBackpack.ConsumableSlots.Length; items++)
        {
            GridSlotBehaviour slot = m_ConsumableGrid.transform.GetChild(items).GetComponentInChildren<GridSlotBehaviour>();
            if (m_InventoryBackpack.ConsumableSlots[items] != null)
            {
                Debug.Log("Seteando el objeto " + m_InventoryBackpack.ConsumableSlots[items].Consumable.itemName + " en el inventario.");
                slot.SetConsumable(m_InventoryBackpack.ConsumableSlots[items].Consumable);
            }
            else
            {
                slot.RemoveConsumable();
            }
            slot.RefreshSlot();
        }
    }

}
