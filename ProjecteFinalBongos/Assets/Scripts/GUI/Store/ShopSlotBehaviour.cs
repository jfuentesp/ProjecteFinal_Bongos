using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlotBehaviour : MonoBehaviour, ISelectHandler, ISubmitHandler, IDeselectHandler, ICancelHandler, IPointerClickHandler, IPointerEnterHandler
{
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
    private Backpack m_Backpack;

    private StoreGUIController m_StoreController;

    private Consumable m_AssignedConsumable;
    public Consumable AssignedConsumable => m_AssignedConsumable;

    private Equipable m_AssignedEquipable;
    public Equipable AssignedEquipable => m_AssignedEquipable;

    [Header("Action Menu")]
    [SerializeField]
    private GameObject m_ActionPanel;

    [Header("Slot type")]
    [SerializeField]
    private ShopSlotType m_SlotType;
    public ShopSlotType SlotType => m_SlotType;

    private void Start()
    {
        m_StoreController = LevelManager.Instance.GetComponent<StoreGUIController>();
    }

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
            m_ItemSprite.gameObject.SetActive(false);
            m_QuantityText.gameObject.SetActive(false);
            m_ItemSprite.sprite = null;
            m_QuantityText.text = string.Empty;
        }
        else
        {
            m_ItemSprite.gameObject.SetActive(true);
            m_QuantityText.gameObject.SetActive(true);
            m_ItemSprite.sprite = m_AssignedConsumable.Sprite;
            m_QuantityText.text = m_Backpack.ConsumableSlots.FirstOrDefault(slot => slot?.Consumable == m_AssignedConsumable).Quantity.ToString();
        }
    }

    public void RefreshStoreConsumableSlot()
    {
        if (m_AssignedConsumable == null)
        {
            m_ItemSprite.gameObject.SetActive(false);
            m_QuantityText.gameObject.SetActive(false);
            m_ItemSprite.sprite = null;
            m_QuantityText.text = string.Empty;
        }
        else
        {
            m_ItemSprite.gameObject.SetActive(true);
            m_QuantityText.gameObject.SetActive(true);
            m_ItemSprite.sprite = m_AssignedConsumable.Sprite;
        }
    }

    public void RefreshEquipableSlot()
    {
        if (m_AssignedEquipable == null)
        {
            m_ItemSprite.gameObject.SetActive(false);
            m_QuantityText.gameObject.SetActive(false);
            m_ItemSprite.sprite = null;
            m_QuantityText.text = string.Empty;
        }
        else
        {
            m_ItemSprite.gameObject.SetActive(true);
            m_QuantityText.gameObject.SetActive(true);
            m_ItemSprite.sprite = m_AssignedEquipable.Sprite;
            m_QuantityText.text = string.Empty;
        }
    }

    public void OnCancel(BaseEventData eventData)
    {
        m_ActionPanel.SetActive(false);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //m_StoreController.SetLastSelection(null);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_AssignedConsumable != null || m_AssignedEquipable != null)
        {
            m_ActionPanel.SetActive(true);
            m_ActionPanel.transform.position = transform.position;
            m_StoreController.SetLastSelection(gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_StoreController.SetLastSelection(gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        m_StoreController.SetLastSelection(gameObject);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (m_AssignedConsumable != null || m_AssignedEquipable != null)
        {
            m_ActionPanel.SetActive(true);
            m_ActionPanel.transform.position = transform.position;
            m_StoreController.SetLastSelection(gameObject);
        }
    }
}
