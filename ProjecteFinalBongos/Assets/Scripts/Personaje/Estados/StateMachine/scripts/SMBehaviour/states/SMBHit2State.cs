using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class SMBHit2State : SMBComboState
{
    public override void InitState()
    {
        base.InitState();
        if (m_PJ.direccion == 1)
        {
            m_Animator.Play("attack2x1Down");
        }
        else if (m_PJ.direccion == 2)
        {
            m_Animator.Play("attack2x1Up");
        }
        else if (m_PJ.direccion == 0)
        {
            m_Animator.Play("attack2x1");
          
        }
        SetDamage();
        StartCoroutine(AttackBehaviour());
  
    }
    IEnumerator AttackBehaviour()
    {
        if (m_PJ.direccion == 1)
        {
            m_Rigidbody.velocity = transform.up * -4f;
        }
        else if (m_PJ.direccion == 2)
        {
            m_Rigidbody.velocity = transform.up * 4f;
        }
        else if (m_PJ.direccion == 0)
        {
            m_Rigidbody.velocity = transform.right * 4f;
        }
        yield return new WaitForSeconds(0.1f);
        m_Rigidbody.velocity = Vector2.zero;   
        
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
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
    }

    protected override void OnComboSuccessActionAttack2()
    {
        StopAllCoroutines();
        m_StateMachine.ChangeState<SMBHit2x2State>();
    }

    protected override void SetDamage()
    {
        base.SetDamage();
    }
}

