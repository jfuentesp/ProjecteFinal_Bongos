using NavMeshPlus.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentOverride2d))]
[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(SMBChargeState))]
[RequireComponent(typeof(LeviatanCrashWaveState))]
[RequireComponent(typeof(LeviatanMinionsSpawnState))]
[RequireComponent(typeof(SMBSingleAttackState))]
[RequireComponent(typeof(HealthController))]
[RequireComponent(typeof(SMBParriedState))]
public class LeviatanBossBehaviour : BossBehaviour
{
    private Coroutine m_DeteccionPlayerCoroutine;
    private Coroutine m_BeginSpawn;
    [Header("Variables mordisco")]
    [SerializeField] private float m_RadioMeleMordisco;
    [SerializeField] private LayerMask m_DeteccionMordisco;
    private int count = 0;
    private new void Awake()
    {
        base.Awake();
        m_SalaPadre = GetComponentInParent<SalaBoss>();
        GetComponent<SMBChargeState>().OnChargeMissed = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<LeviatanCrashWaveState>();
        };
        GetComponent<SMBChargeState>().OnChargeParried = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
        GetComponent<SMBChargeState>().OnChargePlayer = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<LeviatanCrashWaveState>();
        };
        GetComponent<SMBParriedState>().OnRecomposited = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<LeviatanCrashWaveState>();
        };
        GetComponent<SMBSingleAttackState>().OnStopDetectingPlayer = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<LeviatanCrashWaveState>().OnSpawnedWave = (GameObject obj) =>
        {
            ComprobarSiMorder();
        };
        GetComponent<LeviatanMinionsSpawnState>().OnSpawnFinished = (GameObject obj) =>
        {
            ComprobarSiMorder();
        };
        GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBIdleState>().OnPlayerEnter += EmpezarCorrutina;
        m_StateMachine.ChangeState<SMBIdleState>();
    }

    private void EmpezarCorrutina(GameObject @object)
    {
        m_DeteccionPlayerCoroutine = StartCoroutine(PlayerDetectionCoroutine());
        m_BeginSpawn = StartCoroutine(LeviatanMinionsSpawn());
    }

    private void ComprobarSiMorder()
    {
        RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position, m_RadioMeleMordisco, transform.position, m_RadioMeleMordisco, m_DeteccionMordisco);
        
        if (hitInfo.collider != null )
        {
            m_StateMachine.ChangeState<SMBSingleAttackState>();
        }
        else
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        }
    }

    public override void Init(Transform _Target)
    {
        base.Init(_Target);
    }

    private IEnumerator LeviatanMinionsSpawn() {
        while (m_IsAlive) {
            yield return new WaitForSeconds(1f);
            count++;
            if (count >= 10 && !m_IsBusy) {
                count = 0;
                m_StateMachine.ChangeState<LeviatanMinionsSpawnState>();
            }
        }

    }
    private IEnumerator PlayerDetectionCoroutine()
    {
        while (m_IsAlive)
        {

            if (m_PlayerAttackDetectionAreaType == CollisionType.CIRCLE)
            {
                RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position, m_AreaRadius, transform.position, m_AreaRadius, m_LayersToCheck);
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("Player") && !m_IsBusy)
                {
                    m_IsPlayerDetected = true;
                    m_StateMachine.ChangeState<SMBChargeState>();
                }
                else
                {
                    m_IsPlayerDetected = false;
                }
            }
            else
            {
                RaycastHit2D hitInfo = Physics2D.BoxCast(transform.position, m_BoxArea, transform.rotation.z, transform.position);
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("Player") && !m_IsBusy)
                {
                    m_IsPlayerDetected = true;
                }
                else
                {
                    m_IsPlayerDetected = false;
                }
            }
            yield return new WaitForSeconds(m_CheckingPlayerTimelapse);
        }
    }
}
