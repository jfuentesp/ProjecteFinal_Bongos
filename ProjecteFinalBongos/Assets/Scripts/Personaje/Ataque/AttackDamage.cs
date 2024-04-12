using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamage : MonoBehaviour
{
    [SerializeField]
    private float m_Damage;
    public float Damage => m_Damage;
    public void ChangeAttack(float damage) {
        m_Damage = damage;
    
    }
}
