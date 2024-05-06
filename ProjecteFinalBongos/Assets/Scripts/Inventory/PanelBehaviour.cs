using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ActionButtons;

    private InventoryController m_InventoryController;

    private void Awake()
    {
        m_InventoryController = GetComponentInParent<InventoryController>();
    }

    private void OnEnable()
    {
        LevelManager.Instance.EventSystem.SetSelectedGameObject(m_InventoryController.LastSelection);
    }

    private void OnDisable()
    {
        m_ActionButtons.SetActive(false);   
    }
}
