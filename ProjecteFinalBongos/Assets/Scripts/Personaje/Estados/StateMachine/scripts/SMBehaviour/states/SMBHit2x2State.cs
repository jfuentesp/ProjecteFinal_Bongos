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
            m_Animator.Play("attack2x2Down");
        }
        else if (m_PJ.direccion == 2)
        {
            m_Animator.Play("attack2x2Up");
        }
        else if (m_PJ.direccion == 0)
        {
            m_Animator.Play("attack2x2");
        }
       StartCoroutine( AttackBehaviour());
    }
    IEnumerator AttackBehaviour()
    {
        if (m_PJ.direccion == 1)
        {
            m_Rigidbody.velocity = transform.up * 15f;
        }
        else if (m_PJ.direccion == 2)
        {
            m_Rigidbody.velocity = -transform.up * 15f;
        }
        else if (m_PJ.direccion == 0)
        {
            m_Rigidbody.velocity = -transform.right * 15f;
        }
        yield return new WaitForSeconds(0.1f);
        m_Rigidbody.velocity = Vector2.zero;


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