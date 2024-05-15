using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionButtonStore : MonoBehaviour, ISubmitHandler, ICancelHandler, IPointerClickHandler
{
    [SerializeField]
    private GameObject m_ActionButtons;
    [SerializeField]
    private GameObject m_ConfirmationButtons;

    private StoreGUIController m_StoreController;
    private enum ButtonActionsEnum { BUY, SELL, CONFIRM, CANCEL }
    [SerializeField]
    private ButtonActionsEnum m_ButtonActionsEnum;

    // Start is called before the first frame update
    void Start()
    {
        m_StoreController = LevelManager.Instance.StoreGUIController;
    }

    public void OnCancel(BaseEventData eventData)
    {
        m_ActionButtons.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_StoreController.LastSelectedConsumable.TryGetComponent<ShopSlotBehaviour>(out ShopSlotBehaviour lastSelection);
        if (lastSelection != null)
            switch (m_ButtonActionsEnum)
            {
                case ButtonActionsEnum.BUY:
                    if (lastSelection.AssignedConsumable != null || lastSelection.AssignedEquipable != null)
                    {
                        m_StoreController.SetBuying(true);
                        m_ConfirmationButtons.SetActive(true);
                    }
                    break;
                case ButtonActionsEnum.SELL:
                    if (lastSelection.AssignedConsumable != null || lastSelection.AssignedEquipable != null)
                    {
                        m_StoreController.SetSelling(true);
                        m_ConfirmationButtons.SetActive(true);
                    }
                    break;
                case ButtonActionsEnum.CONFIRM:
                    if (lastSelection.AssignedConsumable != null)
                    {
                        if (m_StoreController.IsBuying)
                        {
                            
                        }
                        if (m_StoreController.IsSelling)
                        {

                        }
                    }
                    if (lastSelection.AssignedEquipable != null)
                    {
                        if (m_StoreController.IsBuying)
                        {

                        }
                        if (m_StoreController.IsSelling)
                        {

                        }
                    }
                    break;
                case ButtonActionsEnum.CANCEL:
                    break;
            }
        m_ActionButtons.SetActive(false);
        m_ConfirmationButtons.SetActive(false);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        m_StoreController.LastSelectedConsumable.TryGetComponent<ShopSlotBehaviour>(out ShopSlotBehaviour lastSelection);
        if (lastSelection != null)
            switch (m_ButtonActionsEnum)
            {
                case ButtonActionsEnum.BUY:
                    if (lastSelection.AssignedConsumable != null || lastSelection.AssignedEquipable != null)
                    {
                        m_StoreController.SetBuying(true);
                        m_ConfirmationButtons.SetActive(true);
                    }
                    break;
                case ButtonActionsEnum.SELL:
                    if (lastSelection.AssignedConsumable != null || lastSelection.AssignedEquipable != null)
                    {
                        m_StoreController.SetSelling(true);
                        m_ConfirmationButtons.SetActive(true);
                    }
                    break;
                case ButtonActionsEnum.CONFIRM:
                    if (lastSelection.AssignedConsumable != null)
                    {
                        if (m_StoreController.IsBuying)
                        {

                        }
                        if (m_StoreController.IsSelling)
                        {

                        }
                    }
                    if (lastSelection.AssignedEquipable != null)
                    {
                        if (m_StoreController.IsBuying)
                        {

                        }
                        if (m_StoreController.IsSelling)
                        {

                        }
                    }
                    break;
                case ButtonActionsEnum.CANCEL:
                    break;
            }
        m_ActionButtons.SetActive(false);
        m_ConfirmationButtons.SetActive(false);
    }


}
