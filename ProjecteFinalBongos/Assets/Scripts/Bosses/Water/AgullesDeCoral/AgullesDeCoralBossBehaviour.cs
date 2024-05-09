using NavMeshPlus.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentOverride2d))]
[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(AgullaChargeState))]
[RequireComponent(typeof(AgullaDeCoralFlipState))]
[RequireComponent(typeof(AgullaCoralAreaAttack))]
[RequireComponent(typeof(SMBParriedState))]
[RequireComponent(typeof(HealthController))]
public class AgullesDeCoralBossBehaviour : BossBehaviour
{
    private Pool m_Pool;
    [Header("Coral Summoning")]
    [SerializeField] LayerMask m_LayerParaElCircleCast;
    [SerializeField] private float m_RangeCoralCircleCast;
    [SerializeField] private int m_RangeForCoralSummoning;
    [SerializeField, Range(0.0f, 1.0f)] private float m_HealingReduction;
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
            m_StateMachine.ChangeState<AgullaCoralAreaAttack>();
        };
        GetComponent<AgullaDeCoralFlipState>().OnAttackParried = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
        GetComponent<AgullaDeCoralFlipState>().OnAttackMissed = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<AgullaChargeState>();
        };
        GetComponent<SMBParriedState>().OnRecomposited = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<AgullaChargeState>();
        };
        GetComponent<AgullaCoralAreaAttack>().OnPlayerHitted = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<AgullaChargeState>();
        };
        GetComponent<AgullaCoralAreaAttack>().OnStopDetectingPlayer = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<AgullaChargeState>();
        };
        GetComponent<AgullaCoralAreaAttack>().OnParriedAttack = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };

        m_Pool = LevelManager.Instance._BulletPool;

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
            if(collision.gameObject.TryGetComponent<AttackDamage>(out Damage))
            {
                recibirDaño(Damage.Damage);
                SoltarCoral(Damage.Damage);
            }
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("BossHitBox"))
        {
            AgullaDeCoralCoralBullet coralBullet;
            if (collision.gameObject.TryGetComponent<AgullaDeCoralCoralBullet>(out coralBullet))
            {
                if (coralBullet.enabled)
                {
                    m_HealthController.Heal(coralBullet.Damage * m_HealingReduction);
                }
            }
        }
    }

    private void SoltarCoral(float damage)
    {
        Vector2 posicionHuevo = m_SalaPadre.GetPosicionAleatoriaEnSala();
        RaycastHit2D hit = Physics2D.CircleCast(posicionHuevo, m_RangeCoralCircleCast, posicionHuevo, m_RangeCoralCircleCast, m_LayerParaElCircleCast);
        if (Vector2.Distance(transform.position, posicionHuevo) > m_RangeForCoralSummoning || hit.collider != null)
        {
            SoltarCoral(damage);
        }
        else
        {
            GameObject arrow = m_Pool.GetElement();
            arrow.transform.position = transform.position;
            arrow.GetComponent<CircleCollider2D>().enabled = false;
            arrow.SetActive(true);
            arrow.GetComponent<AgullaDeCoralCoralBullet>().enabled = true;
            arrow.GetComponent<AgullaDeCoralCoralBullet>().Init(posicionHuevo, transform, damage);
        }
    }
    protected override void VidaCero()
    {
        base.VidaCero();
        m_IsAlive = false;
        OnBossDeath?.Invoke();
        Destroy(gameObject);
    }
}
