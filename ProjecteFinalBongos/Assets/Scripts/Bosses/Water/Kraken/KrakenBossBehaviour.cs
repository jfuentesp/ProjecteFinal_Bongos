using NavMeshPlus.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentOverride2d))]
[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(KrakenRangedAttackState))]
[RequireComponent(typeof(KrakenSpinState))]
[RequireComponent(typeof(KrakenParalizingAttack))]
[RequireComponent(typeof(KrakenSetSubMergeState))]
[RequireComponent(typeof(KrakenMergeState))]
[RequireComponent(typeof(SubMergeState))]
[RequireComponent(typeof(HealthController))]
public class KrakenBossBehaviour : BossBehaviour
{
    private Coroutine m_PlayerDetectionCoroutine;
    [SerializeField] private GameObject m_tentacle;
    [SerializeField] private int paralizingTenacleCount = 0;
    [SerializeField] private LayerMask m_TentaculosMask;
    private int numberofAttacksBeforeSubmerging;

    private new void Awake()
    {
        base.Awake();
        m_SalaPadre = GetComponentInParent<SalaBoss>();
        GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBParriedState>().OnRecomposited = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<KrakenParalizingAttack>().onAttackStopped = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<KrakenParalizingAttack>().onAttackDestroyed = (GameObject obj) =>
        {

            m_StateMachine.ChangeState<SMBParriedState>();
        };
        GetComponent<KrakenRangedAttackState>().onAttackStopped = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<KrakenMergeState>().OnMergeFinish = (GameObject obj) =>
        {

            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBParalized>().OnStopParalized = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBBossStunState>().OnStopStun = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
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
        transform.GetChild(4).GetComponent<BossAttackDamage>().OnAttackParried = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
        transform.GetChild(5).GetComponent<BossAttackDamage>().OnAttackParried = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
        transform.GetChild(6).GetComponent<BossAttackDamage>().OnAttackParried = (GameObject obj) =>
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
        GetComponent<SMBIdleState>().OnPlayerEnter += EmpezarCorutina;
    }

    private void Start()
    {

        m_StateMachine.ChangeState<SMBIdleState>();
    }
    private void EmpezarCorutina(GameObject obj)
    {
        m_PlayerDetectionCoroutine = StartCoroutine(PlayerDetectionCoroutine());
        StartCoroutine(SpawnTentacles());
        SetNmberOfAttacks();

    }

    private void SetNmberOfAttacks()
    {
        numberofAttacksBeforeSubmerging = Random.Range(2, 7);
    }
    private void SetSubmerge()
    {
        SetNmberOfAttacks();
        m_StateMachine.ChangeState<KrakenSetSubMergeState>();
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
                    SetAttack();
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
                    SetAttack();
                }
                else
                {
                    m_IsPlayerDetected = false;
                }
            }
            yield return new WaitForSeconds(m_CheckingPlayerTimelapse);
        }
    }

    private IEnumerator SpawnTentacles()
    {
        while (m_IsAlive)
        {
            yield return new WaitForSeconds(1f);
            PonerTentaculo();
        }

    }
    private void PonerTentaculo()
    {
        Vector2 posicionTentacle = m_SalaPadre.GetPosicionAleatoriaEnSala();
        RaycastHit2D hit = Physics2D.CircleCast(posicionTentacle, 1, posicionTentacle, 1, m_TentaculosMask);
        if (hit.collider != null)
        {
            PonerTentaculo();

        }
        else
        {
            GameObject tentacle = Instantiate(m_tentacle, transform.parent);
            tentacle.transform.position = new Vector2(posicionTentacle.x, posicionTentacle.y);
        }
    }
    private void SetAttack()
    {
        float rng = Random.value;
        print(numberofAttacksBeforeSubmerging);
        if (numberofAttacksBeforeSubmerging <= 0)
        {

            SetSubmerge();
        }
        else
        {
            switch (rng)
            {
                case < 0.5f:
                    numberofAttacksBeforeSubmerging--;
                    m_StateMachine.ChangeState<SMBSingleAttackState>();
                    break;
                case < 0.65f:
                    numberofAttacksBeforeSubmerging--;
                    m_StateMachine.ChangeState<KrakenRangedAttackState>();
                    break;
                case > 0.8f:
                    numberofAttacksBeforeSubmerging--;
                    m_StateMachine.ChangeState<KrakenParalizingAttack>();
                    break;

            }
        }

    }

    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        OnPlayerInSala?.Invoke();
    }
    protected override void VidaCero()
    {
        base.VidaCero();
        StopAllCoroutines();
        m_IsAlive = false;
        OnBossDeath?.Invoke();
        if (m_BossFinalSala)
            m_BossMuertoEvent.Raise();
        Destroy(gameObject);
    }
}
