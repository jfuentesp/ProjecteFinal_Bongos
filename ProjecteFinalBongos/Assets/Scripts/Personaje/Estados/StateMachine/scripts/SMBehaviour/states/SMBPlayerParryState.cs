using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBPlayerParryState : MBState
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
            m_Animator.Play("parryPose");
        }
        else if (m_PJ.direccion == 1)
        {
            m_Animator.Play("parryPoseDown");
        }
        else if (m_PJ.direccion == 2)
        {
            m_Animator.Play("parryPoseUp");
        }
        m_Rigidbody.velocity = Vector3.zero;
    }
    public void InitWindow() { 
        parry = true;
    }

    public void ExitWindow() { 
        parry= false;
    }
    public void Exit()
    {
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
    }

    public void OnCollisionExit(Collision collision)
    {
        if (this.enabled) {
            if (parry)
                m_StateMachine.ChangeState<SMBPlayerSuccesfulParryState>();
        }
    }

}
