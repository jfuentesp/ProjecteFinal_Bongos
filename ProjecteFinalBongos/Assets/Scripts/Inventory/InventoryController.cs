using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
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

    [Header("Description section settings")]
    [SerializeField]
    private Image m_DescriptionImage;
    [SerializeField]
    private TextMeshProUGUI m_DescriptionName;
    [SerializeField]
    private TextMeshProUGUI m_DescriptionText;

    private GameObject m_SelectedSlot;
    private GameObject m_TargetSlot;

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
        RefreshDescriptionGUI();
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

    public void OnUse(string itemID)
    {
        Consumable itemToUse = m_InventoryBackpack.ConsumableSlots.FirstOrDefault(item => item?.Consumable.id == itemID).Consumable;
        if(itemToUse != null) 
        {
            itemToUse.OnUse(transform.root.gameObject);
            m_InventoryBackpack.RemoveConsumable(itemToUse);
        }
        RefreshInventoryGUI();
    }

    public void SetSelectedItem(GameObject slot)
    {
        m_SelectedSlot = slot;
        RefreshDescriptionGUI();
    }

    public void RefreshDescriptionGUI()
    {
        if (m_SelectedSlot == null)
        {
            m_DescriptionImage.gameObject.SetActive(false);
            m_DescriptionName.gameObject.SetActive(false);
            m_DescriptionText.gameObject.SetActive(false);
            return;
        }

        GridSlotBehaviour slot = m_SelectedSlot.GetComponent<GridSlotBehaviour>();
        Debug.Log(slot.AssignedConsumable == null ? true : false);
        //Debug.Log("Paso por aquí." + slot.AssignedConsumable.itemName);
        if(slot.AssignedConsumable != null)
        {
            //Debug.Log("Paso por aquí.");
            m_DescriptionImage.gameObject.SetActive(true);
            m_DescriptionName.gameObject.SetActive(true);
            m_DescriptionText.gameObject.SetActive(true);
            m_DescriptionName.text = slot.AssignedConsumable.itemName;
            m_DescriptionText.text = slot.AssignedConsumable.description;
            m_DescriptionImage.sprite = slot.AssignedConsumable.Sprite;
        }
    }
}
