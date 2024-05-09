using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackDamage : MonoBehaviour
{
    [SerializeField]
    private float m_Damage;
    [SerializeField]
    private BossStatsController m_StatsController;
    [SerializeField]
    private GameObject m_player;
    public float Damage => m_Damage;

    [SerializeField] private EstadosAlterados m_EstadoAlterado;
    public EstadosAlterados EstadoAlterado => m_EstadoAlterado;
    private void Start()
    {
        m_StatsController = GetComponentInParent<BossStatsController>();

        SetDamage();
    }

    private void SetDamage()
    {
        if (m_StatsController)
            m_Damage += (m_StatsController.m_Strength * UnityEngine.Random.Range(50, 101) / 100);
    }

    public void SetDamage(float _damage)
    {
        m_Damage = _damage;
        SetDamage();
    }
}
