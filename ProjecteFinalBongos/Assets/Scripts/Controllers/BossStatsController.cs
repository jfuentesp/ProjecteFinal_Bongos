using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatsController : MonoBehaviour
{
    [Header("Tiempos")]
    [SerializeField]
    public TimesScriptable m_BossTimes;
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
    public BossBaseStats m_bossBaseStats;
    [Header("Stats")]
    [SerializeField]
    public float m_Velocity;
    [SerializeField]
    public float m_AttackTime = 1f;
    [SerializeField]
    public float m_Strength;
    [SerializeField]
    public float m_Defense;
    private void Start()
    {
        m_Velocity = m_bossBaseStats.speed;
        m_Strength = m_bossBaseStats.attack;
        Debug.Log(m_Strength);
        m_Defense = m_bossBaseStats.defense;
    }

}
