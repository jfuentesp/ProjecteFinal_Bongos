using m17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBHit2AereoState : SMBComboState
{
    public override void Init()
    {
        base.Init();
        m_Animator.Play("attack2Aereo");
        StartCoroutine(AttackBehaviour());
    }
    IEnumerator AttackBehaviour()
    {
        Vector2 direccion = transform.right - transform.up;
        m_Rigidbody.velocity = direccion * 5f;
        yield return new WaitForSeconds(0.5f);
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
    }

  
}