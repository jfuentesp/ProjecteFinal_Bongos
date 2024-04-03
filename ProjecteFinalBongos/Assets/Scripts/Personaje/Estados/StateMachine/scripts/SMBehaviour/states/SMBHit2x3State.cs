using m17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBHit2x3State : SMBComboState
{
    public override void Init()
    {
        base.Init();
        m_Animator.Play("attack2x3");
        AttackBehaviour();
    }
    public void AttackBehaviour()
    {
        m_Rigidbody.velocity = transform.right * 20f;
   
    }
    protected override void OnComboFailedAction()
    {

    }

    protected override void OnComboSuccessAction()
    {

        StopAllCoroutines();
        m_StateMachine.ChangeState<SMBHit1AplastanteState>();

    }

    protected override void OnEndAction()
    {
        StopAllCoroutines();
        m_StateMachine.ChangeState<SMBIdleState>();
    }

    protected override void OnComboSuccessActionAttack2()
    {
        StopAllCoroutines();
        m_StateMachine.ChangeState<SMBHit2State>();
    }
}
