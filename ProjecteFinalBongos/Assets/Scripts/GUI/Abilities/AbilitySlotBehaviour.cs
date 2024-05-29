using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static SaveLoadGame.SaveGame;

public class AbilitySlotBehaviour : MonoBehaviour, ISelectHandler, ISubmitHandler, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField]
    private AbilityTierEnum m_SlotTier;
    [SerializeField]
    private AbilityCategoryEnum m_SlotCategory;

    private AbilitiesGUIController m_AbilitiesGUI;
    [SerializeField]
    private Image m_AbilityImage;
    [SerializeField]
    private Button m_Slot;
    [SerializeField]
    private Image m_SlotImage;
    [SerializeField]
    private string m_ButtonId;
    public string ButtonId => m_ButtonId;

    private Ability m_AssignedAbility;
    public Ability AssignedAbility => m_AssignedAbility;


    public void Initialize()
    {
        m_AbilitiesGUI = LevelManager.Instance.AbilitiesGUIController;
        m_AssignedAbility = m_AbilitiesGUI.GetRandomAbilityByTierAndType(m_SlotTier, m_SlotCategory);
        print("Existo?");
    }


    public void RefreshSlot(AbilityTierEnum tier)
    {
        if(m_AssignedAbility != null)
        {
            if(m_AssignedAbility.IsLearnt)
            {
                m_Slot.interactable = false;
                m_AbilityImage.color = Color.white;
                m_SlotImage.color = m_Slot.colors.pressedColor;
                return;
            }

            m_AbilityImage.gameObject.SetActive(true);
            m_AbilityImage.sprite = m_AssignedAbility.Sprite;

            if(m_SlotTier != tier)
            {
                m_Slot.interactable = false;
                m_AbilityImage.color = Color.gray;
            }
            else
            {
                m_Slot.interactable = true;
                m_AbilityImage.color = Color.white;
            }
        }
    }

    public void SetAbility(Ability _Ability)
    {
        m_AssignedAbility = _Ability;
    }

    private void SetChosen(bool status)
    {
        m_AssignedAbility.IsLearnt = status;
    }

    public void OnSelect(BaseEventData eventData)
    {
        m_AbilitiesGUI.SetSelection(gameObject);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (m_AbilitiesGUI.AbilityPoints.HabilityPoints <= 0)
            return;

        switch(m_AssignedAbility.Category)
        {
            case AbilityCategoryEnum.OFFENSIVE:
                if (m_SlotTier != PJSMB.Instance.PlayerAbilitiesController.CurrentOffensiveTier)
                    return;
                break;
            case AbilityCategoryEnum.DEFENSIVE:
                if (m_SlotTier != PJSMB.Instance.PlayerAbilitiesController.CurrentDefensiveTier)
                    return;
                break;
            case AbilityCategoryEnum.AGILITY:
                if (m_SlotTier != PJSMB.Instance.PlayerAbilitiesController.CurrentAgilityTier)
                    return;
                break;
        }

        m_AssignedAbility.IsLearnt = true;
        m_AbilitiesGUI.SetAbility(AssignedAbility);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_AbilitiesGUI.AbilityPoints.HabilityPoints <= 0)
            return;

        switch (m_AssignedAbility.Category)
        {
            case AbilityCategoryEnum.OFFENSIVE:
                if (m_SlotTier != PJSMB.Instance.PlayerAbilitiesController.CurrentOffensiveTier)
                    return;
                break;
            case AbilityCategoryEnum.DEFENSIVE:
                if (m_SlotTier != PJSMB.Instance.PlayerAbilitiesController.CurrentDefensiveTier)
                    return;
                break;
            case AbilityCategoryEnum.AGILITY:
                if (m_SlotTier != PJSMB.Instance.PlayerAbilitiesController.CurrentAgilityTier)
                    return;
                break;
        }

        m_AssignedAbility.IsLearnt = true;
        m_AbilitiesGUI.SetAbility(AssignedAbility);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_AbilitiesGUI.SetSelection(gameObject);
    }

}
