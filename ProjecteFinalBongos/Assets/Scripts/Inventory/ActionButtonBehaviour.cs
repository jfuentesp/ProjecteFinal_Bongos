using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionButtonBehaviour : MonoBehaviour, ISubmitHandler, ICancelHandler, IPointerClickHandler
{
    [SerializeField]
    private GameObject m_ActionButtons;
    [SerializeField]
    private InventoryController m_InventoryController;
    private enum ButtonActionsEnum { USE, EQUIP, MOVE, DROP, CANCEL}
    [SerializeField]
    private ButtonActionsEnum m_ButtonActionsEnum;

    public void OnCancel(BaseEventData eventData)
    {
        m_ActionButtons.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_ActionButtons.SetActive(false);
        m_InventoryController.LastSelection.TryGetComponent<GridSlotBehaviour>(out GridSlotBehaviour lastSelection);
        if (lastSelection != null)
            switch (m_ButtonActionsEnum)
            {
                case ButtonActionsEnum.USE:
                    if(lastSelection.AssignedConsumable != null)
                    {
                        Debug.Log("Usando objeto " + lastSelection.AssignedConsumable.itemName);
                        m_InventoryController.OnUse(lastSelection.AssignedConsumable.id);
                    }
                    if (lastSelection.AssignedEquipable != null)
                        Debug.Log("Este objeto es un equipable, no puedes usarlo.");
                    break;
                case ButtonActionsEnum.EQUIP:
                    if (lastSelection.AssignedConsumable != null)
                    {
                        Debug.Log("Equipando objeto " + lastSelection.AssignedConsumable.itemName);
                        m_InventoryController.OnQuickItemEquip(lastSelection.AssignedConsumable.id);
                    }
                    if (lastSelection.AssignedEquipable != null)
                    {
                        Debug.Log("Equipando objeto " + lastSelection.AssignedEquipable.itemName);
                        m_InventoryController.OnEquip(lastSelection.AssignedEquipable.id);
                    }
                    break;
                case ButtonActionsEnum.MOVE:
                    if (lastSelection.AssignedConsumable != null)
                        m_InventoryController.SetMoveConsumables(lastSelection.gameObject);
                    if (lastSelection.AssignedEquipable != null)
                        m_InventoryController.SetMoveEquipables(lastSelection.gameObject);
                    break;
                case ButtonActionsEnum.DROP:
                    if (lastSelection.AssignedConsumable != null)
                        Debug.Log("Soltado objeto " + lastSelection.AssignedConsumable.itemName);
                    if (lastSelection.AssignedEquipable != null)
                        Debug.Log("Soltado equipo " + lastSelection.AssignedEquipable.itemName);
                    break;
                case ButtonActionsEnum.CANCEL:
                    break;       
            }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        m_InventoryController.LastSelection.TryGetComponent<GridSlotBehaviour>(out GridSlotBehaviour lastSelection);
        if (lastSelection != null)
            switch (m_ButtonActionsEnum)
            {
                case ButtonActionsEnum.USE:
                    if (lastSelection.AssignedConsumable != null)
                    {
                        Debug.Log("Usando objeto " + lastSelection.AssignedConsumable.itemName);
                        m_InventoryController.OnUse(lastSelection.AssignedConsumable.id);
                    }
                    if (lastSelection.AssignedEquipable != null)
                        Debug.Log("Este objeto es un equipable, no puedes usarlo.");
                    break;
                case ButtonActionsEnum.EQUIP:
                    if (lastSelection.AssignedConsumable != null)
                    {
                        Debug.Log("Equipando objeto " + lastSelection.AssignedConsumable.itemName);
                        m_InventoryController.OnQuickItemEquip(lastSelection.AssignedConsumable.id);
                    }
                    if (lastSelection.AssignedEquipable != null)
                    {
                        Debug.Log("Equipando objeto " + lastSelection.AssignedEquipable.itemName);
                        m_InventoryController.OnEquip(lastSelection.AssignedEquipable.id);
                    }
                    break;
                case ButtonActionsEnum.MOVE:
                    if (lastSelection.AssignedConsumable != null)
                        m_InventoryController.SetMoveConsumables(lastSelection.gameObject);
                    if (lastSelection.AssignedEquipable != null)
                        m_InventoryController.SetMoveEquipables(lastSelection.gameObject);
                    break;
                case ButtonActionsEnum.DROP:
                    if (lastSelection.AssignedConsumable != null)
                        Debug.Log("Soltado objeto " + lastSelection.AssignedConsumable.itemName);
                    if (lastSelection.AssignedEquipable != null)
                        Debug.Log("Soltado equipo " + lastSelection.AssignedEquipable.itemName);
                    break;
                case ButtonActionsEnum.CANCEL:
                    break;
            }
        m_ActionButtons.SetActive(false);
    }
}
