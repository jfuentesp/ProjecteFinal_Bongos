using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBBelosHealingState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;

    [Header("Amount to heal")]
    [SerializeField]
    private float m_AmountToHeal;

    [Header("Time before the boss starts to heal")]
    [SerializeField]
    private float m_HealingDelay;

    [Header("Amount to substract to the healing multiplier with every healing cycle")]
    [Tooltip("Healing Multiplier starts at 1. With every healing loop, it substracts a 0.x quantity to the healin multiplier so it heals less with every cycle.")]
    [SerializeField, Range(0f, 0.9f)]
    private float m_HealingMultiplierPenalization;

    [Header("Healing animation")]
    [SerializeField]
    private string m_HealingAnimationName;

    private float m_HealingMultiplier;

    [SerializeField] private GameObject HealthParticles;
    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        m_Boss.OnBossDeath += SeMurioMientrasSeCuraba;
        m_HealingMultiplier = 1;
    }

    private void SeMurioMientrasSeCuraba()
    {
        if(m_HealingCoroutine != null)
        {
            StopAllCoroutines();
        }
    }

    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true);
        m_HealingCoroutine = StartCoroutine(HealingCoroutine());
        m_Animator.Play(m_HealingAnimationName);
    }

    public override void ExitState()
    {
        base.ExitState();
 
    }

    private void Heal()
    {
        m_Boss.CurarBoss(m_AmountToHeal * m_HealingMultiplier);
        //Current HP += m_AmountToheal * m_HealingMultiplier;
    }
    public void HealParticles() { 
        HealthParticles.SetActive(true);
    }
    private void Update()
    {

    }
    private Coroutine m_HealingCoroutine;
    private IEnumerator HealingCoroutine()
    {
        yield return new WaitForSeconds(m_HealingDelay);
        if(m_Boss.IsAlive)
        {
            Heal();
            //if(m_HealingMultiplier > 0)
            m_HealingMultiplier -= m_HealingMultiplierPenalization;
            // else se muere jjajajajaj
            HealthParticles.SetActive(false);
            m_StateMachine.ChangeState<SMBChaseState>();
        }
    }

}
