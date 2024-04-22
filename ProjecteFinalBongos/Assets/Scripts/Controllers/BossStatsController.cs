using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatsController : MonoBehaviour
{

    [Header("Tiempos")]
    [SerializeField]
    public TimesScriptable m_bossTimes;
    [Header("Modificadores")]
    [SerializeField]
    private float m_ParalizedLifeModifier = 2f;
    [SerializeField]
    private float m_WrathLifeModifier = 1.5f;
    private float m_WrathSpeedModifier;
    private float m_WrathStrengthModifier;
    private float m_WetModifier;
    private float m_StrengthModifier;
    private float m_PoisonModifier;
    private float m_BurntModifier;
    private float m_VelocityModifier;
    [Header("MaxMin")]
    private float poisonMin = 5;
    private float poisonMax = 8;
    private float burntMin = 10;
    private float burntMax = 16;
    private float wrathStrenghtMin = 30;
    private float wrathStrengthMax = 46;
    private float wrathSpeedMin = 50;
    private float wrathSpeedMax = 61;
    private float wetMin = 10;
    private float wetMax = 21;
    private float strengthMin = 20;
    private float strengthMax = 61;
    private float speedMin = 50;
    private float speedMax = 71;
    [Header("Resistances")]
    public float m_WaterResistance = 5f;
    public float m_FireResistance = 5f;
    public float m_ElectricResistance = 5f;
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
        m_Defense = m_bossBaseStats.defense;

    }
    public float getModifier(string modifier)
    {
        switch (modifier)
        {
            case "Fast":
                m_VelocityModifier = Random.Range(speedMin, speedMax);
                return m_VelocityModifier;
            case "Wet":
                m_WetModifier = Random.Range(wetMin, wetMax);
                return m_WetModifier;
            case "Burnt":
                m_BurntModifier = Random.Range(burntMin, burntMax);
                return m_BurntModifier;
            case "WrathLife":
                return m_WrathLifeModifier;
            case "Paralized":
                return m_ParalizedLifeModifier;
            case "WrathStrength":
                m_WrathStrengthModifier = Random.Range(wrathStrenghtMin, wrathStrengthMax);
                return m_WrathStrengthModifier;
            case "WrathSpeed":
                m_WrathSpeedModifier = Random.Range(wrathSpeedMin, wrathSpeedMax);
                return m_WrathSpeedModifier;
            case "Strong":
                m_StrengthModifier = Random.Range(strengthMin, strengthMax);
                return m_StrengthModifier;
            case "Poison":
                m_PoisonModifier = Random.Range(poisonMin, poisonMax);
                return m_PoisonModifier;
            default:
                return 0;
        }

    }

}
