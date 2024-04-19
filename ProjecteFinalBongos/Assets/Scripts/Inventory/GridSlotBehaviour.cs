using System.Collections;
using System.Collections.Generic;
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

    private bool m_containsItem;

    private void Awake()
    {
        m_containsItem = false;
    }

    public void RefreshSlot(bool containsItem, Sprite sprite, int quantity)
    {
        m_Slot.SetActive(containsItem);
        if (!containsItem)
            return;
        m_ItemSprite.sprite = sprite;
        m_QuantityText.text = quantity.ToString();
    }
}
