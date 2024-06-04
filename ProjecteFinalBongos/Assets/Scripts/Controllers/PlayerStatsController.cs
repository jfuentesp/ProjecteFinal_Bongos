using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerStatsController : MonoBehaviour, IBuffable
{
    private HealthController m_HealthController;
    public HealthController Health => m_HealthController;
    [Header("Tiempos")]
    [SerializeField]
    public TimesScriptable m_playerTimes;
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
    [SerializeField]
    private Sword m_Sword;
    public Sword Sword => m_Sword;
    [SerializeField]
    private Sword m_InitialSword;
    [SerializeField]
    private Armor m_Armor;
    public Armor Armor => m_Armor;

    public Action<StatType, float> OnApplyBuff;

    private void Awake()
    {
        m_Velocity = m_PlayerBaseStats.m_BaseVelocity;
        m_AttackTime = m_PlayerBaseStats.m_BaseAttackTime;
        m_Strength = m_PlayerBaseStats.m_BaseStrength;
        m_Defense = m_PlayerBaseStats.m_BaseDefense;
        EquipSword(m_InitialSword);
    }

    private void Start()
    {
        m_HealthController = GetComponent<HealthController>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            m_HealthController.Heal(m_HealthController.HPMAX);
        }
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

    public void EquipArmor(Armor armor) { 
        m_Armor = armor;
        m_Defense += m_Armor.defense;
        m_Velocity += m_Armor.speed;
        if (m_Armor.propiedades.Contains(EquipablePropertiesEnum.ESTADO))
        {
            StartCoroutine(m_Armor.Regen(gameObject));
        }
    }
    public void UnequipArmor()
    {
        if (m_Armor == null)
            return;
        m_Defense -= m_Armor.defense;
        m_Velocity -= m_Armor.speed;
        if (m_Armor.propiedades.Contains(EquipablePropertiesEnum.ENCHANTED))
        {
            StopCoroutine(m_Armor.Regen(gameObject));
        }
        m_Armor = null;
    }

    public void EquipSword(Sword sword)
    {
        m_Sword = sword;
        m_Strength += sword.attack;
        m_Velocity += sword.speed;
        m_AttackTime += sword.speedAttack;
      
    }
    public void UnequipSword()
    {
        if (m_Sword == null)
            return;
        m_Strength -= m_Sword.attack;
        m_Velocity -= m_Sword.speed;
        m_AttackTime -= m_Sword.speedAttack;
        m_Sword = null;
    }

    public void IncreaseDamage(float damageUp)
    {
        m_Strength += damageUp;
        Debug.Log(string.Format("El valor de daño ha cambiado de {0} a {1}", m_Strength - damageUp, m_Strength));
    }

    public void DecreaseDamage(float damageDown)
    {
        m_Strength -= damageDown;
        Debug.Log(string.Format("El valor de daño ha cambiado de {0} a {1}", m_Strength + damageDown, m_Strength));
    }

    public void IncreaseDefense(float defenseUp)
    {
        m_Defense += defenseUp;
        Debug.Log(string.Format("El valor de defensa ha cambiado de {0} a {1}", m_Defense - defenseUp, m_Defense));
    }

    public void DecreaseDefense(float defenseDown)
    {
        m_Defense -= defenseDown;
        Debug.Log(string.Format("El valor de defensa ha cambiado de {0} a {1}", m_Defense + defenseDown, m_Defense));
    }

    public void IncreaseHealth(float healthUp)
    {
        m_HealthController.IncreaseHP(healthUp);
        Debug.Log(string.Format("El valor de vida ha cambiado de {0} a {1}", m_HealthController.HPMAX - healthUp, m_HealthController.HPMAX));
    }

    public void IncreaseSpeed(float speedUp)
    {
        m_Velocity += speedUp;
        Debug.Log(string.Format("El valor de velocidad ha cambiado de {0} a {1}", m_Velocity - speedUp, m_Velocity));
    }

    public void DecreaseSpeed(float speedDown)
    {
        m_Velocity -= speedDown;
        Debug.Log(string.Format("El valor de velocidad ha cambiado de {0} a {1}", m_Velocity + speedDown, m_Velocity));
    }

    public void IncreaseAttackSpeed(float atkspeedUp)
    {
        m_AttackTime += atkspeedUp;
        Debug.Log(string.Format("El valor de velocidad de ataque ha cambiado de {0} a {1}", m_AttackTime - atkspeedUp, m_AttackTime));
    }

    public IEnumerator AttackBuff(float statAmount, float duration)
    {
        IncreaseDamage(statAmount);
        yield return new WaitForSeconds(duration);
        DecreaseDamage(statAmount);
    }

    public IEnumerator DefenseBuff(float statAmount, float duration)
    {
        IncreaseDefense(statAmount);
        yield return new WaitForSeconds(duration);
        DecreaseDefense(statAmount);
    }

    public IEnumerator SpeedBuff(float statAmount, float duration)
    {
        IncreaseSpeed(statAmount);
        yield return new WaitForSeconds(duration);
        DecreaseSpeed(statAmount);
    }

    public void BuffStat(StatType statToBuff, float statAmount, float duration)
    {
        switch (statToBuff)
        {
            case StatType.ATTACK:
                StartCoroutine(AttackBuff(statAmount, duration));

                break;
            case StatType.DEFENSE:
                StartCoroutine(DefenseBuff(statAmount, duration));
                break;
            case StatType.SPEED:
                StartCoroutine((SpeedBuff(statAmount, duration)));
                break;
        }
        OnApplyBuff.Invoke(statToBuff, duration);
    }
}