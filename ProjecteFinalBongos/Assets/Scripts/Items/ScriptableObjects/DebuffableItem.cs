using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Debuff Area Item", menuName = "Scriptables/Items/Debuff Area Item")]
public class DebuffableItem : Consumable
{
    [SerializeField]
    private LayerMask m_Layermask;
    [SerializeField]
    private float m_AreaRadius;
    [SerializeField]
    private float m_AmountToDebuff;
    [SerializeField]
    private float m_Duration;
    private enum StatType { DAMAGE, DEFENSE, SPEED }
    [SerializeField]
    private StatType m_StatType;

    public override void OnUse(GameObject usedBy)
    {
        RaycastHit2D[] enemiesHit = Physics2D.CircleCastAll(usedBy.transform.position, m_AreaRadius, usedBy.transform.position, m_AreaRadius, m_Layermask);
        for (int i = 0; i < enemiesHit.Length; i++)
        {
            enemiesHit[i].collider.TryGetComponent<IDebuffable>(out IDebuffable debuffable);
            if (debuffable != null)
            {
                switch (m_StatType) 
                {
                    case StatType.DAMAGE:
                        debuffable.AttackDebuff(m_AmountToDebuff, m_Duration);
                        break;
                    case StatType.DEFENSE:
                        debuffable.DefenseDebuff(m_AmountToDebuff, m_Duration);
                        break;
                    case StatType.SPEED:
                        debuffable.SpeedDebuff(m_AmountToDebuff, m_Duration);
                        break;
                }
            }
        }
    }
}
