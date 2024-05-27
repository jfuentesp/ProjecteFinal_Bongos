using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour, IHealable, IDamageable
{
    public Action onDeath;
    public Action onHurt;
    public Action onHeal;
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
            onDeath?.Invoke();
        }
        else
            if(damageAmount > 0)
                onHurt?.Invoke();

        //Debug.Log(string.Format(gameObject.name + " Received {0} damage. Remaining HP: {1}", damageAmount, m_HP));
    }

    public void Heal(float healAmount)
    {
        if (m_HP <= 0)
        {
            m_HP = 0;
            onDeath?.Invoke();
            return;
        }
        else
        {
            m_HP += healAmount;
            onHeal?.Invoke();
            if (m_HP > MAXHP)
                m_HP = MAXHP;
        }
       

        //Debug.Log(string.Format(gameObject.name + " Healed by {0} points. Remaining HP: {1}", healAmount, m_HP));
    }

    public void Revive()
    {
        m_HP = MAXHP;
    }

    public void Regenerate(float regenerationAmount, float duration, float tickDelay)
    {
        throw new NotImplementedException();
    }

    public void IncreaseHP(float amount)
    {
        MAXHP += amount;
        m_HP += amount;
    }
}

