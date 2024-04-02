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
        StartCoroutine(AttackBehaviour());
    }
    IEnumerator AttackBehaviour()
    {
        m_Rigidbody.velocity = -transform.right * 20f;
        yield return new WaitForSeconds(0.1f);
        m_Rigidbody.velocity = Vector2.zero;
        m_ComboHandler.InitComboWindow();
        yield return new WaitForSeconds(0.5f);
        m_ComboHandler.EndComboWindow();
        OnEndAction();
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