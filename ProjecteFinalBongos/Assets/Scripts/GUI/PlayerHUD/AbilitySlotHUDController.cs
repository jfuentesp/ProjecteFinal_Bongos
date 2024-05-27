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
    private enum AbilitySlotEnum { PREVIOUS, CURRENT, NEXT}
    [SerializeField]
    private AbilitySlotEnum m_AbilitySlotEnum;

    // Start is called before the first frame update
    void Start()
    {
        PJSMB.Instance.PlayerAbilitiesController.OnLearnAbility += OnLearnAbilityAction;
        PJSMB.Instance.PlayerAbilitiesController.OnMovementAbilityCooldown += OnLearnAbilityAction;
        m_CooldownImage.fillAmount = 0;
        UpdateAssignedAbility();
        UpdateAbilitySlotGUI();
    }

    private void Update()
    {
        if (m_CurrentSlotAbility == null)
            return;

        if (m_CurrentSlotAbility.OnCooldown)
        {
            m_CurrentSlotAbility.CooldownRemaining -= Time.deltaTime;
            m_CooldownImage.fillAmount = m_CurrentSlotAbility.CooldownRemaining / m_CurrentSlotAbility.Cooldown;
            if (m_CurrentSlotAbility.CooldownRemaining <= 0)
            {
                m_CurrentSlotAbility.CooldownRemaining = 0;
                m_CooldownImage.gameObject.SetActive(false);
                m_CooldownImage.fillAmount = 0;
            }
        }
        else
        {
            m_CooldownImage.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        PJSMB.Instance.PlayerAbilitiesController.OnLearnAbility -= OnLearnAbilityAction;
        PJSMB.Instance.PlayerAbilitiesController.OnMovementAbilityCooldown -= OnLearnAbilityAction;
    }

    private void OnLearnAbilityAction()
    {
        UpdateAssignedAbility();
        CheckIfIsOnCooldown();
        UpdateAbilitySlotGUI();
    }

    private void UpdateAssignedAbility()
    {
        switch (m_AbilitySlotEnum)
        {
            case AbilitySlotEnum.CURRENT:
                m_CurrentSlotAbility = PJSMB.Instance.PlayerAbilitiesController.Movement;
                break;
            case AbilitySlotEnum.NEXT:
                m_CurrentSlotAbility = PJSMB.Instance.PlayerAbilitiesController.GetNextAbility();
                break;
            case AbilitySlotEnum.PREVIOUS:
                m_CurrentSlotAbility = PJSMB.Instance.PlayerAbilitiesController.GetPreviousAbility();
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
        if(m_CurrentSlotAbility.OnCooldown)
            m_CooldownImage.gameObject.SetActive(true);
    }

    private void CheckIfIsOnCooldown()
    {
        if (m_CurrentSlotAbility == null)
            return;

        if (m_CurrentSlotAbility.OnCooldown)
            AbilityCooldown();
    }

    private void AbilityCooldown()
    {
        if (m_CurrentSlotAbility.CooldownRemaining <= 0)
            m_CurrentSlotAbility.CooldownRemaining = m_CurrentSlotAbility.Cooldown;
    }
}
