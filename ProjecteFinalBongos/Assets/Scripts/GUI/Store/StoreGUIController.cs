using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StoreGUIController : MonoBehaviour
{
    [Header("Store GUI GameObject references")]
    [SerializeField]
    private GameObject m_GUIPanel;
    [Header("Store GUI Components")]
    [SerializeField]
    private InventoryController m_PlayerInventory;
    [SerializeField]
    private GameObject m_StoreConsumableGrid;
    [SerializeField]
    private GameObject m_StoreEquipablesGrid;
    [SerializeField]
    private GameObject m_PlayerConsumableGrid;
    [SerializeField]
    private GameObject m_PlayerEquipableGrid;

    [Header("Arrays that represent each of Piccolo stores")]
    [SerializeField]
    private Consumable[] m_PiccoloStoreConsumables = new Consumable[10];
    [SerializeField]
    private Equipable[] m_PiccoloStoreEquipables = new Equipable[10];

    private GameObject m_LastSelectedConsumable;

    private void Start()
    {
        m_PlayerInventory = GameManager.Instance.PlayerInGame.transform.GetChild(2).GetComponent<InventoryController>();
    }

    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void LoadStore()
    {

    }

    private void LoadPlayerInventory()
    {
        
    }

    public void OpenShop(List<Consumable> consumables, List<Equipable> equipables)
    {
        m_PiccoloStoreConsumables = consumables.ToArray();
        m_PiccoloStoreEquipables = equipables.ToArray();
        LoadStore();
        LoadPlayerInventory();
        RefreshGUI();
        m_GUIPanel.SetActive(true);
    }

    public void CloseShop() 
    {
        m_GUIPanel.SetActive(false);
    }

    /* Setters */

    public void SetSelectedItem(GameObject slot)
    {
        m_LastSelectedConsumable = slot;
        RefreshDescriptionGUI();
    }

    public void SetLastSelection(GameObject slot)
    {
        m_LastSelectedConsumable = slot;
    }

    /* GUI */
    public void RefreshGUI()
    {
        RefreshConsumablesStoreGUI();
        RefreshEquipablesStoreGUI();
        RefreshPlayerStoreConsumablesGUI();
        RefreshPlayerStoreEquipablesGUI();
    }

    public void RefreshConsumablesStoreGUI()
    {
        for (int items = 0; items < m_StoreConsumableGrid.transform.childCount; items++) 
        {
            GridSlotBehaviour slot = m_StoreConsumableGrid.transform.GetChild(items).GetComponentInChildren<GridSlotBehaviour>();
            //if()
        }
    }

    public void RefreshEquipablesStoreGUI()
    {

    }

    public void RefreshPlayerStoreConsumablesGUI()
    {
        for (int items = 0; items < m_PlayerInventory.BackPack.ConsumableSlots.Length; items++)
        {
            GridSlotBehaviour slot = m_PlayerConsumableGrid.transform.GetChild(items).GetComponentInChildren<GridSlotBehaviour>();
            if (m_PlayerInventory.BackPack.ConsumableSlots[items] != null)
            {
                slot.SetConsumable(m_PlayerInventory.BackPack.ConsumableSlots[items].Consumable);
            }
            else
            {
                slot.RemoveConsumable();
            }
            slot.RefreshConsumableSlot();
        }
    }

    public void RefreshPlayerStoreEquipablesGUI()
    {
        for (int items = 0; items < m_PlayerInventory.BackPack.EquipableSlots.Length; items++)
        {
            GridSlotBehaviour slot = m_PlayerEquipableGrid.transform.GetChild(items).GetComponentInChildren<GridSlotBehaviour>();
            if (m_PlayerInventory.BackPack.EquipableSlots[items] != null)
            {
                slot.SetEquipable(m_PlayerInventory.BackPack.EquipableSlots[items].Equipable);
            }
            else
            {
                slot.RemoveEquipable();
            }
            slot.RefreshEquipableSlot();
        }
    }

    public void RefreshDescriptionGUI()
    {

    }
}
