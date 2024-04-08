using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;


public class SMBHit1State : SMBComboState
{
    public override void InitState()
    {
        base.InitState();
        m_Animator.Play("attack1x1");
    }

    protected override void OnComboFailedAction()
    {
    }

    protected override void OnComboSuccessAction()
    {
        m_StateMachine.ChangeState<SMBHit1x2State>();


    }

    protected override void OnEndAction()
    {
    
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
    }

    protected override void OnComboSuccessActionAttack2()
    {
        StopAllCoroutines();
        m_StateMachine.ChangeState<SMBHit2State>();
    }
}

