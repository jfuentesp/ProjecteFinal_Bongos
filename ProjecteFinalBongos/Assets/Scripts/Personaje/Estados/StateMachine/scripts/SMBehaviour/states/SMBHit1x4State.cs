using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SMBHit1x4State : SMBComboState
{
    [SerializeField]
    private EstadoEvent m_ChangeEstado;
    public override void InitState()
    {
        base.InitState();
        if (m_PJ.PlayerAbilitiesController.AtaquesMejoradosDisponibles.Exists(ability => ability.AbilityEnum.Equals(AbilityEnum.WHIRLWINDATTACK)))
        {
            m_Animator.Play("attack1x4Better");
        }
        else {
            m_Animator.Play("attack1x4");
        }
        if (m_PJ.PlayerAbilitiesController.AtaquesMejoradosDisponibles.Exists(ability => ability.AbilityEnum.Equals(AbilityEnum.FRENZYATTACK)))
        {
           m_ChangeEstado.Raise(EstadosAlterados.For�ut);
        }
        m_Animator.speed = m_PJ.PlayerStatsController.m_AttackTime;
        StartCoroutine(AttackBehaviour());
        SetDamage();
    }
    IEnumerator AttackBehaviour()
    {
        
        if (m_PJ.direccion == 1) {
            m_Rigidbody.velocity = transform.up * -8;
        }
        else if (m_PJ.direccion == 2)
        {
            m_Rigidbody.velocity = transform.up * 8;
        }
        else if (m_PJ.direccion == 0)
        {
            m_Rigidbody.velocity = transform.right * 8;
        }

        yield return new WaitForSeconds(0.2f);
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
        m_StateMachine.ChangeState<SMBHit2x2State>();
    }

    protected override void SetDamage()
    {
        base.SetDamage();
    }

}