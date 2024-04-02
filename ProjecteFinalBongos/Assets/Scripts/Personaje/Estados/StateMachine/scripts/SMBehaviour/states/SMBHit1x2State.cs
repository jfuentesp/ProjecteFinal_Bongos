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
        StartCoroutine(comboTime());
    }
    IEnumerator comboTime()
    {
        yield return new WaitForSeconds(0.3f);
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
        StopAllCoroutines();
        m_StateMachine.ChangeState<SMBHit1x3State>();


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

