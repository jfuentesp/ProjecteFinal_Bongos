using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilitiesGUIController : MonoBehaviour
{
    [Header("Ability GUI components")]
    [SerializeField]
    private GameObject m_AbilityHUD;
    [SerializeField]
    private PlayerStatsController m_PlayerStats;

    [Header("Listed Abilities")]
    [SerializeField]
    private List<Ability> m_Tier1Abilities;
    [SerializeField]
    private List<Ability> m_Tier2Abilities;
    [SerializeField]
    private List<Ability> m_Tier3Abilities;

    private AbilityTierEnum m_CurrentOffensiveTier = AbilityTierEnum.TIER1;
    private AbilityTierEnum m_CurrentDefensiveTier = AbilityTierEnum.TIER1;
    private AbilityTierEnum m_CurrentAgilityTier = AbilityTierEnum.TIER1;

    public AbilityTierEnum CurrentOffensiveTier => m_CurrentOffensiveTier;
    public AbilityTierEnum CurrentDefensiveTier => m_CurrentDefensiveTier;
    public AbilityTierEnum CurrentAgilityTier => m_CurrentAgilityTier;

    [Header("Description GUI")]
    [SerializeField]
    private Image m_DescriptionImage;
    [SerializeField]
    private TextMeshProUGUI m_DescriptionName;
    [SerializeField]
    private TextMeshProUGUI m_DescriptionText;
    [SerializeField]
    private TextMeshProUGUI m_AvailablePointsText;

    [Header("Text colors")]
    [SerializeField]
    private Color m_AvailablePointsColor;
    [SerializeField]
    private Color m_ZeroPointsColor;


    [Header("Event system settings")]
    [SerializeField]
    private GameObject m_InitialButton;
    private GameObject m_LastSelectedSlot;
    public GameObject LastSelectedSlot => m_LastSelectedSlot;

    [Header("Ability Slot List")]
    [SerializeField]
    private List<AbilitySlotBehaviour> m_Slots;

    private int m_AbilityPoints;
    public int AbilityPoints => m_AbilityPoints;

    [SerializeField]
    private PlayerAbilitiesController m_PlayerAbilities;

    // Start is called before the first frame update
    void Awake()
    {
        m_LastSelectedSlot = m_InitialButton;
        m_AbilityPoints = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            m_AbilityHUD.SetActive(!m_AbilityHUD.activeSelf);
            RefreshAbilityGUI();
        }
    }

    public Ability GetRandomAbilityByTierAndType(AbilityTierEnum tier, AbilityCategoryEnum category)
    {
        int random = 0;
        Ability ability = null;
        switch (tier)
        {
            case AbilityTierEnum.TIER1:
                random = Random.Range(0, m_Tier1Abilities.Count);
                ability = m_Tier1Abilities[random];
                if(ability.Category == category)
                {
                    m_Tier1Abilities.Remove(ability);
                    return ability;
                } 
                else
                {
                    return GetRandomAbilityByTierAndType(tier, category);
                }
            case AbilityTierEnum.TIER2:
                random = Random.Range(0, m_Tier1Abilities.Count);
                ability = m_Tier2Abilities[random];
                if (ability.Category == category)
                {
                    m_Tier2Abilities.Remove(ability);
                    return ability;
                }
                else
                {
                    return GetRandomAbilityByTierAndType(tier, category);
                }
            case AbilityTierEnum.TIER3:
                random = Random.Range(0, m_Tier1Abilities.Count);
                ability = m_Tier3Abilities[random];
                if (ability.Category == category)
                {
                    m_Tier3Abilities.Remove(ability);
                    return ability;
                }
                else
                {
                    return GetRandomAbilityByTierAndType(tier, category);
                }
        }
        return ability;
    }

    /* GUI */

    private void RefreshAbilityGUI()
    {
        OnGuiRefresh();
        RefreshDescriptionGUI();
    }

    private void OnGuiRefresh()
    {
        foreach(AbilitySlotBehaviour slot in m_Slots)
        {
            switch(slot.AssignedAbility.Category)
            {
                case AbilityCategoryEnum.OFFENSIVE:
                    slot.RefreshSlot(m_CurrentOffensiveTier);
                    break;
                case AbilityCategoryEnum.DEFENSIVE:
                    slot.RefreshSlot(m_CurrentDefensiveTier);
                    break;
                case AbilityCategoryEnum.AGILITY:
                    slot.RefreshSlot(m_CurrentAgilityTier);
                    break;
            }
        }
    }

    private void RefreshDescriptionGUI()
    {
        m_AvailablePointsText.text = m_AbilityPoints.ToString();

        if(m_AbilityPoints <= 0)
        {
            m_AvailablePointsText.color = m_ZeroPointsColor;
        } 
        else
        {
            m_AvailablePointsText.color = m_AvailablePointsColor;
        }

        if (m_LastSelectedSlot == null)
        {
            m_DescriptionImage.gameObject.SetActive(false);
            m_DescriptionName.gameObject.SetActive(false);
            m_DescriptionText.gameObject.SetActive(false);
            return;
        }

        AbilitySlotBehaviour slot = m_LastSelectedSlot.GetComponent<AbilitySlotBehaviour>();
        Debug.Log(slot.AssignedAbility == null ? true : false);
        if (slot.AssignedAbility != null)
        {
            m_DescriptionImage.gameObject.SetActive(true);
            m_DescriptionName.gameObject.SetActive(true);
            m_DescriptionText.gameObject.SetActive(true);
            m_DescriptionName.text = slot.AssignedAbility.abilityName;
            m_DescriptionText.text = slot.AssignedAbility.abilityDescription;
            m_DescriptionImage.sprite = slot.AssignedAbility.Sprite;
        }
    }

    public void SetSelection(GameObject go)
    {
        m_LastSelectedSlot = go;
        RefreshDescriptionGUI();
    }

    public void SetAbility(Ability ability)
    {
        m_AbilityPoints--;
        switch(ability.TypeEnum)
        {
            case AbilityTypeEnum.ABILITY:
                if (ability.Category == AbilityCategoryEnum.OFFENSIVE)
                    m_PlayerAbilities.learnAttack(ability.AbilityEnum);
                if (ability.Category == AbilityCategoryEnum.DEFENSIVE)
                    m_PlayerAbilities.learnParry(ability.AbilityEnum);
                if (ability.Category == AbilityCategoryEnum.AGILITY)
                    m_PlayerAbilities.learnMovement(ability.AbilityEnum);
                break;
            case AbilityTypeEnum.SPEEDUP:
                m_PlayerStats.IncreaseSpeed(ability.PowerUpAmount);
                break;
            case AbilityTypeEnum.HITPOINTSUP:
                m_PlayerStats.IncreaseHealth(ability.PowerUpAmount);
                break;
            case AbilityTypeEnum.ATTACKSPEEDUP:
                m_PlayerStats.IncreaseAttackSpeed(ability.PowerUpAmount);
                break;
            case AbilityTypeEnum.DEFENSEUP:
                m_PlayerStats.IncreaseDefense(ability.PowerUpAmount);
                break;
            case AbilityTypeEnum.DAMAGEUP:
                m_PlayerStats.IncreaseDamage(ability.PowerUpAmount);
                break;
        }

        switch(ability.Category)
        {
            case AbilityCategoryEnum.OFFENSIVE:
                m_CurrentOffensiveTier = ability.Tier == AbilityTierEnum.TIER1 ? AbilityTierEnum.TIER2 : AbilityTierEnum.TIER3;
                break;
            case AbilityCategoryEnum.DEFENSIVE:
                m_CurrentDefensiveTier = ability.Tier == AbilityTierEnum.TIER1 ? AbilityTierEnum.TIER2 : AbilityTierEnum.TIER3;
                break;
            case AbilityCategoryEnum.AGILITY:
                m_CurrentAgilityTier = ability.Tier == AbilityTierEnum.TIER1 ? AbilityTierEnum.TIER2 : AbilityTierEnum.TIER3;
                break;
        }
        RefreshAbilityGUI();
    }
}
