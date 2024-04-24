using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridSlotBehaviour : MonoBehaviour, ISelectHandler, ISubmitHandler, IDeselectHandler, ICancelHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //Igual habr�a que mirarse esto https://docs.unity3d.com/es/2018.4/Manual/SupportedEvents.html
    [Header("Slot settings")]
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

    [Header("Action Menu")]
    [SerializeField]
    private GameObject m_ActionPanel;

    //private Equipable m_AssignedEquipable;
    //public Equipable AssignedEquipable;

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

    public void OnSelect(BaseEventData eventData)
    {
        m_InventoryController.SetSelectedItem(gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        m_InventoryController.SetSelectedItem(null);
        //m_ActionPanel.SetActive(false);
    }

    public void OnSubmit(BaseEventData eventData) 
    {
        if (m_AssignedConsumable != null)
        {
            //m_InventoryController.OnUse(m_AssignedConsumable.id);
            m_ActionPanel.SetActive(true);
            m_ActionPanel.transform.position = transform.position;
        }
    }

    public void OnCancel(BaseEventData eventData) 
    {
        m_ActionPanel.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_AssignedConsumable != null)
        {
            //m_InventoryController.OnUse(m_AssignedConsumable.id);
            m_ActionPanel.SetActive(true);
            m_ActionPanel.transform.position = transform.position;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_InventoryController.SetSelectedItem(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_InventoryController.SetSelectedItem(null);
    }
}
