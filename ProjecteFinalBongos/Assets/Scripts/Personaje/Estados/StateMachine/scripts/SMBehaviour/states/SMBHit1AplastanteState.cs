using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBHit1AplastanteState : SMBComboState
{
    public override void InitState()
    {
        base.InitState();
        m_Animator.Play("attack1Aplastante");
        StartCoroutine(comboTime());
        ChangeAttack();
    }
    IEnumerator comboTime()
    {
   
        yield return new WaitForSeconds(0.7f);
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
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
    }

    protected override void OnComboSuccessActionAttack2()
    {
     
    }

    protected override void ChangeAttack()
    {
  
        OnAttack.Invoke(m_StrongAttack + ((Strength * Random.Range(50, 101) / 100)));
    }
}