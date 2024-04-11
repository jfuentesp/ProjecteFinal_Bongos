using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamage : MonoBehaviour
{
    private float m_Damage;
    public float Damage { get { return m_Damage; } }
    private SMBComboState m_ComboState;
    void Start()
    {
        m_ComboState = transform.parent.GetComponent<SMBComboState>();
        m_ComboState.OnAttack += ChangeAttack;
    }

    private void ChangeAttack(float damage) { 
        m_Damage = damage;
    
    }
}
