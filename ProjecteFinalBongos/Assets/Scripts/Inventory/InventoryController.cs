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
    [Header("Inventory components")]
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
    [SerializeField]
    private Equipable m_EquipableSword;
    [SerializeField]
    private Equipable m_EquipableArmor;
    

    [Header("Consumable Grid settings")]
    [SerializeField]
    private GridLayoutGroup m_ConsumableGrid;
    [SerializeField]
    private int m_ConsumableGridColumns;
    [SerializeField]
    private int m_ConsumableGridRows;

    [Header("Equipable Grid settings")]
    [SerializeField]
    private GridLayoutGroup m_EquipableGrid;
    [SerializeField]
    private int m_EquipableGridColumns;
    [SerializeField]
    private int m_EquipableGridRows;

    [Header("Description section settings")]
    [SerializeField]
    private Image m_DescriptionImage;
    [SerializeField]
    private TextMeshProUGUI m_DescriptionName;
    [SerializeField]
    private TextMeshProUGUI m_DescriptionText;

    private GameObject m_SelectedSlot;

    private GameObject m_MoveSelectedSlot;
    private GameObject m_MoveTargetSlot;

    [SerializeField]
    private GameObject m_InitialButton;
    private GameObject m_LastSelection;
    public GameObject LastSelection => m_LastSelection;

    [Header("Canvas groups")]
    [SerializeField]
    private CanvasGroup m_ConsumableCanvasGroup;
    [SerializeField]
    private CanvasGroup m_EquipableCanvasGroup;
    [SerializeField]
    private CanvasGroup m_EquippedGearCanvasGroup;
    [SerializeField]
    private CanvasGroup m_EquippedConsumablesCanvasGroup;

    private bool m_MovingConsumable;
    private bool m_MovingEquipable;


    private void Start()
    {
        m_MovingConsumable = false;
        m_MovingEquipable = false;
        m_LastSelection = m_InitialButton;
        m_InventoryBackpack.AddConsumable(m_ConsumableToAdd);
        m_InventoryBackpack.AddConsumable(m_ConsumableToAdd);
        m_InventoryBackpack.AddConsumable(m_ConsumableToAdd2);
        m_InventoryBackpack.AddEquipable(m_EquipableSword);
        m_InventoryBackpack.AddEquipable(m_EquipableArmor);
        RefreshInventoryGUI();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            m_InventoryHUD.SetActive(!m_InventoryHUD.activeSelf);
        }
    }

    public void OnUse(string itemID)
    {
        Consumable itemToUse = m_InventoryBackpack.ConsumableSlots.FirstOrDefault(item => item?.Consumable.id == itemID).Consumable;
        if (itemToUse != null)
        {
            itemToUse.OnUse(transform.root.gameObject);
            m_InventoryBackpack.RemoveConsumable(itemToUse);
        }
        RefreshInventoryGUI();
    }

    public void OnEquip(string itemID)
    {
        Equipable itemToUse = m_InventoryBackpack.EquipableSlots.FirstOrDefault(item => item?.Equipable.id == itemID).Equipable;
        if(itemToUse != null) 
        { 
            itemToUse.OnEquip(transform.root.gameObject);
            m_InventoryBackpack.RemoveEquipable(itemToUse);
        }
        RefreshEquipableGUI();
    }

    public void OnMoveConsumable(int indexSelected, int indexTarget)
    {
        m_InventoryBackpack.MoveConsumable(indexSelected, indexTarget);
    }

    public void OnMoveEquipable(int indexSelected, int indexTarget)
    {
        m_InventoryBackpack.MoveEquipable(indexSelected, indexTarget);
    }

    public void OnMoveConsumables(int indexSelected, int indexTarget)
    {
        
    }

    public void OnWithdraw()
    {

    }

    /* GUI COMPONENTS */

    private void RefreshInventoryGUI()
    {
        RefreshConsumableGUI();
        RefreshEquipableGUI();
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
            slot.RefreshConsumableSlot();
        }
    }

    private void RefreshEquipableGUI()
    {
        for (int items = 0; items < m_InventoryBackpack.EquipableSlots.Length; items++)
        {
            GridSlotBehaviour slot = m_EquipableGrid.transform.GetChild(items).GetComponentInChildren<GridSlotBehaviour>();
            if (m_InventoryBackpack.EquipableSlots[items] != null)
            {
                Debug.Log("Seteando el objeto " + m_InventoryBackpack.EquipableSlots[items].Equipable.itemName + " en el inventario.");
                slot.SetEquipable(m_InventoryBackpack.EquipableSlots[items].Equipable);
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
        if (slot.AssignedConsumable != null)
        {
            //Debug.Log("Paso por aquí.");
            m_DescriptionImage.gameObject.SetActive(true);
            m_DescriptionName.gameObject.SetActive(true);
            m_DescriptionText.gameObject.SetActive(true);
            m_DescriptionName.text = slot.AssignedConsumable.itemName;
            m_DescriptionText.text = slot.AssignedConsumable.description;
            m_DescriptionImage.sprite = slot.AssignedConsumable.Sprite;
        }

        if (slot.AssignedEquipable != null)
        {
            m_DescriptionImage.gameObject.SetActive(true);
            m_DescriptionName.gameObject.SetActive(true);
            m_DescriptionText.gameObject.SetActive(true);
            m_DescriptionName.text = slot.AssignedEquipable.itemName;
            m_DescriptionText.text = slot.AssignedEquipable.description;
            m_DescriptionImage.sprite = slot.AssignedEquipable.Sprite;
        }
    }

    /* SETTERS */

    public void SetSelectedItem(GameObject slot)
    {
        m_SelectedSlot = slot;
        RefreshDescriptionGUI();
    }

    public void SetLastSelection(GameObject slot)
    {
        m_LastSelection = slot;
    }

    public void SetMoveSelected(GameObject slot)
    {
        m_MoveSelectedSlot = slot;
        slot.TryGetComponent<GridSlotBehaviour>(out GridSlotBehaviour slotSelected);
        if(slotSelected != null)
        {
            Debug.Log("Entro aquí");
            if (slotSelected.AssignedConsumable != null)
            {
                m_MovingConsumable = true;
                SetMoveConsumables();
            }
            if (slotSelected.AssignedEquipable != null)
            {
                m_MovingEquipable = true;
                SetMoveEquipables();
            }
        }
    }

    public void SetMoveConsumables()
    {
        m_ConsumableCanvasGroup.interactable = true;
        m_ConsumableCanvasGroup.blocksRaycasts = true;
        m_EquipableCanvasGroup.interactable = false;
        m_EquipableCanvasGroup.blocksRaycasts = false;
        m_EquippedGearCanvasGroup.interactable = false;
        m_EquippedGearCanvasGroup.blocksRaycasts = false;
        m_EquippedConsumablesCanvasGroup.interactable = false;
        m_EquippedConsumablesCanvasGroup.blocksRaycasts = false;
    }

    public void SetMoveEquipables()
    {
        m_EquipableCanvasGroup.interactable = true;
        m_EquipableCanvasGroup.blocksRaycasts = true;
        m_ConsumableCanvasGroup.interactable = false;
        m_ConsumableCanvasGroup.blocksRaycasts = false;
        m_EquippedGearCanvasGroup.interactable = false;
        m_EquippedGearCanvasGroup.blocksRaycasts = false;
        m_EquippedConsumablesCanvasGroup.interactable = false;
        m_EquippedConsumablesCanvasGroup.blocksRaycasts = false;
    }
}
