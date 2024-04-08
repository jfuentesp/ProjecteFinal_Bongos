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
        m_Animator.Play("attack2x1");
        AttackBehaviour();
    }
    public void AttackBehaviour()
    {
        m_Rigidbody.velocity = transform.right * 8f;
        
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

