using m17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBHit1x2State : SMBComboState
{
    public override void Init()
    {
        base.Init();
        m_Animator.Play("attack1x2");
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
 
        m_StateMachine.ChangeState<SMBIdleState>();
    }

    protected override void OnComboSuccessActionAttack2()
    {
        m_StateMachine.ChangeState<SMBHit2State>();
    }
}

