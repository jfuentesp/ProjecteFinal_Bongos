using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridSlotBehaviour : MonoBehaviour
{
    [SerializeField]
    private Image m_ItemSprite;
    [SerializeField]
    private GameObject m_Slot;
    [SerializeField]
    private TextMeshProUGUI m_QuantityText;
    [SerializeField]
    private Button m_SlotButton;
    [SerializeField]
    private InventoryController m_InventoryController;
    [SerializeField]
    private Backpack m_Backpack;

    private Consumable m_AssignedConsumable;
    public Consumable AssignedConsumable => m_AssignedConsumable;

    //private Equipable m_AssignedEquipable;
    //public Equipable AssignedEquipable;

    private void Awake()
    {
        if(m_SlotButton)
        {
            m_SlotButton.onClick.RemoveAllListeners();
            m_SlotButton.onClick.AddListener(() =>
            {
                m_InventoryController.SetSelectedItem(gameObject);
                if(m_AssignedConsumable != null)
                    m_InventoryController.OnUse(m_AssignedConsumable.id);
            });
        }
    }

    public void SetConsumable(Consumable consumableToSet)
    {
        m_AssignedConsumable = consumableToSet;
    }

    public void RemoveConsumable()
    {
        m_AssignedConsumable = null;
    }

    public void RefreshSlot()
    {
        if (m_AssignedConsumable == null)
        {
            m_Slot.SetActive(false);
        }
        else
        {
            m_Slot.SetActive(true);
            m_ItemSprite.sprite = m_AssignedConsumable.Sprite;
            m_QuantityText.text = m_Backpack.ConsumableSlots.FirstOrDefault(slot => slot?.Consumable == m_AssignedConsumable).Quantity.ToString();
        }
    }
}
