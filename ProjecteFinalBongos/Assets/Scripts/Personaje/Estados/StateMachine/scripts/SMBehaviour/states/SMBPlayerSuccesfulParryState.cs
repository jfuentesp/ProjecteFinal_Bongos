using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBPlayerSuccesfulParryState : MBState
{
    private PJSMB m_PJ;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private string m_parry;
    [SerializeField]
    private EstadoEvent m_ChangeEstado;
    [SerializeField]
    private EstadoEvent m_ChangeEstadoEnemigo;

    private void Awake()
    {
        m_PJ = GetComponent<PJSMB>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_parry = m_PJ.Parry;
    }

    public override void InitState()
    {
        base.InitState();
        parryAction();
    }
    private void parryAction() {
        switch (m_parry) {
            case "Invincible":
                m_ChangeEstado.Raise(EstadosAlterados.Invencible);
                Exit();
                break;
            case "Paralized":
                m_ChangeEstadoEnemigo.Raise(EstadosAlterados.Paralitzat);
                Exit();
                break;
            case "Fast":
                m_ChangeEstado.Raise(EstadosAlterados.Peus_Lleugers);
                Exit();
                break;
            default:
                break;
        }
    }
    public void Exit()
    {
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
        GetComponent<DañoEnemigoListener>().enabled = true;
    }

}
