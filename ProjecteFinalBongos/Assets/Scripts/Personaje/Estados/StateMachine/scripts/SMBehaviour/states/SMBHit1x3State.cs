using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBHit1x3State : SMBComboState
{
    public override void InitState()
    {
        base.InitState();
        if (m_PJ.direccion == 1)
        {
            m_Animator.Play("attack1x3Down");
        }
        else if (m_PJ.direccion == 2)
        {
            m_Animator.Play("attack1x3Up");
        }
        else if (m_PJ.direccion == 0)
        {
            m_Animator.Play("attack1x3");

        }
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