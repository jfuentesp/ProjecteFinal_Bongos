using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HabilidadDeMovimientoState : SMState
{
    private PJSMB m_PJ;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private SMBStunState m_State;
    private string m_habilidad = "Dash";
    private float dashSpeed = 10f;

    private new void Awake()
    {
        base.Awake();
        m_PJ = GetComponent<PJSMB>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_State = GetComponent<SMBStunState>();

    }

    public override void InitState()
    {
        base.InitState();
        StartCoroutine(habilidad());
    }

    IEnumerator habilidad()
    {
        switch (m_habilidad)
        {
            case "Dash":
                if (m_PJ.direccion == 0)
                {
                    m_Animator.Play("Dash");
                }
                else if (m_PJ.direccion == 1)
                {
                    m_Animator.Play("DashDown");
                }
                else if (m_PJ.direccion == 2)
                {
                    m_Animator.Play("DashUp");
                }
                m_Rigidbody.velocity = Vector3.zero;
                m_Rigidbody.velocity = m_PJ.MovementAction.ReadValue<Vector2>() * dashSpeed;
                yield return new WaitForSeconds(0.2f);
                Exit();
                break;

        }
    }

    private void Exit() { 
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
    }
    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();
    }
}
