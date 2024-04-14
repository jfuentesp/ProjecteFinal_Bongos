using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBSetFlyingState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;

    private Transform m_Target;

    [Header("Speed to fly")]
    [SerializeField]
    private float m_FlyingSpeed;

    [Header("Set Flying Animation")]
    [SerializeField]
    private string m_SetFlyingAnimationName;

    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Animator = GetComponent<Animator>();
        m_Boss = GetComponent<BossBehaviour>();
    }

    public override void InitState()
    {
        base.InitState();
        m_Animator.Play(m_SetFlyingAnimationName);
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
