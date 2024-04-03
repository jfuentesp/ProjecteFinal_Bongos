using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBIdleState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;

    [Header("Idle animation")]
    [SerializeField]
    private string m_IdleAnimationName;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
    }

    public override void InitState()
    {
        base.InitState();
        m_Rigidbody.velocity = Vector3.zero;
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
