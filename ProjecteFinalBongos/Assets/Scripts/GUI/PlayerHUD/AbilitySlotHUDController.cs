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
        PJSMB.Instance.PlayerAbilitiesController.OnMovementAbilityCooldown += AbilityCooldown;
        m_CooldownImage.fillAmount = 0;
        UpdateAssignedAbility();
        UpdateAbilitySlotGUI();
    }

    private bool m_IsOnCooldown;
    private float m_ElapsedTime;
    private void Update()
    {
        if(m_CurrentSlotAbility != null) 
            if (m_CurrentSlotAbility.OnCooldown)
            {
                m_ElapsedTime -= Time.deltaTime;
                m_CooldownImage.fillAmount = m_ElapsedTime / m_Cooldown;
                if (m_ElapsedTime <= 0)
                {
                    m_IsOnCooldown = false;
                    m_CooldownImage.gameObject.SetActive(false);
                    m_CooldownImage.fillAmount = 0;
                }
            }
    }

    private void OnDestroy()
    {
        m_PlayerAbilities.OnLearnAbility -= OnLearnAbilityAction;
        PJSMB.Instance.PlayerAbilitiesController.OnMovementAbilityCooldown -= AbilityCooldown;
    }

    private void OnLearnAbilityAction()
    {
        UpdateAssignedAbility();
        UpdateAbilitySlotGUI();
    }

    private void UpdateAssignedAbility()
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

    private void AbilityCooldown(AbilityEnum ability, float timer)
    {
        if (m_CurrentSlotAbility != null)
            if (ability == m_CurrentSlotAbility.AbilityEnum)
            {
                m_CooldownImage.gameObject.SetActive(true);
                m_IsOnCooldown = true;
                m_Cooldown = m_CurrentSlotAbility.Cooldown;
                m_ElapsedTime = m_CurrentSlotAbility.Cooldown;
            }
    }

    public void SetRemainingCD(float remainingCD)
    {
        m_ElapsedTime = remainingCD;
    }
}
