using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlotHUDController : MonoBehaviour
{
    [SerializeField]
    private Ability m_CurrentSlotAbility;
    [SerializeField]
    private Image m_AbilityImage;
    [SerializeField]
    private Image m_CooldownImage;
    private float m_Cooldown;
    private enum AbilitySlotEnum { PREVIOUS, CURRENT, NEXT}
    [SerializeField]
    private AbilitySlotEnum m_AbilitySlotEnum;

    [Header("Player components")]
    private PlayerAbilitiesController m_PlayerAbilities;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerAbilities = PJSMB.Instance.PlayerAbilitiesController;
        m_PlayerAbilities.OnLearnAbility += OnLearnAbilityAction;
        UpdateAssignedAbility();
        UpdateAbilitySlotGUI();
    }

    private void OnDestroy()
    {
        m_PlayerAbilities.OnLearnAbility -= OnLearnAbilityAction;
    }

    private void OnLearnAbilityAction()
    {
        UpdateAssignedAbility();
        UpdateAbilitySlotGUI();
    }

    private void UpdateAssignedAbility()
    {
        try
        {
            switch (m_AbilitySlotEnum)
            {
                case AbilitySlotEnum.CURRENT:
                    m_CurrentSlotAbility = m_PlayerAbilities.Movement;
                    break;
                case AbilitySlotEnum.NEXT:
                    m_CurrentSlotAbility = m_PlayerAbilities.GetNextAbility();
                    break;
                case AbilitySlotEnum.PREVIOUS:
                    m_CurrentSlotAbility = m_PlayerAbilities.GetPreviousAbility();
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.Log("Ha petado el try catch: " + e);
        }
    }

    private void UpdateAbilitySlotGUI()
    {
        if (m_CurrentSlotAbility == null)
        {
            m_AbilityImage.gameObject.SetActive(false);
            m_CooldownImage.gameObject.SetActive(false);
            return;
        }
        m_AbilityImage.sprite = m_CurrentSlotAbility.Sprite;
        m_AbilityImage.gameObject.SetActive(true);
    }
}
