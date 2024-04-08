using JetBrains.Annotations;
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
    [Range(0f, 0.9f)]
    private float m_HealingMultiplierPenalization;

    [Header("Healing animation")]
    [SerializeField]
    private string m_WalkAnimationName;

    private float m_HealingMultiplier;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        m_HealingMultiplier = 1;
    }

    public override void InitState()
    {
        base.InitState();

    }

    public override void ExitState()
    {
        base.ExitState();
 
    }

    private void Heal()
    {
        //Current HP += m_AmountToheal * m_HealingMultiplier;
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
        } 
        else
        {
            m_StateMachine.ChangeState<SMBDeathState>();
        }
    }

}
