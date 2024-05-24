using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SMBPlayerStopState : SMState
{
    private PJSMB m_PJ;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;


    private new void Awake()
    {
        m_PJ = GetComponent<PJSMB>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Animator.speed = 1.0f;

    }

    public override void InitState()
    {
        base.InitState();
        if (m_PJ.direccion == 0)
        {
            m_Animator.Play("idlePlayer");
        }
        else if (m_PJ.direccion == 1)
        {
            m_Animator.Play("idleDown");
        }
        else if (m_PJ.direccion == 2)
        {
            m_Animator.Play("idleUp");
        }
        m_Rigidbody.velocity = Vector2.zero;
    }
    public override void ExitState()
    {
        base.ExitState();
    }
    public void Exit()
    {
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
    }
 
}
