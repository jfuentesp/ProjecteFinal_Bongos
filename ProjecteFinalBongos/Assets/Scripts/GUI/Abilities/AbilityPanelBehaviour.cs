using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPanelBehaviour : MonoBehaviour
{
    private AbilitiesGUIController m_AbilitiesController;

    private void Awake()
    {
        m_AbilitiesController = GetComponentInParent<AbilitiesGUIController>();
    }

    private void OnEnable()
    {
        LevelManager.Instance.EventSystem.SetSelectedGameObject(m_AbilitiesController.LastSelectedSlot);
    }
}
