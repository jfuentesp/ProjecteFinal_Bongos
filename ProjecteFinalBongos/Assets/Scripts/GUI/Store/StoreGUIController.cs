using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class StoreGUIController : MonoBehaviour
{
    [Header("Store GUI GameObject references")]
    [SerializeField]
    private GameObject m_GUIPanel;
    [Header("Store GUI Components")]
    [SerializeField]
    private GameObject m_StoreConsumableGrid;
    [SerializeField]
    private GameObject m_StoreEquipablesGrid;
    [SerializeField]
    private GameObject m_PlayerConsumableGrid;
    [SerializeField]
    private GameObject m_PlayerEquipableGrid;
    [SerializeField]
    private TextMeshProUGUI m_GoldQuantityText;

    private InventoryController m_PlayerInventory;

    [Header("Arrays that represent each of Piccolo stores")]
    [SerializeField]
    private Consumable[] m_PiccoloStoreConsumables = new Consumable[10];
    [SerializeField]
    private Equipable[] m_PiccoloStoreEquipables = new Equipable[10];

    private GameObject m_InitialButton;
    private GameObject m_LastSelected;
    public GameObject LastSelected => m_LastSelected;

    [Header("Description panel settings")]
    [SerializeField]
    private Image m_DescriptionImage;
    [SerializeField]
    private TextMeshProUGUI m_DescriptionName;
    [SerializeField]
    private TextMeshProUGUI m_DescriptionText;
    [SerializeField]
    private TextMeshProUGUI m_CostText;
    [SerializeField]
    private GameObject m_Cost;

    private bool m_IsBuying;
    public bool IsBuying => m_IsBuying;
    private bool m_IsSelling;
    public bool IsSelling => m_IsSelling;

    private GoldController m_PlayerGold;
    public GoldController PlayerGold => m_PlayerGold;

    [Header("Confirmation panel settings")]
    [SerializeField]
    private TextMeshProUGUI m_CurrentGoldText;
    [SerializeField]
    private TextMeshProUGUI m_CalculatedCostText;
    [SerializeField]
    private TMP_InputField m_QuantityStoreText;
    public TMP_InputField QuantityStoreText => m_QuantityStoreText;

    public event Action OnClosingStore;

    private GUIInGamePlayerScript m_GUIInGame;


    private void Start()
    {
        m_PlayerInventory = LevelManager.Instance.InventoryController;
        m_PlayerGold = PJSMB.Instance.PlayerGold;
        m_GUIInGame = GetComponent<GUIInGamePlayerScript>();
        m_InitialButton = m_StoreConsumableGrid.transform.GetChild(0).GetChild(0).gameObject;
        m_LastSelected = m_InitialButton;
        RefreshGUI();
    }

    public void OpenShop(List<Consumable> consumables, List<Equipable> equipables)
    {
        m_PiccoloStoreConsumables = consumables.ToArray();
        m_PiccoloStoreEquipables = equipables.ToArray();
        RefreshGUI();
        m_GUIInGame.ClosePanelsInsteadOf(TypeOfPanels.STORE);
    }

    public void CloseShop() 
    {
        m_GUIPanel.SetActive(false);
        OnClosingStore?.Invoke();
    }

    public void OnBuyConsumable(Consumable itemToBuy)
    {
        int quantity = m_PlayerInventory.BackPack.GetQuantity(itemToBuy);
        int quantityToBuy = int.Parse(m_QuantityStoreText.text);
        int quantityPrice = itemToBuy.shopPrice * quantityToBuy;
        if (m_PlayerGold.DINERO >= quantityPrice)
        {
            Debug.Log("Entro en el primer if!");
            m_PlayerGold.RemoveDinero(itemToBuy.shopPrice *  quantityToBuy);
            m_PlayerInventory.BackPack.AddConsumableStack(itemToBuy, quantityToBuy);
            RefreshGUI();
        }
    }

    public void OnBuyEquipable(Equipable itemToBuy)
    {
        if(m_PlayerGold.DINERO >= itemToBuy.shopPrice)
        {
            m_PlayerGold.RemoveDinero(itemToBuy.shopPrice);
            m_PlayerInventory.BackPack.AddEquipable(itemToBuy);
            RefreshGUI();
        }
    }

    public void OnSellConsumable(Consumable itemToSell)
    {
        int quantity = m_PlayerInventory.BackPack.GetQuantity(itemToSell);
        int quantityToSell = int.Parse(m_QuantityStoreText.text);
        int quantityPrice = itemToSell.sellPrice * quantityToSell;
        if(quantity >= quantityToSell)
        {
            m_PlayerGold.AddDinero(quantityPrice);
            m_PlayerInventory.BackPack.RemoveConsumableStack(itemToSell, quantityToSell);
            RefreshGUI();
        }
    }

    public void OnSellEquipable(Equipable itemToSell) 
    {
        m_PlayerGold.AddDinero(itemToSell.sellPrice);
        m_PlayerInventory.BackPack.RemoveEquipable(itemToSell);
        RefreshGUI();
    }

    public void OnIncreaseQuantity(Consumable consumable)
    {
        if (IsBuying)
        {
            int numberToSet = int.Parse(m_QuantityStoreText.text);
            if (numberToSet < (99-m_PlayerInventory.BackPack.GetQuantity(consumable)))
            {
                numberToSet += 1;
                m_QuantityStoreText.text = numberToSet.ToString();
                RefreshConfirmationGUI();
            }
        }
        if(IsSelling)
        {
            int numberToSet = int.Parse(m_QuantityStoreText.text);
            if (numberToSet < m_PlayerInventory.BackPack.GetQuantity(consumable))
            {
                numberToSet += 1;
                m_QuantityStoreText.text = numberToSet.ToString();
                RefreshConfirmationGUI();
            }
        }
    }

    public void OnDecreaseQuantity()
    {
        int numberToSet = int.Parse(m_QuantityStoreText.text);
        if(numberToSet > 1)
        {
            numberToSet -= 1;
            m_QuantityStoreText.text = numberToSet.ToString();
            RefreshConfirmationGUI();
        }
    }

    public bool CheckIfPurchasable()
    {
        if (int.Parse(m_CurrentGoldText.text) < int.Parse(m_CalculatedCostText.text))
            return false;
        else
            return true;
    }

    /* Setters */

    public void SetLastSelection(GameObject slot)
    {
        m_LastSelected = slot;
        RefreshDescriptionGUI();
    }

    public void SetBuying(bool state)
    {
        m_IsBuying = state;
    }

    public void SetSelling(bool state)
    {
        m_IsSelling = state;
    }

    /* GUI */
    public void RefreshGUI()
    {
        RefreshConsumablesStoreGUI();
        RefreshEquipablesStoreGUI();
        RefreshPlayerStoreConsumablesGUI();
        RefreshPlayerStoreEquipablesGUI();
        RefreshGoldGUI();
        RefreshConfirmationGUI();
    }

    public void RefreshConsumablesStoreGUI()
    {
        for (int items = 0; items < m_PiccoloStoreConsumables.Length; items++) 
        {
            ShopSlotBehaviour slot = m_StoreConsumableGrid.transform.GetChild(items).GetComponentInChildren<ShopSlotBehaviour>();
            if (m_PiccoloStoreConsumables[items] != null)
            {
                slot.SetConsumable(m_PiccoloStoreConsumables[items]);
            }
            else
            {
                slot.RemoveConsumable();
            }
            slot.RefreshStoreConsumableSlot();       
        }
    }

    public void RefreshEquipablesStoreGUI()
    {
        for (int items = 0; items < m_PiccoloStoreEquipables.Length; items++)
        {
            ShopSlotBehaviour slot = m_StoreEquipablesGrid.transform.GetChild(items).GetComponentInChildren<ShopSlotBehaviour>();
            if (m_PiccoloStoreEquipables[items] != null)
            {
                slot.SetEquipable(m_PiccoloStoreEquipables[items]);
            }
            else
            {
                slot.RemoveEquipable();
            }
            slot.RefreshEquipableSlot();
        }
    }

    public void RefreshPlayerStoreConsumablesGUI()
    {
        for (int items = 0; items < m_PlayerInventory.BackPack.ConsumableSlots.Length; items++)
        {
            ShopSlotBehaviour slot = m_PlayerConsumableGrid.transform.GetChild(items).GetComponentInChildren<ShopSlotBehaviour>();
            if (m_PlayerInventory.BackPack.ConsumableSlots[items] != null)
            {
                slot.SetConsumable(m_PlayerInventory.BackPack.ConsumableSlots[items].Consumable);
            }
            else
            {
                slot.RemoveConsumable();
            }
            slot.RefreshConsumableSlot();
        }
    }

    public void RefreshPlayerStoreEquipablesGUI()
    {
        for (int items = 0; items < m_PlayerInventory.BackPack.EquipableSlots.Length; items++)
        {
            ShopSlotBehaviour slot = m_PlayerEquipableGrid.transform.GetChild(items).GetComponentInChildren<ShopSlotBehaviour>();
            if (m_PlayerInventory.BackPack.EquipableSlots[items] != null)
            {
                slot.SetEquipable(m_PlayerInventory.BackPack.EquipableSlots[items].Equipable);
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
        if (m_LastSelected == null)
        {
            m_DescriptionImage.gameObject.SetActive(false);
            m_DescriptionName.gameObject.SetActive(false);
            m_DescriptionText.gameObject.SetActive(false);
            return;
        }

        ShopSlotBehaviour slot = m_LastSelected.GetComponent<ShopSlotBehaviour>();

        if (slot.AssignedConsumable != null)
        {
            m_DescriptionImage.gameObject.SetActive(true);
            m_DescriptionName.gameObject.SetActive(true);
            m_DescriptionText.gameObject.SetActive(true);
            m_Cost.gameObject.SetActive(true);
            m_DescriptionName.text = slot.AssignedConsumable.itemName;
            m_DescriptionText.text = slot.AssignedConsumable.description;
            m_DescriptionImage.sprite = slot.AssignedConsumable.Sprite;
            if(slot.SlotType == ShopSlotType.BUY)
                m_CostText.text = slot.AssignedConsumable.shopPrice.ToString();
            if(slot.SlotType == ShopSlotType.SELL)
                m_CostText.text = slot.AssignedConsumable.sellPrice.ToString();
        }

        if (slot.AssignedEquipable != null)
        {
            m_DescriptionImage.gameObject.SetActive(true);
            m_DescriptionName.gameObject.SetActive(true);
            m_DescriptionText.gameObject.SetActive(true);
            m_Cost.gameObject.SetActive(true);
            m_DescriptionName.text = slot.AssignedEquipable.itemName;
            m_DescriptionText.text = slot.AssignedEquipable.description;
            m_DescriptionImage.sprite = slot.AssignedEquipable.Sprite;
            if (slot.SlotType == ShopSlotType.BUY)
                m_CostText.text = slot.AssignedEquipable.shopPrice.ToString();
            if (slot.SlotType == ShopSlotType.SELL)
                m_CostText.text = slot.AssignedEquipable.sellPrice.ToString();
        }
    }

    public void RefreshGoldGUI()
    {
        m_GoldQuantityText.text = m_PlayerGold.DINERO.ToString();
    }

    public void RefreshConfirmationGUI()
    {
        m_LastSelected.TryGetComponent<ShopSlotBehaviour>(out ShopSlotBehaviour slot);
        m_CurrentGoldText.text = m_PlayerGold.DINERO.ToString();
        if (slot?.AssignedConsumable != null)
            if(slot.SlotType == ShopSlotType.BUY)
            {
                m_CalculatedCostText.text = (slot.AssignedConsumable?.shopPrice * int.Parse(m_QuantityStoreText.text)).ToString();
                if (int.Parse(m_CurrentGoldText.text) < int.Parse(m_CalculatedCostText.text))
                    m_CalculatedCostText.color = Color.red;
                else
                    m_CalculatedCostText.color = Color.black;
            }
            if (slot.SlotType == ShopSlotType.SELL)
            {
                m_CalculatedCostText.color = Color.black;
                m_CalculatedCostText.text = (slot.AssignedConsumable?.sellPrice * int.Parse(m_QuantityStoreText.text)).ToString();
            }
    }
}
