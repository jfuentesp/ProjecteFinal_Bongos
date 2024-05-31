using SaveLoadGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static SaveLoadGame.SaveGame;

public class InventoryController : MonoBehaviour, ISaveableBackPackData
{
    [Header("Inventory components")]
    [SerializeField]
    private GameObject m_InventoryHUD;
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
    private bool m_IsStarted;
    [SerializeField] private GameObject m_ItemPrefab;

    private GameObject m_Player;

    private void Start()
    {
        m_PlayerStats = PJSMB.Instance.PlayerStatsController;
        m_Player = PJSMB.Instance.gameObject;
        m_MovingConsumable = false;
        m_MovingEquipable = false;
        m_LastSelection = m_InitialButton;

        PJSMB.Instance.Input.FindActionMap("PlayerActions").FindAction("UseQuickItem").performed += UseQuickItem;
        PJSMB.Instance.Input.FindActionMap("PlayerActions").FindAction("UseQuickItem2").performed += UseQuickItem2;
        PJSMB.Instance.Input.FindActionMap("PlayerActions").FindAction("UseQuickItem3").performed += UseQuickItem3;
        RefreshInventoryGUI();
    }

    private void UseQuickItem(InputAction.CallbackContext context)
    {
        if (m_QuickItem1.AssignedConsumable != null)
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
        Consumable itemToUse = m_InventoryBackpack.ConsumableSlots.FirstOrDefault(item => item?.Consumable.id == itemID)?.Consumable;

        if (itemToUse != null)
        {
            itemToUse.OnUse(m_Player);
            m_InventoryBackpack.RemoveConsumable(itemToUse);
        }
        RefreshInventoryGUI();
        OnUseQuickConsumable.Invoke();
    }

    public void OnEquip(string itemID)
    {
        Equipable itemToUse = m_InventoryBackpack.EquipableSlots.FirstOrDefault(item => item?.Equipable.id == itemID).Equipable;
        if (itemToUse == null)
            return;
        if (itemToUse != null)
        {
            itemToUse.OnEquip(m_Player);
            m_InventoryBackpack.RemoveEquipable(itemToUse);
        }
        RefreshEquipableGUI();
        RefreshEquippedGearGUI();
    }

    public void OnEquipmentReplace(Equipable equipment)
    {
        if (equipment is Armor)
        {
            if (PJSMB.Instance.PlayerStatsController.Armor != null)
                m_InventoryBackpack.AddEquipable(PJSMB.Instance.PlayerStatsController.Armor);
        }
        if (equipment is Sword)
        {
            if (PJSMB.Instance.PlayerStatsController.Sword != null)
                m_InventoryBackpack.AddEquipable(PJSMB.Instance.PlayerStatsController.Sword);
        }
    }

