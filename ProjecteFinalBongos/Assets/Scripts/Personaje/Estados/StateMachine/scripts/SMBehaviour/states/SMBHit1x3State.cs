using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBHit1x3State : SMBComboState
{
    public override void InitState()
    {
        base.InitState();
        m_Animator.Play("attack1x3");
        AttackBehaviour();
    }
    public void AttackBehaviour()
    {
        m_Rigidbody.velocity = transform.up * 5f;
        m_Rigidbody.gravityScale = 1f;
    }
   
     
    protected override void OnComboFailedAction()
    {

    }

    protected override void OnComboSuccessAction()
    {
      
        m_Rigidbody.gravityScale = 0;
        m_StateMachine.ChangeState<SMBHit1x4State>();


    }

    protected override void OnEndAction()
    {
     
        m_Rigidbody.gravityScale = 0;
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
    }

    protected override void OnComboSuccessActionAttack2()
    {
        m_Rigidbody.gravityScale = 0;
        m_StateMachine.ChangeState<SMBHit2AereoState>();
    }
}