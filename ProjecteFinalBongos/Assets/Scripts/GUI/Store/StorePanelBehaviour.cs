using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePanelBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ActionButtons;
    [SerializeField]
    private GameObject m_ConfirmationButtons;

    private StoreGUIController m_StoreController;

    private void Awake()
    {
        m_StoreController = LevelManager.Instance.StoreGUIController;
    }

    private void OnEnable()
    {
        LevelManager.Instance.EventSystem.SetSelectedGameObject(m_StoreController.LastSelected);
    }

    private void OnDisable()
    {
        m_ActionButtons.SetActive(false);
        m_ConfirmationButtons.SetActive(false);
    }
}
