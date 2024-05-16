using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtonsStoreCanvas : MonoBehaviour
{
    [Header("Canvas groups")]
    [SerializeField]
    private CanvasGroup m_StoreCanvasGroup;
    [SerializeField]
    private CanvasGroup m_ActionButtonsCanvasGroup;

    private void OnEnable()
    {
        m_StoreCanvasGroup.interactable = false;
        m_StoreCanvasGroup.blocksRaycasts = false;
        m_ActionButtonsCanvasGroup.interactable = true;
        m_ActionButtonsCanvasGroup.blocksRaycasts = true;
        LevelManager.Instance.StoreGUIController.QuantityStoreText.text = "1";
        LevelManager.Instance.EventSystem.SetSelectedGameObject(transform.GetChild(0).gameObject);
    }

    private void OnDisable()
    {
        m_StoreCanvasGroup.interactable = true;
        m_StoreCanvasGroup.blocksRaycasts = true;
        m_ActionButtonsCanvasGroup.interactable = false;
        m_ActionButtonsCanvasGroup.blocksRaycasts = false;
        LevelManager.Instance.EventSystem.SetSelectedGameObject(LevelManager.Instance.StoreGUIController.LastSelected);
    }
}
