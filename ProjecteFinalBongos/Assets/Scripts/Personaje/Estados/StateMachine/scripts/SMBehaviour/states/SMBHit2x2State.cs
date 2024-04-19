using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBHit2x2State : SMBComboState
{
    private float damage;
    [SerializeField]
    private GameObject projectile;
    public override void InitState()
    {
        base.InitState();
        if (m_PJ.direccion == 1)
        {
            m_Animator.Play("attack2x2Down");
        }
        else if (m_PJ.direccion == 2)
        {
            m_Animator.Play("attack2x2Up");
        }
        else if (m_PJ.direccion == 0)
        {
            m_Animator.Play("attack2x2");
        }
        StartCoroutine( AttackBehaviour());
        SetDamage();
   
    }
    IEnumerator AttackBehaviour()
    {
        if (m_PJ.PlayerAbilitiesController.AtaquesMejoradosDisponibles.Contains("2x2better"))
        {
            if (m_PJ.direccion == 1)
            {
                m_Rigidbody.velocity = transform.up * 15f;
                GameObject p1 = Instantiate(projectile);
                GameObject p2 = Instantiate(projectile);
                GameObject p3 = Instantiate(projectile);
                p1.transform.up = -transform.up;
                p2.transform.up = -transform.up + transform.right;
                p3.transform.up = -transform.up  -transform.right;
                p1.transform.position = transform.position;
                p2.transform.position = transform.position;
                p3.transform.position = transform.position;    
            }
            else if (m_PJ.direccion == 2)
            {
                m_Rigidbody.velocity = -transform.up * 15f;
                GameObject p1 = Instantiate(projectile);
                GameObject p2 = Instantiate(projectile);
                GameObject p3 = Instantiate(projectile);
                p1.transform.up = transform.up;
                p2.transform.up = transform.up + transform.right;
                p3.transform.up = transform.up - transform.right;
                p1.transform.position = transform.position;
                p2.transform.position = transform.position;
                p3.transform.position = transform.position;
            }
            else if (m_PJ.direccion == 0)
            {
                m_Rigidbody.velocity = -transform.right * 15f;
                GameObject p1 = Instantiate(projectile);
                GameObject p2 = Instantiate(projectile);
                GameObject p3 = Instantiate(projectile);
                p1.transform.up = transform.right;
                p2.transform.up = transform.right + transform.up;
                p3.transform.up = transform.right - transform.up;
                p1.transform.position = transform.position;
                p2.transform.position = transform.position;
                p3.transform.position = transform.position;
            }
        }
        else {
            if (m_PJ.direccion == 1)
            {
                m_Rigidbody.velocity = transform.up * 15f;
            }
            else if (m_PJ.direccion == 2)
            {
                m_Rigidbody.velocity = -transform.up * 15f;
            }
            else if (m_PJ.direccion == 0)
            {
                m_Rigidbody.velocity = -transform.right * 15f;
            }
        }

        yield return new WaitForSeconds(0.1f);
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
        StopAllCoroutines();
        m_StateMachine.ChangeState<SMBHit2x3State>();
    }

    protected override void SetDamage()
    {
        base.SetDamage();
    }
}