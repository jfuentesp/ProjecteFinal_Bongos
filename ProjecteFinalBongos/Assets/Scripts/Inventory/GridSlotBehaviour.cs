using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridSlotBehaviour : MonoBehaviour, ISelectHandler, ISubmitHandler, IDeselectHandler, ICancelHandler, IPointerClickHandler, IPointerEnterHandler
{
    //Igual habría que mirarse esto https://docs.unity3d.com/es/2018.4/Manual/SupportedEvents.html
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

    private Equipable m_AssignedEquipable;
    public Equipable AssignedEquipable => m_AssignedEquipable;

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

    public void SetEquipable(Equipable equipableToSet)
    {
        m_AssignedEquipable = equipableToSet;
    }

    public void RemoveEquipable()
    {
        m_AssignedEquipable = null;
    }

    public void RefreshConsumableSlot()
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

    public void RefreshEquipableSlot()
    {
        if (m_AssignedEquipable == null)
        {
            m_Slot.SetActive(false);
        }
        else
        {
            m_Slot.SetActive(true);
            m_ItemSprite.sprite = m_AssignedEquipable.Sprite;
            m_QuantityText.text = "";
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

    public void OnSubmit(BaseEventData eventData) 
    {
        if (m_InventoryController.MoveConsumableSlot != null)
        {
            int index1 = m_InventoryController.LastSelection.transform.parent.GetSiblingIndex();
            int index2 = gameObject.transform.parent.GetSiblingIndex();
            Debug.Log(string.Format("Cambiando el slot {0} por {1}", index1, index2));
            m_InventoryController.OnMoveConsumable(index1, index2);
            return;
        }

        if (m_InventoryController.MoveEquipableSlot != null)
        {
            int index1 = m_InventoryController.LastSelection.transform.parent.GetSiblingIndex();
            int index2 = gameObject.transform.parent.GetSiblingIndex();
            Debug.Log(string.Format("Cambiando el slot {0} por {1}", index1, index2));
            m_InventoryController.OnMoveEquipable(index1, index2);
            return;
        }

        if (m_AssignedConsumable != null || m_AssignedEquipable != null)
        {
            m_ActionPanel.SetActive(true);
            m_ActionPanel.transform.position = transform.position;
            m_InventoryController.SetLastSelection(gameObject);
        }
    }

    public void OnCancel(BaseEventData eventData) 
    {
        m_ActionPanel.SetActive(false);
        if (m_InventoryController.MoveConsumableSlot != null)
        {
            m_InventoryController.SetMoveConsumableSlot(null);
            m_InventoryController.ClearCanvasGroupBlockages();
        }
        if (m_InventoryController.MoveEquipableSlot != null)
        {
            m_InventoryController.SetMoveEquipableSlot(null);
            m_InventoryController.ClearCanvasGroupBlockages();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_InventoryController.MoveConsumableSlot != null)
        {
            int index1 = m_InventoryController.LastSelection.transform.parent.GetSiblingIndex();
            int index2 = gameObject.transform.parent.GetSiblingIndex();
            Debug.Log(string.Format("Cambiando el slot {0} por {1}", index1, index2));
            m_InventoryController.OnMoveConsumable(index1, index2);
            return;
        }

        if (m_InventoryController.MoveEquipableSlot != null)
        {
            int index1 = m_InventoryController.LastSelection.transform.parent.GetSiblingIndex();
            int index2 = gameObject.transform.parent.GetSiblingIndex();
            Debug.Log(string.Format("Cambiando el slot {0} por {1}", index1, index2));
            m_InventoryController.OnMoveEquipable(index1, index2);
            return;
        }

        if (m_AssignedConsumable != null || m_AssignedEquipable != null)
        {
            m_ActionPanel.SetActive(true);
            m_ActionPanel.transform.position = transform.position;
            m_InventoryController.SetLastSelection(gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_InventoryController.SetSelectedItem(gameObject);
    }
}
