using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickItemGUIController : MonoBehaviour
{
    private InventoryController m_Inventory;

    [SerializeField]
    private Backpack m_PlayerBackpack;
    [SerializeField]
    private GridSlotBehaviour m_GridSlot;
    private Consumable m_AssignedConsumable;
    [SerializeField]
    private Image m_QuickItemImage;
    [SerializeField]
    private TextMeshProUGUI m_Quantity;

    private void Awake()
    {
       
    }

    private void Start()
    {
        m_Inventory = LevelManager.Instance.InventoryController;
        m_Inventory.OnEquipQuickConsumable += OnEquipConsumableAction;
        m_Inventory.OnUseQuickConsumable += UpdateQuickItem;
        m_Inventory.OnRemoveQuickConsumable += OnRemoveConsumableAction;
    }

    private void OnDestroy()
    {
        m_Inventory.OnEquipQuickConsumable -= OnEquipConsumableAction;
        m_Inventory.OnUseQuickConsumable -= UpdateQuickItem;
        m_Inventory.OnRemoveQuickConsumable -= OnRemoveConsumableAction;
    }

    public void OnSetQuickItem()
    {
        m_AssignedConsumable = m_GridSlot.AssignedConsumable;
    }

    public void OnRemoveQuickItem()
    {
        m_AssignedConsumable = null;
    }

    public void OnEquipConsumableAction()
    {
        OnSetQuickItem();
        UpdateQuickItem();
    }

    public void OnRemoveConsumableAction()
    {
        OnRemoveQuickItem();
        UpdateQuickItem();
    }

    public void UpdateQuickItem()
    {
        if(m_AssignedConsumable == null)
        {
            m_QuickItemImage.gameObject.SetActive(false);
            return;
        }
        m_QuickItemImage.sprite = m_AssignedConsumable.Sprite;
        int quantity = m_PlayerBackpack.GetQuantity(m_AssignedConsumable);
        if (quantity <= 0)
        {
            m_Quantity.text = "";
            m_QuickItemImage.color = Color.gray;
        } 
        else
        {
            m_Quantity.text = quantity.ToString();
            m_QuickItemImage.color = Color.white;
        }
        m_QuickItemImage.gameObject.SetActive(true);
    }
}
