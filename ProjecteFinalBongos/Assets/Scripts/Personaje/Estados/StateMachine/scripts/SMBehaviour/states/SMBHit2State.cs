using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;


public class SMBHit2State : SMBComboState
{
    public override void InitState()
    {
        base.InitState();
        if (m_PJ.direccion == 1)
        {
            m_Animator.Play("attack2x1Down");
        }
        else if (m_PJ.direccion == 2)
        {
            m_Animator.Play("attack2x1Up");
        }
        else if (m_PJ.direccion == 0)
        {
            m_Animator.Play("attack2x1");
            Debug.Log(m_PJ.direccion);
        }
        AttackBehaviour();
    }
    public void AttackBehaviour()
    {
        if (m_PJ.direccion == 1)
        {
            m_Rigidbody.velocity = transform.up * -4f;
        }
        else if (m_PJ.direccion == 2)
        {
            m_Rigidbody.velocity = transform.up * 4f;
        }
        else if (m_PJ.direccion == 0)
        {
            m_Rigidbody.velocity = transform.right * 4f;
        }

        
    }
    protected override void OnComboFailedAction()
    {

    }

    protected override void OnComboSuccessAction()
    {

        m_StateMachine.ChangeState<SMBHit1x3State>();


    }

    protected override void OnEndAction()
    {
       
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
    }

    protected override void OnComboSuccessActionAttack2()
    {
        m_StateMachine.ChangeState<SMBHit2x2State>();
    }
}

