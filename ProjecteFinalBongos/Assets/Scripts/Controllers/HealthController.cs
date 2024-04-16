using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour, IHealable, IDamageable
{
    public Action onDeath;
    [SerializeField]
    private float MAXHP = 100f;
    public float HPMAX => MAXHP;

    private float m_HP;

    private void Start()
    {
        m_HP = MAXHP;
    }

    public float HP { get { return m_HP; } }

    public void Damage(float damageAmount)
    {
        m_HP -= damageAmount;
        if (m_HP <= 0)
        {
            m_HP = 0;
            onDeath.Invoke();
        }   

        Debug.Log(string.Format("Received {0} damage. Remaining HP: {1}", damageAmount, m_HP));
    }

    public void Heal(float healAmount)
    {
        m_HP += healAmount;
        if (m_HP > MAXHP)
            m_HP = MAXHP;

        Debug.Log(string.Format("Healed by {0} points. Remaining HP: {1}", healAmount, m_HP));
    }
}

