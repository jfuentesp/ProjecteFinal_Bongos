using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff Item", menuName = "Scriptables/Items/Buff Item")]
public class BuffableItem : Consumable
{
    [SerializeField]
    private float m_BuffStatAmount;
    [SerializeField]
    private float m_Duration;
    [SerializeField]
    private StatType m_StatType;

    public override void OnUse(GameObject usedBy)
    {
        usedBy.TryGetComponent<IBuffable>(out IBuffable buffable);
        if(buffable != null)
        {
            switch(m_StatType) 
            { 
                case StatType.ATTACK:
                    buffable.BuffStat(StatType.ATTACK, m_BuffStatAmount, m_Duration);
                    break;
                case StatType.DEFENSE:
                    buffable.BuffStat(StatType.DEFENSE, m_BuffStatAmount, m_Duration);
                    break;
                case StatType.SPEED:
                    buffable.BuffStat(StatType.SPEED, m_BuffStatAmount, m_Duration);
                    break;      
            }
        }
    }
}
