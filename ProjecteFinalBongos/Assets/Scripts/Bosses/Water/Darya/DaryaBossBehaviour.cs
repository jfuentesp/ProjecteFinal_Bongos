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
[RequireComponent(typeof(HealthController))]
[RequireComponent(typeof(DaryaWavesState))]
[RequireComponent(typeof(DaryaTornadosState))]
[RequireComponent(typeof(DaryaAreaAttackState))]

public class DaryaBossBehaviour : BossBehaviour
{

    private Coroutine m_PlayerDetectionCoroutine;
    private enum Phase { ONE, TWO }
    private Phase m_CurrentPhase;
    [Header("Segunda fase")]
    [SerializeField] private float m_TimeBetwwenAttacksSecondPhase;
    private bool atacando;

    private new void Awake()
    {
        base.Awake();
        m_CurrentPhase = Phase.ONE;
        m_SalaPadre = GetComponentInParent<SalaBoss>();
        GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<DaryaWavesState>();
        };
        GetComponent<DaryaTornadosState>().onTornadoSpawned = (GameObject obj) =>
        {
            StartCoroutine(SetAttack());
        };
        GetComponent<DaryaAreaAttackState>().onAttackSpawned = (GameObject obj) =>
        {
            StartCoroutine(SetAttack());
        };
        atacando = false;
        m_StateMachine.ChangeState<SMBIdleState>();
    }

    protected new void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_CurrentPhase == Phase.ONE)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHitBox") && collision.CompareTag("DaryaWave"))
            {
                if (collision.TryGetComponent<AttackDamage>(out AttackDamage damage))
                {
                    recibirDaño(damage.Damage);
                }
            }
        }
        else if (m_CurrentPhase == Phase.TWO)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHitBox"))
            {
                if (collision.TryGetComponent<AttackDamage>(out AttackDamage damage))
                {
                    recibirDaño(damage.Damage);
                }
            }
        }
    }
    protected override void VidaCero()
    {
        base.VidaCero();
        if (m_CurrentPhase == Phase.TWO)
        {
            m_IsAlive = false;
            OnBossDeath?.Invoke();
            Destroy(gameObject);
        }
        else if (m_CurrentPhase == Phase.ONE)
        {
            m_HealthController.Heal(m_HealthController.HPMAX);
            m_CurrentPhase = Phase.TWO;
            StartCoroutine(SetAttack());
        }
    }

    private IEnumerator SetAttack()
    {
        if (!atacando)
        {
            atacando = true;
            yield return new WaitForSeconds(m_TimeBetwwenAttacksSecondPhase);

            int rand = Random.Range(0, 2);

            if (rand == 0)
            {
                m_StateMachine.ChangeState<DaryaTornadosState>();
            }
            else if (rand == 1)
            {
                m_StateMachine.ChangeState<DaryaAreaAttackState>();
            }
            atacando = false;
        }
    }
}
