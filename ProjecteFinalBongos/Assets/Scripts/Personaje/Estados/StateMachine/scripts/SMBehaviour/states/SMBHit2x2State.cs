using m17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBHit2x2State : SMBComboState
{
    public override void Init()
    {
        base.Init();
        m_Animator.Play("attack2x2");
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
        m_StateMachine.ChangeState<SMBIdleState>();
    }

    protected override void OnComboSuccessActionAttack2()
    {
        StopAllCoroutines();
        m_StateMachine.ChangeState<SMBHit2x3State>();
    }
}