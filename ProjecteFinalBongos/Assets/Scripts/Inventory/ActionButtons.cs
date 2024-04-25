using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionButtons : MonoBehaviour
{
    [Header("Canvas groups")]
    [SerializeField]
    private CanvasGroup m_InventoryCanvasGroup;
    [SerializeField]
    private CanvasGroup m_ActionButtonsCanvasGroup;

    [SerializeField]
    private InventoryController m_InventoryController;

    private void OnEnable()
    {
        m_InventoryCanvasGroup.interactable = false;
        m_InventoryCanvasGroup.blocksRaycasts = false;
        m_ActionButtonsCanvasGroup.interactable = true;
        m_ActionButtonsCanvasGroup.blocksRaycasts = true;
        LevelManager.Instance.EventSystem.SetSelectedGameObject(transform.GetChild(0).GetChild(0).gameObject);
    }

    private void OnDisable()
    {
        m_InventoryCanvasGroup.interactable = true;
        m_InventoryCanvasGroup.blocksRaycasts = true;
        m_ActionButtonsCanvasGroup.interactable = false;
        m_ActionButtonsCanvasGroup.blocksRaycasts = false;
        LevelManager.Instance.EventSystem.SetSelectedGameObject(m_InventoryController.LastSelection);
    }
}
