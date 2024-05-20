using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryController : MonoBehaviour
{
    [Header("Inventory components")]
    [SerializeField]
    private GameObject m_InventoryHUD;
    [SerializeField]
    private GameObject m_InventorySlotPrefab;
    [SerializeField]
    private Backpack m_InventoryBackpack;
    public Backpack BackPack => m_InventoryBackpack;

    public Action OnEquipQuickConsumable;
    public Action OnUseQuickConsumable;
    public Action OnRemoveQuickConsumable;

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

    [Header("Equipment HUD settings")]
    [SerializeField]
    private GridLayoutGroup m_EquipmentGrid;
    [SerializeField]
    private GridSlotBehaviour m_Weapon;
    [SerializeField]
    private GridSlotBehaviour m_Armor;
    [SerializeField]
    private PlayerStatsController m_PlayerStats;

    [Header("QuickItems HUD settings")]
    [SerializeField]
    private GridLayoutGroup m_QuickConsumablesGrid;

    [Header("Description section settings")]
    [SerializeField]
    private Image m_DescriptionImage;
    [SerializeField]
    private TextMeshProUGUI m_DescriptionName;
    [SerializeField]
    private TextMeshProUGUI m_DescriptionText;

    private GameObject m_SelectedSlot;

    private GameObject m_MoveConsumableSlot;
    public GameObject MoveConsumableSlot => m_MoveConsumableSlot;

    private GameObject m_MoveEquipableSlot;
    public GameObject MoveEquipableSlot => m_MoveEquipableSlot;
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
    [SerializeField] private GridSlotBehaviour m_QuickItem1;
    [SerializeField] private GridSlotBehaviour m_QuickItem2;
    [SerializeField] private GridSlotBehaviour m_QuickItem3;

    private bool m_MovingConsumable;
    private bool m_MovingEquipable;
    [SerializeField] private GameObject m_ItemPrefab;

    private void Start()
    {
        m_MovingConsumable = false;
        m_MovingEquipable = false;
        m_LastSelection = m_InitialButton;
        PJSMB.Instance.Input.FindActionMap("PlayerActions").FindAction("OpenInventory").performed += OpenInventory;
        PJSMB.Instance.Input.FindActionMap("PlayerActions").FindAction("UseQuickItem").performed += UseQuickItem;
        PJSMB.Instance.Input.FindActionMap("PlayerActions").FindAction("UseQuickItem2").performed += UseQuickItem2;
        PJSMB.Instance.Input.FindActionMap("PlayerActions").FindAction("UseQuickItem3").performed += UseQuickItem3;
    }
    private void OpenInventory(InputAction.CallbackContext context)
    {
        RefreshInventoryGUI();
        m_InventoryHUD.SetActive(!m_InventoryHUD.activeSelf);
    }
    private void UseQuickItem(InputAction.CallbackContext context)
    {
        if(m_QuickItem1.AssignedConsumable != null)
            OnUse(m_QuickItem1.AssignedConsumable.id);
    }
    private void UseQuickItem2(InputAction.CallbackContext context)
    {
        if (m_QuickItem2.AssignedConsumable != null)
            OnUse(m_QuickItem2.AssignedConsumable.id);
    }
    private void UseQuickItem3(InputAction.CallbackContext context)
    {
        if (m_QuickItem3.AssignedConsumable != null)
            OnUse(m_QuickItem3.AssignedConsumable.id);
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
        OnUseQuickConsumable.Invoke();
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
        RefreshEquippedGearGUI();
    }

    public void OnEquipmentRemove()
    {
        GridSlotBehaviour slot = m_LastSelection.GetComponent<GridSlotBehaviour>();
        if (slot.AssignedEquipable != null)
        {
            m_InventoryBackpack.AddEquipable(slot.AssignedEquipable);
            slot.AssignedEquipable.OnRemove(transform.root.gameObject);
            slot.RefreshEquipment();
            RefreshInventoryGUI();
        }
    }

    public void OnQuickItemEquip(string itemID)
    {
        Consumable itemToEquip = m_InventoryBackpack.ConsumableSlots.FirstOrDefault(item => item?.Consumable.id == itemID).Consumable;
        if (itemToEquip != null)
        {
            bool exists = CheckIfQuickItemIsAlreadyEquipped(itemToEquip);
            if (exists)
                return;
            for (int index = 0; index < m_QuickConsumablesGrid.transform.childCount; index++)
            {
                GridSlotBehaviour slot = m_QuickConsumablesGrid.transform.GetChild(index).GetComponentInChildren<GridSlotBehaviour>();
                if (slot.AssignedConsumable == null)
                {        
                    slot.SetConsumable(itemToEquip);
                    slot.RefreshEquippedSlot();
                    OnEquipQuickConsumable.Invoke();
                    return;
                }
            }
        }
    }

    public void OnQuickItemRemove()
    {
        m_LastSelection.TryGetComponent<GridSlotBehaviour>(out GridSlotBehaviour slot);
        if (slot?.AssignedConsumable != null)
        {
            slot.RemoveConsumable();
            slot.RefreshEquippedSlot();
            RefreshInventoryGUI();
            OnRemoveQuickConsumable.Invoke();
        }
    }

    public bool CheckIfQuickItemIsAlreadyEquipped(Consumable consumable)
    {
        for (int index = 0; index < m_QuickConsumablesGrid.transform.childCount; index++)
        {
            GridSlotBehaviour slot = m_QuickConsumablesGrid.transform.GetChild(index).GetComponentInChildren<GridSlotBehaviour>();
            if (slot.AssignedConsumable == consumable)
                return true;
        }
        return false;
    }

    public void OnMoveConsumable(int indexSelected, int indexTarget)
    {
        m_InventoryBackpack.MoveConsumable(indexSelected, indexTarget);
        m_MoveConsumableSlot = null;
        ClearCanvasGroupBlockages();
        RefreshConsumableGUI();
    }

    public void OnMoveEquipable(int indexSelected, int indexTarget)
    {
        m_InventoryBackpack.MoveEquipable(indexSelected, indexTarget);
        m_MoveEquipableSlot = null;
        ClearCanvasGroupBlockages();
        RefreshEquipableGUI();
    }

    public void OnMoveConsumables(int indexSelected, int indexTarget)
    {
        m_InventoryBackpack.MoveConsumable(indexSelected, indexTarget);
        m_MoveConsumableSlot = null;
    }
    public void OnDropEquipable(string itemID) {
        Equipable itemToUse = m_InventoryBackpack.EquipableSlots.FirstOrDefault(item => item?.Equipable.id == itemID).Equipable;
        if (itemToUse != null)
        {
            GameObject equipable = Instantiate(m_ItemPrefab);
            equipable.GetComponent<ProximityItemBehaviour>().SetEquipable(itemToUse);
            equipable.GetComponent<ProximityItemBehaviour>().SetSprite(itemToUse.Sprite);
            equipable.transform.position = transform.parent.position;
            m_InventoryBackpack.RemoveEquipable(itemToUse);
        }
        RefreshEquipableGUI();
        RefreshEquippedGearGUI();
    }

    public void OnDropConsumable(string itemID)
    {
        Consumable itemToUse = m_InventoryBackpack.ConsumableSlots.FirstOrDefault(item => item?.Consumable.id == itemID).Consumable;
        if (itemToUse != null)
        {
            GameObject consumable = Instantiate(m_ItemPrefab);
            consumable.GetComponent<ProximityItemBehaviour>().SetConsumable(itemToUse);
            consumable.GetComponent<ProximityItemBehaviour>().SetSprite(itemToUse.Sprite);
            consumable.transform.position = transform.parent.position;
            m_InventoryBackpack.RemoveConsumable(itemToUse);
        }
        RefreshInventoryGUI();
    }
    /* GUI COMPONENTS */

    public void RefreshInventoryGUI()
    {
        RefreshConsumableGUI();
        RefreshEquipableGUI();
        RefreshDescriptionGUI();
        RefreshEquippedGearGUI();
    }

    private void RefreshConsumableGUI()
    {
        for(int items = 0; items < m_InventoryBackpack.ConsumableSlots.Length; items++)
        {
            GridSlotBehaviour slot = m_ConsumableGrid.transform.GetChild(items).GetComponentInChildren<GridSlotBehaviour>();
            if (m_InventoryBackpack.ConsumableSlots[items] != null)
            {
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
        //Debug.Log(slot.AssignedConsumable == null ? true : false);
        if (slot.AssignedConsumable != null)
        {
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

    public void RefreshEquippedGearGUI()
    {
        m_Weapon.SetEquipable(m_PlayerStats.Sword);
        m_Armor.SetEquipable(m_PlayerStats.Armor);
        m_Weapon.RefreshEquipment();
        m_Armor.RefreshEquipment();
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

    public void SetMoveConsumables(GameObject consumable)
    {
        m_MoveConsumableSlot = consumable;
        m_MovingConsumable = true;
        m_ConsumableCanvasGroup.interactable = true;
        m_ConsumableCanvasGroup.blocksRaycasts = true;
        m_EquipableCanvasGroup.interactable = false;
        m_EquipableCanvasGroup.blocksRaycasts = false;
        m_EquippedGearCanvasGroup.interactable = false;
        m_EquippedGearCanvasGroup.blocksRaycasts = false;
        m_EquippedConsumablesCanvasGroup.interactable = false;
        m_EquippedConsumablesCanvasGroup.blocksRaycasts = false;
    }

    public void SetMoveEquipables(GameObject equipable)
    {
        m_MoveEquipableSlot = equipable;
        m_MovingEquipable = true;
        m_EquipableCanvasGroup.interactable = true;
        m_EquipableCanvasGroup.blocksRaycasts = true;
        m_ConsumableCanvasGroup.interactable = false;
        m_ConsumableCanvasGroup.blocksRaycasts = false;
        m_EquippedGearCanvasGroup.interactable = false;
        m_EquippedGearCanvasGroup.blocksRaycasts = false;
        m_EquippedConsumablesCanvasGroup.interactable = false;
        m_EquippedConsumablesCanvasGroup.blocksRaycasts = false;
    }

    public void ClearCanvasGroupBlockages()
    {
        m_EquipableCanvasGroup.interactable = true;
        m_EquipableCanvasGroup.blocksRaycasts = true;
        m_ConsumableCanvasGroup.interactable = true;
        m_ConsumableCanvasGroup.blocksRaycasts = true;
        m_EquippedGearCanvasGroup.interactable = true;
        m_EquippedGearCanvasGroup.blocksRaycasts = true;
        m_EquippedConsumablesCanvasGroup.interactable = true;
        m_EquippedConsumablesCanvasGroup.blocksRaycasts = true;
    }

    public void SetMoveConsumableSlot(GameObject slot)
    {
        m_MoveConsumableSlot = slot;
    }

    public void SetMoveEquipableSlot(GameObject slot)
    {
        m_MoveEquipableSlot = slot;
    }

}
