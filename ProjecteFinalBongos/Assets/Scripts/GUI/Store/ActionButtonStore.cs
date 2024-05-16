using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButtonStore : MonoBehaviour, ISubmitHandler, ICancelHandler, IPointerClickHandler
{
    [SerializeField]
    private GameObject m_ActionButtons;
    [SerializeField]
    private GameObject m_ConfirmationButtons;

    private StoreGUIController m_StoreController;
    private enum ButtonActionsEnum { BUY, SELL, CONFIRM, CANCEL, QUANTITYUP, QUANTITYDOWN }
    [SerializeField]
    private ButtonActionsEnum m_ButtonActionsEnum;

    private Button m_Button;

    // Start is called before the first frame update
    void Start()
    {
        m_StoreController = LevelManager.Instance.StoreGUIController;
        if (m_ButtonActionsEnum == ButtonActionsEnum.CONFIRM)
            m_Button = GetComponent<Button>();     
    }

    private void Update()
    {
        if(m_ButtonActionsEnum == ButtonActionsEnum.CONFIRM)
            m_Button.interactable = m_StoreController.CheckIfPurchasable();
    }

    public void OnCancel(BaseEventData eventData)
    {
        m_ActionButtons.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_StoreController.LastSelected.TryGetComponent<ShopSlotBehaviour>(out ShopSlotBehaviour lastSelection);
        if (lastSelection != null)
            switch (m_ButtonActionsEnum)
            {
                case ButtonActionsEnum.BUY:
                    if (lastSelection.AssignedConsumable != null)
                    {
                        m_StoreController.SetBuying(true);
                        m_StoreController.SetSelling(false);
                        m_ActionButtons.SetActive(false);
                        m_ConfirmationButtons.SetActive(true);
                    }
                    if(lastSelection.AssignedEquipable != null)
                    {
                        m_StoreController.OnBuyEquipable(lastSelection.AssignedEquipable);
                    }
                    break;
                case ButtonActionsEnum.SELL:
                    if (lastSelection.AssignedConsumable != null)
                    {
                        m_StoreController.SetSelling(true);
                        m_StoreController.SetBuying(false);
                        m_ActionButtons.SetActive(false);
                        m_ConfirmationButtons.SetActive(true);
                    }
                    if(lastSelection.AssignedEquipable != null)
                    {
                        m_StoreController.OnSellEquipable(lastSelection.AssignedEquipable);
                    }
                    break;
                case ButtonActionsEnum.CONFIRM:
                    if (lastSelection.AssignedConsumable != null)
                    {
                        if (m_StoreController.IsBuying)
                        {
                            m_StoreController.OnBuyConsumable(lastSelection.AssignedConsumable);
                        }
                        if (m_StoreController.IsSelling)
                        {
                            m_StoreController.OnSellConsumable(lastSelection.AssignedConsumable);
                        }
                    }
                    if (lastSelection.AssignedEquipable != null)
                    {
                        m_ActionButtons.SetActive(false);
                        m_ConfirmationButtons.SetActive(false);
                    }
                    break;
                case ButtonActionsEnum.QUANTITYUP:
                    m_StoreController.OnIncreaseQuantity();
                    break;
                case ButtonActionsEnum.QUANTITYDOWN:
                    m_StoreController.OnDecreaseQuantity();
                    break;
                case ButtonActionsEnum.CANCEL:
                    m_ActionButtons.SetActive(false);
                    m_ConfirmationButtons.SetActive(false);
                    break;
            }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        m_StoreController.LastSelected.TryGetComponent<ShopSlotBehaviour>(out ShopSlotBehaviour lastSelection);
        if (lastSelection != null)
            switch (m_ButtonActionsEnum)
            {
                case ButtonActionsEnum.BUY:
                    if (lastSelection.AssignedConsumable != null)
                    {
                        m_StoreController.SetBuying(true);
                        m_StoreController.SetSelling(false);
                        m_ActionButtons.SetActive(false);
                        m_ConfirmationButtons.SetActive(true);
                    }
                    if (lastSelection.AssignedEquipable != null)
                    {
                        m_StoreController.OnBuyEquipable(lastSelection.AssignedEquipable);
                    }
                    break;
                case ButtonActionsEnum.SELL:
                    if (lastSelection.AssignedConsumable != null)
                    {
                        m_StoreController.SetSelling(true);
                        m_StoreController.SetBuying(false);
                        m_ActionButtons.SetActive(false);
                        m_ConfirmationButtons.SetActive(true);
                    }
                    if (lastSelection.AssignedEquipable != null)
                    {
                        m_StoreController.OnSellEquipable(lastSelection.AssignedEquipable);
                    }
                    break;
                case ButtonActionsEnum.CONFIRM:
                    if (lastSelection.AssignedConsumable != null)
                    {
                        if (m_StoreController.IsBuying)
                        {
                            m_StoreController.OnBuyConsumable(lastSelection.AssignedConsumable);
                        }
                        if (m_StoreController.IsSelling)
                        {
                            m_StoreController.OnSellConsumable(lastSelection.AssignedConsumable);
                        }
                    }
                    if (lastSelection.AssignedEquipable != null)
                    {
                        m_ActionButtons.SetActive(false);
                        m_ConfirmationButtons.SetActive(false);
                    }
                    break;
                case ButtonActionsEnum.QUANTITYUP:
                    m_StoreController.OnIncreaseQuantity();
                    break;
                case ButtonActionsEnum.QUANTITYDOWN:
                    m_StoreController.OnDecreaseQuantity();
                    break;
                case ButtonActionsEnum.CANCEL:
                    m_ActionButtons.SetActive(false);
                    m_ConfirmationButtons.SetActive(false);
                    break;
            }
    }


}
