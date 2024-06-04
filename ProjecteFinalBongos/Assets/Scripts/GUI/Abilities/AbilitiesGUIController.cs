using SaveLoadGame;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static SaveLoadGame.SaveGame;

public class AbilitiesGUIController : MonoBehaviour, ISaveableAbilitiesPlayerData
{
    [Header("Ability GUI components")]
    [SerializeField]
    private GameObject m_AbilityHUD;

    private PlayerStatsController m_PlayerStats;
    private PlayerAbilitiesController m_PlayerAbilities;

    private GameManager m_GameManager;



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
    public AbilitySlotBehaviour[] Slots => m_Slots.ToArray();

    private AbilityPointsController m_AbilityPoints;
    public AbilityPointsController AbilityPoints => m_AbilityPoints;

    // Start is called before the first frame update
    void Awake()
    {
        m_LastSelectedSlot = m_InitialButton;
    }

    private void Start()
    {
        m_GameManager = GameManager.Instance;
        m_PlayerStats = PJSMB.Instance.PlayerStatsController;
        m_PlayerAbilities = PJSMB.Instance.PlayerAbilitiesController;
        m_AbilityPoints = PJSMB.Instance.PlayerAbilityPoints;
        RefreshAbilityGUI();
    }

    private void Update()
    {
        RefreshDescriptionGUI();
    }

    public Ability GetRandomAbilityByTierAndType(AbilityTierEnum tier, AbilityCategoryEnum category)
    {
        int random = 0;
        Ability ability = null;
        switch (tier)
        {
            case AbilityTierEnum.TIER1:
                random = Random.Range(0, m_GameManager.Tier1Abilities.Count);
                ability = m_GameManager.Tier1Abilities[random];
                if (ability.Category == category)
                {
                    m_GameManager.Tier1Abilities.Remove(ability);
                    ability.IsLearnt = false;
                    return ability;
                }
                else
                {
                    return GetRandomAbilityByTierAndType(tier, category);
                }
            case AbilityTierEnum.TIER2:
                random = Random.Range(0, m_GameManager.Tier2Abilities.Count);
                ability = m_GameManager.Tier2Abilities[random];
                if (ability.Category == category)
                {
                    m_GameManager.Tier2Abilities.Remove(ability);
                    ability.IsLearnt = false;
                    return ability;
                }
                else
                {
                    return GetRandomAbilityByTierAndType(tier, category);
                }
            case AbilityTierEnum.TIER3:
                random = Random.Range(0, m_GameManager.Tier3Abilities.Count);
                ability = m_GameManager.Tier3Abilities[random];
                if (ability.Category == category)
                {
                    m_GameManager.Tier3Abilities.Remove(ability);
                    ability.IsLearnt = false;
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

    private void OnInitializeAbilities(AbilitySlotBehaviour slot)
    {
        if (slot.AssignedAbility != null)
            return;

        if (m_GameManager.NuevaPartida && LevelManager.Instance.MundoActualJugador == MundoEnum.MUNDO_UNO)
        {
            slot.Initialize();
        }
    }

    private void OnGuiRefresh()
    {
        foreach (AbilitySlotBehaviour slot in m_Slots)
        {
            OnInitializeAbilities(slot);
            switch (slot.AssignedAbility?.Category)
            {
                case AbilityCategoryEnum.OFFENSIVE:
                    slot.RefreshSlot(PJSMB.Instance.PlayerAbilitiesController.CurrentOffensiveTier);
                    break;
                case AbilityCategoryEnum.DEFENSIVE:
                    slot.RefreshSlot(PJSMB.Instance.PlayerAbilitiesController.CurrentDefensiveTier);
                    break;
                case AbilityCategoryEnum.AGILITY:
                    slot.RefreshSlot(PJSMB.Instance.PlayerAbilitiesController.CurrentAgilityTier);
                    break;
            }
        }
    }

    private void RefreshDescriptionGUI()
    {
        if (m_AbilityPoints == null)
            return;

        m_AvailablePointsText.text = m_AbilityPoints.AbilityPoints.ToString();

        if (m_AbilityPoints.AbilityPoints <= 0)
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
        m_AbilityPoints.ConsumeAbilityPoints(1);
        switch (ability.TypeEnum)
        {
            case AbilityTypeEnum.ABILITY:
                if (ability.Category == AbilityCategoryEnum.OFFENSIVE)
                    m_PlayerAbilities.learnAttack(ability);
                if (ability.Category == AbilityCategoryEnum.DEFENSIVE)
                    m_PlayerAbilities.learnParry(ability);
                if (ability.Category == AbilityCategoryEnum.AGILITY)
                    m_PlayerAbilities.learnMovement(ability);
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

        switch (ability.Category)
        {
            case AbilityCategoryEnum.OFFENSIVE:
                PJSMB.Instance.PlayerAbilitiesController.SetOffensiveTier(ability.Tier);
                break;
            case AbilityCategoryEnum.DEFENSIVE:
                PJSMB.Instance.PlayerAbilitiesController.SetDefensiveTier(ability.Tier);
                break;
            case AbilityCategoryEnum.AGILITY:
                PJSMB.Instance.PlayerAbilitiesController.SetAgilityTier(ability.Tier);
                break;
        }
        RefreshAbilityGUI();
    }

    public PlayerAbilities[] Save()
    {
        PlayerAbilities[] abilities = new PlayerAbilities[m_Slots.Count];
        for (int i = 0; i < m_Slots.Count; i++)
        {
            PlayerAbilities ability = new();
            ability.m_AbilityId = m_Slots[i].AssignedAbility.id;
            ability.m_AbilityIsLearned = m_Slots[i].AssignedAbility.IsLearnt;
            ability.m_ButtonId = m_Slots[i].ButtonId;
            abilities[i] = ability;
        }
        return abilities;
    }

    public void Load(PlayerAbilities[] _PlayerAbilities, bool entreEscena)
    {
        if (!entreEscena)
        {
            for (int i = 0; i < _PlayerAbilities.Length; i++)
            {
                m_Slots[i].InitAbilitiesGUI();
                m_Slots[i].SetAbility(LevelManager.Instance.AbilityDataBase.GetItemByID(_PlayerAbilities[i].m_AbilityId));
                if (_PlayerAbilities[i].m_AbilityIsLearned)
                {
                    switch (m_Slots[i].AssignedAbility.Category)
                    {
                        case AbilityCategoryEnum.OFFENSIVE:
                            PJSMB.Instance.PlayerAbilitiesController.learnAttack(m_Slots[i].AssignedAbility);
                            break;
                        case AbilityCategoryEnum.DEFENSIVE:
                            PJSMB.Instance.PlayerAbilitiesController.learnParry(m_Slots[i].AssignedAbility);
                            break;
                        case AbilityCategoryEnum.AGILITY:
                            PJSMB.Instance.PlayerAbilitiesController.learnMovement(m_Slots[i].AssignedAbility);
                            break;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < _PlayerAbilities.Length; i++)
            {
                m_Slots[i].InitAbilitiesGUI();
                m_Slots[i].SetAbility(LevelManager.Instance.AbilityDataBase.GetItemByID(_PlayerAbilities[i].m_AbilityId));
            }
        }
        m_LastSelectedSlot = m_InitialButton;
        RefreshAbilityGUI();
    }
}
