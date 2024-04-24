using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBHit1x3State : SMBComboState
{
    [SerializeField]
    private EstadoEvent m_ChangeEstado;
    public override void InitState()
    {
        base.InitState();
        if (m_PJ.direccion == 1)
        {
            m_Animator.Play("attack1x3Down");
        }
        else if (m_PJ.direccion == 2)
        {
            m_Animator.Play("attack1x3Up");
        }
        else if (m_PJ.direccion == 0)
        {
            m_Animator.Play("attack1x3");

        }
        m_Animator.speed = m_PJ.PlayerStatsController.m_AttackTime;
        if (m_PJ.PlayerAbilitiesController.AtaquesMejoradosDisponibles.Contains("1x3Paralize")) {
            int rnd = Random.Range(0, 11);
            if (rnd >= 5 && rnd <= 10)
            {
                m_ChangeEstado.Raise(EstadosAlterados.Paralitzat);
            }
        }
    
        SetDamage();
    }
 
     
    protected override void OnComboFailedAction()
    {

    }

    protected override void OnComboSuccessAction()
    {
      
    
        m_StateMachine.ChangeState<SMBHit1x4State>();


    }

    protected override void OnEndAction()
    {
     
    
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
    }

    protected override void OnComboSuccessActionAttack2()
    {
    }

    protected override void SetDamage()
    {
        base.SetDamage();
    }
}