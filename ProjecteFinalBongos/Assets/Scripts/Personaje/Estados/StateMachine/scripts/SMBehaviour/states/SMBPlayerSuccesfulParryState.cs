using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBPlayerSuccesfulParryState : MBState
{
    private PJSMB m_PJ;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private Ability m_parry;


    private void Awake()
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
        m_Rigidbody.velocity = Vector2.zero;
        m_parry = m_PJ.PlayerAbilitiesController.Parry;
    }

    public void Exit()
    {
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
        GetComponent<DañoEnemigoListener>().enabled = true;
    }

}
