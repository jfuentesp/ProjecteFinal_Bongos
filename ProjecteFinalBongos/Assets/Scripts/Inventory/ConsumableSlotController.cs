using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableSlotController : MonoBehaviour
{
    [SerializeField]
    private Consumable m_ConsumableItem;
    [SerializeField]
    private int m_Quantity;

    [SerializeField]
    private TextMeshProUGUI m_QuantityText;
    [SerializeField]
    private Image m_SpriteImage;

    private bool m_IsHighlighted;

}
