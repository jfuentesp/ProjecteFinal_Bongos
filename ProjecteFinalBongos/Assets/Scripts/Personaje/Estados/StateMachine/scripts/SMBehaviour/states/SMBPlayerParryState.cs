using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBPlayerParryState : MBState
{
    private PJSMB m_PJ;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private SMBStunState m_State;
    public bool parry;
    private AbilityEnum m_parry;
    private LayerMask m_BossHurtBox;
    private LayerMask m_BossHitBox;
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
        m_State = GetComponent<SMBStunState>();
        m_Animator.speed = 1.0f;
        m_BossHurtBox = LayerMask.NameToLayer("BossHurtBox");
        m_BossHitBox = LayerMask.NameToLayer("BossHitBox");
    }

    public override void InitState()
    {
        base.InitState();
        if (m_PJ.direccion == 0)
        {
            m_Animator.Play("parryPose");
        }
        else if (m_PJ.direccion == 1)
        {
            m_Animator.Play("parryPoseDown");
        }
        else if (m_PJ.direccion == 2)
        {
            m_Animator.Play("parryPoseUp");
        }
        m_Rigidbody.velocity = Vector3.zero;
        m_parry = m_PJ.PlayerAbilitiesController.Parry;
 
    }
    public void InitWindow() { 
        parry = true;
        GetComponent<DañoEnemigoListener>().enabled = false;
    }

    public void ExitWindow()
    {
        parry = false;
        GetComponent<DañoEnemigoListener>().enabled = true;
        Exit();
    }
 
    public void Exit()
    {
        m_State.ChangeTime(1.5f);
        m_StateMachine.ChangeState<SMBStunState>();



    }

    private void parryAction(GameObject boss)
    {
        switch (m_parry)
        {
            case AbilityEnum.INVULNERABLEPARRY:
                m_ChangeEstado.Raise(EstadosAlterados.Invencible);
                break;
            case AbilityEnum.PARALIZATIONPARRY:
                boss.GetComponent<BossEstadosController>().AlternarEstado(EstadosAlterados.Paralitzat);
                break;
            case AbilityEnum.PARRYSPEED:
                m_ChangeEstado.Raise(EstadosAlterados.Peus_Lleugers);
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enabled)
        {
            if (parry && m_BossHurtBox == collision.gameObject.layer || parry && m_BossHitBox == collision.gameObject.layer)
            {
                parry = false;
                parryAction(collision.gameObject);
                m_StateMachine.ChangeState<SMBPlayerSuccesfulParryState>();
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enabled)
        {
            if (parry && m_BossHurtBox == collision.gameObject.layer || parry && m_BossHitBox == collision.gameObject.layer)
            {
                parry = false;
                parryAction(collision.gameObject);
                m_StateMachine.ChangeState<SMBPlayerSuccesfulParryState>();
            }

        }
    }

}
