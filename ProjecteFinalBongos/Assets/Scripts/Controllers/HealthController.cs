using m17;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class HealthController : MonoBehaviour, IHealable, IDamageable
{
    private const float MAXHP = 100f;
    [SerializeField]
    private float m_HP = MAXHP;
    public float HP { get => m_HP; }

    public Action<EstadosAlterados> OnChange;

    public void Damage(float damageAmount)
    {
            m_HP -= damageAmount;
            if (m_HP < 0)
                m_HP = 0;
    }
    public void Heal(float healAmount)
    {
        m_HP += healAmount;
        if (m_HP < 0)
            m_HP = 0;

    }
}