    public void OnEquipmentRemove()
    {
        GridSlotBehaviour slot = m_LastSelection.GetComponent<GridSlotBehaviour>();
        if (slot.AssignedEquipable != null)
        {
            m_InventoryBackpack.AddEquipable(slot.AssignedEquipable);
            slot.AssignedEquipable.OnRemove(m_Player);
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
    public void OnDropEquipable(string itemID)
    {
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
            consumable.transform.position = PJSMB.Instance.gameObject.transform.position;
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
        for (int items = 0; items < m_InventoryBackpack.ConsumableSlots.Length; items++)
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
        print(m_Weapon.AssignedEquipable);
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

    public BackPack Save()
    {
        SaveGame.BackPack m_BackPack = new SaveGame.BackPack();
        SaveGame.ConsumablesSLots[] consumableSlotId = new SaveGame.ConsumablesSLots[25];

        for (int i = 0; i < consumableSlotId.Length; i++)
        {
            //print(m_InventoryBackpack.ConsumableSlots[i].Consumable.id);
            if (m_InventoryBackpack.ConsumableSlots[i] != null)
            {
                consumableSlotId[i] = new SaveGame.ConsumablesSLots(m_InventoryBackpack.ConsumableSlots[i].Consumable.id, m_InventoryBackpack.ConsumableSlots[i].Quantity);

            }
            else
            {
                consumableSlotId[i] = new SaveGame.ConsumablesSLots("99", 0);
            }
        }

        string[] equipableSlotId = new string[25];
        for (int i = 0; i < equipableSlotId.Length; i++)
        {
            if (m_InventoryBackpack.EquipableSlots[i] != null)
            {
                equipableSlotId[i] = m_InventoryBackpack.EquipableSlots[i].Equipable.id;
            }
            else
            {
                equipableSlotId[i] = "99";
            }
        }

        m_BackPack.m_ConsumableSlotId = consumableSlotId;
        m_BackPack.m_EquipableSlotId = equipableSlotId;

        string[] m_QuickItemList = new string[3];

        if (m_QuickItem1.AssignedConsumable != null)
        {
            print("entro");
            m_QuickItemList[0] = m_QuickItem1.AssignedConsumable.id;
        }
        else { m_QuickItemList[0] = "99"; }
        if (m_QuickItem2.AssignedConsumable != null)
        {
            print("entro");
            m_QuickItemList[1] = m_QuickItem2.AssignedConsumable.id;
        }
        else { m_QuickItemList[1] = "99"; }
        if (m_QuickItem3.AssignedConsumable != null)
        {
            print("entro");
            m_QuickItemList[2] = m_QuickItem3.AssignedConsumable.id;
        }
        else { m_QuickItemList[2] = "99"; }

        print("QuickItems");
        foreach (string item in m_QuickItemList)
            print(item);
        m_BackPack.m_QuickCosnumableSlotsId = m_QuickItemList;

        return m_BackPack;
    }

    public void Load(BackPack _BackPack, bool EntreEscena)
    {
        if (!EntreEscena)
        {
            for (int i = 0; i < _BackPack.m_ConsumableSlotId.Length; i++)
            {
                if (_BackPack.m_ConsumableSlotId[i].id != "99")
                {
                    m_InventoryBackpack.AddConsumableLoadGame(LevelManager.Instance.ConsumableDataBase.GetItemByID(_BackPack.m_ConsumableSlotId[i].id), _BackPack.m_ConsumableSlotId[i].quantity, i);
                }
            }
            for (int i = 0; i < _BackPack.m_EquipableSlotId.Length; i++)
            {
                if (_BackPack.m_EquipableSlotId[i] != "99")
                {
                    m_InventoryBackpack.AddEquipableLoadGame(LevelManager.Instance.EquipableDataBase.GetItemByID(_BackPack.m_EquipableSlotId[i]), i);
                }
            }

            if (_BackPack.m_QuickCosnumableSlotsId[0] != "99")
            {
                //slot.SetConsumable(itemToEquip);
                //slot.RefreshEquippedSlot();

                m_QuickItem1.SetConsumable(LevelManager.Instance.ConsumableDataBase.GetItemByID(_BackPack.m_QuickCosnumableSlotsId[0]));
                print(m_QuickItem1.AssignedConsumable.itemName);
                m_QuickItem1.RefreshEquippedSlot();

            }
            if (_BackPack.m_QuickCosnumableSlotsId[1] != "99")
            {
                //m_Quickitem2.SetConsumable()
                m_QuickItem2.SetConsumable(LevelManager.Instance.ConsumableDataBase.GetItemByID(_BackPack.m_QuickCosnumableSlotsId[1]));
                print(m_QuickItem2.AssignedConsumable.itemName);
                m_QuickItem2.RefreshEquippedSlot();
            }
            if (_BackPack.m_QuickCosnumableSlotsId[2] != "99")
            {
                //m_Quickitem2.SetConsumable()
                m_QuickItem3.SetConsumable(LevelManager.Instance.ConsumableDataBase.GetItemByID(_BackPack.m_QuickCosnumableSlotsId[2]));
                print(m_QuickItem3.AssignedConsumable.itemName);
                m_QuickItem3.RefreshEquippedSlot();
            }
            OnEquipQuickConsumable?.Invoke();
        }
        else
        {
            if (_BackPack.m_QuickCosnumableSlotsId[0] != "99")
            {
                //slot.SetConsumable(itemToEquip);
                //slot.RefreshEquippedSlot();

                m_QuickItem1.SetConsumable(LevelManager.Instance.ConsumableDataBase.GetItemByID(_BackPack.m_QuickCosnumableSlotsId[0]));
                print(m_QuickItem1.AssignedConsumable.itemName);
                m_QuickItem1.RefreshEquippedSlot();

            }
            if (_BackPack.m_QuickCosnumableSlotsId[1] != "99")
            {
                //m_Quickitem2.SetConsumable()
                m_QuickItem2.SetConsumable(LevelManager.Instance.ConsumableDataBase.GetItemByID(_BackPack.m_QuickCosnumableSlotsId[1]));
                print(m_QuickItem2.AssignedConsumable.itemName);
                m_QuickItem2.RefreshEquippedSlot();
            }
            if (_BackPack.m_QuickCosnumableSlotsId[2] != "99")
            {
                //m_Quickitem2.SetConsumable()
                m_QuickItem3.SetConsumable(LevelManager.Instance.ConsumableDataBase.GetItemByID(_BackPack.m_QuickCosnumableSlotsId[2]));
                print(m_QuickItem3.AssignedConsumable.itemName);
                m_QuickItem3.RefreshEquippedSlot();
            }
            OnEquipQuickConsumable?.Invoke();
        }
        
    }
}
