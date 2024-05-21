using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquippedButtonBehaviour : MonoBehaviour, ISubmitHandler, ICancelHandler, IPointerClickHandler
{
    [SerializeField]
    private GameObject m_ActionButtons;

    private InventoryController m_InventoryController;
    private enum ButtonActionsEnum { REMOVE, CANCEL }
    [SerializeField]
    private ButtonActionsEnum m_ButtonActionsEnum;

    private void Start()
    {
        m_InventoryController = LevelManager.Instance.InventoryController;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        m_ActionButtons.SetActive(false);
        m_InventoryController.LastSelection.TryGetComponent<GridSlotBehaviour>(out GridSlotBehaviour lastSelection);
        if (lastSelection != null)
            switch (m_ButtonActionsEnum)
            {
                case ButtonActionsEnum.REMOVE:
                    if (lastSelection.AssignedConsumable != null)
                        m_InventoryController.OnQuickItemRemove();
                    if (lastSelection.AssignedEquipable != null)
                        m_InventoryController.OnEquipmentRemove();
                    break;
                case ButtonActionsEnum.CANCEL:
                    break;
            }
        m_ActionButtons.SetActive(false);
    }

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
                case ButtonActionsEnum.REMOVE:
                    if (lastSelection.AssignedConsumable != null)
                        m_InventoryController.OnQuickItemRemove();
                    if (lastSelection.AssignedEquipable != null)
                        m_InventoryController.OnEquipmentRemove();
                    break;
                case ButtonActionsEnum.CANCEL:
                    break;
            }
        m_ActionButtons.SetActive(false);
    }
}
