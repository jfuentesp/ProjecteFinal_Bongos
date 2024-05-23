using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBPlayerParryState : SMState
{
    private PJSMB m_PJ;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private SMBStunState m_State;
    public bool parry;
    private Ability m_parry;
    private LayerMask m_BossHurtBox;
    private LayerMask m_BossHitBox;
    private LayerMask m_AllHitBox;
    [SerializeField]
    private EstadoEvent m_ChangeEstado;
    [SerializeField]
    private EstadoEvent m_ChangeEstadoEnemigo;
    private new void Awake()
    {
        m_PJ = GetComponent<PJSMB>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_State = GetComponent<SMBStunState>();
        m_Animator.speed = 1.0f;
        m_BossHurtBox = LayerMask.NameToLayer("BossHurtBox");
        m_BossHitBox = LayerMask.NameToLayer("BossHitBox");
        m_AllHitBox = LayerMask.NameToLayer("AllHitBox");
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
        GetComponent<FloatGameEventListener>().enabled = false;
    }

    public void ExitWindow()
    {
        parry = false;
        GetComponent<FloatGameEventListener>().enabled = true;
        Exit();
    }
 
    public void Exit()
    {
        m_State.ChangeTime(1.5f);
        m_StateMachine.ChangeState<SMBStunState>();



    }

    private void parryAction(GameObject boss)
    {
        switch (m_parry.AbilityEnum)
        {
            case AbilityEnum.INVULNERABLEPARRY:
                m_PJ.GetComponent<PlayerEstadosController>().AlternarEstado(EstadosAlterados.Invencible, 0.2f);
                break;
            case AbilityEnum.PARALIZATIONPARRY:
                boss.GetComponent<BossEstadosController>().AlternarEstado(EstadosAlterados.Paralitzat);
                break;
            case AbilityEnum.PARRYSPEED:
                m_PJ.GetComponent<PlayerEstadosController>().AlternarEstado(EstadosAlterados.Peus_Lleugers, 5f); 
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enabled)
        {
            if (parry && m_BossHurtBox == collision.gameObject.layer || parry && m_BossHitBox == collision.gameObject.layer || parry && m_AllHitBox == collision.gameObject.layer)
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
            if (parry && m_BossHurtBox == collision.gameObject.layer || parry && m_BossHitBox == collision.gameObject.layer || parry && m_AllHitBox == collision.gameObject.layer)
            {
                parry = false;
                parryAction(collision.gameObject);
                m_StateMachine.ChangeState<SMBPlayerSuccesfulParryState>();
            }

        }
    }

}
