using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBHit2x2State : SMBComboState
{
    public override void InitState()
    {
        base.InitState();
        if (m_PJ.direccion == 1)
        {
            m_Animator.Play("attack1x1Down");
        }
        else if (m_PJ.direccion == 2)
        {
            m_Animator.Play("attack1x1Up");
        }
        else if (m_PJ.direccion == 0)
        {
            m_Animator.Play("attack1x1");
            Debug.Log(m_PJ.direccion);
        }
        AttackBehaviour();
    }
    public void AttackBehaviour()
    {
        m_Rigidbody.velocity = -transform.right * 20f;
     
    }
    protected override void OnComboFailedAction()
    {
    }

    protected override void OnComboSuccessAction()
    {
    }

    protected override void OnEndAction()
    {
        StopAllCoroutines();
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
    }

    protected override void OnComboSuccessActionAttack2()
    {
        StopAllCoroutines();
        m_StateMachine.ChangeState<SMBHit2x3State>();
    }
}