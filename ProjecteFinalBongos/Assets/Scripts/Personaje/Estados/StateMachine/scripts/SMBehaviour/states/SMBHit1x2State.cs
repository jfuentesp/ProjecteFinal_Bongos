using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBHit1x2State : SMBComboState
{
    public override void InitState()
    {
        base.InitState();
        if (m_PJ.direccion == 1)
        {
            m_Animator.Play("attack1x2Down");
        }
        else if (m_PJ.direccion == 2)
        {
            m_Animator.Play("attack1x2Up");
        }
        else if (m_PJ.direccion == 0)
        {
            m_Animator.Play("attack1x2");
           
        }
        m_Animator.speed = m_PJ.PlayerStatsController.m_AttackTime;
        SetDamage();

    }

    protected override void OnComboFailedAction()
    {

    }

    protected override void OnComboSuccessAction()
    {
        m_StateMachine.ChangeState<SMBHit1x3State>();


    }

    protected override void OnEndAction()
    {
 
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
    }

    protected override void OnComboSuccessActionAttack2()
    {
        if (m_PJ.PlayerAbilitiesController.AtaquesMejoradosDisponibles.Exists(ability => ability.AbilityEnum.Equals(AbilityEnum.CANCEL)))
        {
            m_StateMachine.ChangeState<SMBHit2x3State>();
        }
    }

    protected override void SetDamage()
    {
        base.SetDamage();
    }
}

