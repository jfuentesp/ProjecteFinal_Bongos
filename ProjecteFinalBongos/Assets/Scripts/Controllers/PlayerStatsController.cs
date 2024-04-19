using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    [Header("Tiempos")]
    [SerializeField]
    public TimesScriptable m_playerTimes;
    [Header("Modificadores")]
    [SerializeField]
    public float m_ParalizedLifeModifier;
    [SerializeField]
    public float m_WrathLifeModifier;
    public float m_WrathSpeedModifier;
    public float m_WrathStrengthModifier;
    public float m_WetModifier;
    public float m_StrengthModifier;
    public float m_PoisonModifier;
    public float m_BurntModifier;
    public float m_VelocityModifier;
    [Header("BaseStats")]
    [SerializeField]
    public PlayerBase m_PlayerBaseStats;
    [Header("Stats")]
    [SerializeField]
    public float m_Velocity;
    [SerializeField]
    public float m_AttackTime;
    [SerializeField]
    public float m_Strength;
    [SerializeField]
    public float m_Defense;

    private void Start()
    {
        m_Velocity = m_PlayerBaseStats.m_BaseVelocity;
        m_AttackTime = m_PlayerBaseStats.m_BaseAttackTime;
        m_Strength = m_PlayerBaseStats.m_BaseStrength;
        m_Defense = m_PlayerBaseStats.m_BaseDefense;
    }
}
