using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Ability", menuName = "Scriptables/Ability/Player Ability")]
public class Ability : ScriptableObject
{
    [SerializeField]
    private string m_AbilityName;
    [SerializeField]
    private Sprite m_AbilitySprite;
    [SerializeField]
    private AbilityTierEnum m_AbilityTier;
    [SerializeField]
    private AbilityTypeEnum m_AbilityType;
}
