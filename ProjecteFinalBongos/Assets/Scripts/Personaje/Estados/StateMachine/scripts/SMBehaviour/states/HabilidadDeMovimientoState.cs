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
    private string m_habilidad;
    private float dashSpeed = 10f;
    private float dashSpeedInvicible = 5f;
    [SerializeField]
    private GameObject m_SlowDownZone;
    [SerializeField]
    private EstadoEvent changeEstado;
    private new void Awake()
    {
        base.Awake();
        m_PJ = GetComponent<PJSMB>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_habilidad = m_PJ.Movement;

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
                print("a");
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
            case "InvinvibleDash":
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
                m_Rigidbody.velocity = m_PJ.MovementAction.ReadValue<Vector2>() * dashSpeedInvicible;
                changeEstado.Raise(EstadosAlterados.Invencible);
                yield return new WaitForSeconds(0.2f);
                Exit();
                break;
            case "SlowDown":
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
                GameObject slowDown = Instantiate(m_SlowDownZone);
                slowDown.transform.position = transform.position;
                Exit();
                break;
            case "Fast":
                changeEstado.Raise(EstadosAlterados.Peus_Lleugers);
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
