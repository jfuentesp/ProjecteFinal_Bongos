using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtons : MonoBehaviour
{
    [Header("Canvas groups")]
    [SerializeField]
    private CanvasGroup m_InventoryCanvasGroup;
    [SerializeField]
    private CanvasGroup m_ActionButtonsCanvasGroup;

    private void OnEnable()
    {
        m_InventoryCanvasGroup.interactable = false;
        m_InventoryCanvasGroup.blocksRaycasts = false;
        m_ActionButtonsCanvasGroup.interactable = true;
        m_ActionButtonsCanvasGroup.blocksRaycasts = true;
    }

    private void OnDisable()
    {
        m_InventoryCanvasGroup.interactable = true;
        m_InventoryCanvasGroup.blocksRaycasts = true;
        m_ActionButtonsCanvasGroup.interactable = false;
        m_ActionButtonsCanvasGroup.blocksRaycasts = false;
    }
}
