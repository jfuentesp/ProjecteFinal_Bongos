using NavMeshPlus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentOverride2d))]
[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(AgullaChargeState))]
[RequireComponent(typeof(AgullaDeCoralFlipState))]
[RequireComponent(typeof(SMBParriedState))]
[RequireComponent(typeof(HealthController))]
[RequireComponent(typeof(DeathState))]

public class MiniAgullesDeCoralScript : BossBehaviour
{
    private new void Awake()
    {
        base.Awake();
        m_SalaPadre = GetComponentInParent<SalaBoss>();
        GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<AgullaChargeState>();
        };
        GetComponent<AgullaChargeState>().OnChargeMissed = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<AgullaDeCoralFlipState>();
        };
        GetComponent<AgullaChargeState>().OnChargePlayer = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<AgullaDeCoralFlipState>();
        };
        GetComponent<AgullaDeCoralFlipState>().OnAttackHitted = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<AgullaChargeState>();
        };
        GetComponent<AgullaDeCoralFlipState>().OnAttackMissed = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<AgullaChargeState>();
        };
        GetComponent<SMBParriedState>().OnRecomposited = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<AgullaChargeState>();
        };
        transform.GetChild(0).GetComponent<BossAttackDamage>().OnAttackParried = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
    }

    private void Start()
    {
        m_StateMachine.ChangeState<SMBIdleState>();
    }

    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        OnPlayerInSala?.Invoke();
    }
    protected new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHitBox"))
        {
            AttackDamage Damage;
            if (collision.gameObject.TryGetComponent<AttackDamage>(out Damage))
            {
                recibirDaño(Damage.Damage);
            }
        }
    }
    private void MatarBoss()
    {
        Destroy(gameObject);
    }

    protected override void VidaCero()
    {
        base.VidaCero();
        StopAllCoroutines();
        GetComponentInParent<SalaBoss>().OnPlayerIn -= Init;
        m_StateMachine.ChangeState<DeathState>();
        m_IsAlive = false;
        OnBossDeath?.Invoke();
        if (m_BossFinalSala)
            m_BossMuertoEvent.Raise();
    }
}
