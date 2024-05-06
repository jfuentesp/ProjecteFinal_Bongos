using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickItemBehaviour : MonoBehaviour, ISelectHandler, ISubmitHandler, IDeselectHandler, ICancelHandler, IPointerClickHandler, IPointerEnterHandler
{    
    private Consumable m_AssignedConsumable;
    public Consumable AssignedConsumable => m_AssignedConsumable;

    [Header("Slot settings")]
    [SerializeField]
    private Image m_ItemSprite;
    [SerializeField]
    private GameObject m_Slot;
    [SerializeField]
    private Button m_SlotButton;
    [SerializeField]
    private InventoryController m_InventoryController;
    [SerializeField]
    private Backpack m_Backpack;

    [Header("Action Menu")]
    [SerializeField]
    private GameObject m_ActionPanel;

    public void SetConsumable(Consumable consumableToSet)
    {
        m_AssignedConsumable = consumableToSet;
    }

    public void RemoveConsumable()
    {
        m_AssignedConsumable = null;
    }

    public void OnCancel(BaseEventData eventData)
    {
        m_ActionPanel.SetActive(false);
        if (m_InventoryController.MoveConsumableSlot != null)
        {
            m_InventoryController.SetMoveConsumableSlot(null);
            m_InventoryController.ClearCanvasGroupBlockages();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(m_AssignedConsumable != null)
        {
            m_ActionPanel.SetActive(true);
            m_ActionPanel.transform.position = transform.position;
            m_InventoryController.SetLastSelection(gameObject);
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (m_AssignedConsumable != null)
        {
            m_ActionPanel.SetActive(true);
            m_ActionPanel.transform.position = transform.position;
            m_InventoryController.SetLastSelection(gameObject);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        m_InventoryController.SetSelectedItem(gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        m_InventoryController.SetSelectedItem(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_InventoryController.SetSelectedItem(gameObject);
    }

    public void RefreshSlot()
    {
        if (m_AssignedConsumable == null)
        {
            m_ItemSprite.gameObject.SetActive(false);
        }
        else
        {
            m_ItemSprite.gameObject.SetActive(true);
            m_ItemSprite.sprite = m_AssignedConsumable.Sprite;
        }
    }
}
