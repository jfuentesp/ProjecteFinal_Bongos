using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBPlayerSuccesfulParryState : MBState
{
    private PJSMB m_PJ;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private bool parry;

    private void Awake()
    {
        m_PJ = GetComponent<PJSMB>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();

    }

    public override void InitState()
    {
        base.InitState();
        if (m_PJ.direccion == 0)
        {
            m_Animator.Play("parriedPose");
        }
        else if (m_PJ.direccion == 1)
        {
            m_Animator.Play("parriedPoseDown");
        }
        else if (m_PJ.direccion == 2)
        {
            m_Animator.Play("parriedPoseUp");
        }
    }
    public void Exit()
    {
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
    }

}