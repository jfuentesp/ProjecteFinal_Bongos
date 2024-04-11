using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBHit1AplastanteState : SMBComboState
{
    public override void InitState()
    {
        base.InitState();
        if (m_PJ.direccion == 1)
        {
            m_Animator.Play("attack1AplastanteDown");
        }
        else if (m_PJ.direccion == 2)
        {
            m_Animator.Play("attack1AplastanteUp");
        }
        else if (m_PJ.direccion == 0)
        {
            m_Animator.Play("attack1Aplastante");
        }
        SetDamage();
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

    protected override void SetDamage()
    {
        base.SetDamage();

        transform.GetChild(1).GetComponent<AttackDamage>().ChangeAttack(m_Damage + ((Strength * Random.Range(50, 101)) / 100));
    }
}