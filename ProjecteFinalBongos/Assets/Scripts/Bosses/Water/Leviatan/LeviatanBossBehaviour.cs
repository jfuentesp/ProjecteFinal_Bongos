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
    [Header("Maximum Snakes")]
    [SerializeField] private int m_MaximumCreeps;
    [SerializeField] private int m_CurrentCreeps;
    private int count = 0;
    private new void Awake()
    {
        base.Awake();
        m_SalaPadre = GetComponentInParent<SalaBoss>();
        GetComponent<SMBChargeState>().OnChargeMissed = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<LeviatanCrashWaveState>();
        };
        GetComponent<SMBParalized>().OnStopParalized = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBBossStunState>().OnStopStun = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
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
        GetComponent<SMBSingleAttackState>().OnAttackStopped = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBSingleAttackState>().OnAttackParried = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
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
        transform.GetChild(0).GetComponent<BossAttackDamage>().OnAttackParried = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
        GetComponent<SMBParalized>().OnStopParalized = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBBossStunState>().OnStopStun = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBIdleState>().OnPlayerEnter += EmpezarCorrutina;
   
    }

    private void Start()
    {
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

        if (hitInfo.collider != null)
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
        m_Target = _Target;
        OnPlayerInSala?.Invoke();
    }

    private IEnumerator LeviatanMinionsSpawn()
    {
        while (m_IsAlive)
        {
            yield return new WaitForSeconds(1f);
            count++;
            if (m_CurrentCreeps < m_MaximumCreeps)
            {
                if (count >= 20 && !m_IsBusy)
                {
                    m_CurrentCreeps += 2;
                    count = 0;
                    m_StateMachine.ChangeState<LeviatanMinionsSpawnState>();
                }
            }
        }

    }
    public void CreepMuerto()
    {
        m_CurrentCreeps--;
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

    private void MatarBoss()
    {
        SpawnEconomy();
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
