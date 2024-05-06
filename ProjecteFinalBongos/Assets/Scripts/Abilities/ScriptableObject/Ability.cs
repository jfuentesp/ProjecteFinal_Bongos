using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Ability", menuName = "Scriptables/Ability/Player Ability")]
public class Ability : ScriptableObject
{
    [SerializeField]
    private string m_AbilityId;
    [SerializeField]
    private string m_AbilityName;
    [SerializeField]
    private string m_AbilityDescription;
    [SerializeField]
    private Sprite m_AbilitySprite;
    [SerializeField]
    private AbilityTierEnum m_AbilityTier;
    [SerializeField]
    private AbilityCategoryEnum m_AbilityCategory;
    [SerializeField]
    private AbilityCategoryEnum m_AbilityType;
    [SerializeField]
    private float m_PowerUpAmount;

    public string id { get => m_AbilityId; set => m_AbilityId = value; }
    public string abilityName { get => m_AbilityName; set => m_AbilityName = value; }
    public string abilityDescription { get => m_AbilityDescription; set => m_AbilityDescription = value; }
    public Sprite Sprite { get => m_AbilitySprite; set => m_AbilitySprite = value; }
    public AbilityTierEnum Tier { get => m_AbilityTier; set => m_AbilityTier = value; }
    public AbilityCategoryEnum Category { get => m_AbilityCategory; set => m_AbilityCategory = value; }
    public AbilityCategoryEnum TypeEnum { get => m_AbilityType; set => m_AbilityType = value; }
    public float PowerUpAmount { get => m_PowerUpAmount; set => m_PowerUpAmount = value; }
}
