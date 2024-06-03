using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBParriedState))]
[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(SMBSingleAttackState))]
[RequireComponent(typeof(SMBDoubleAttackState))]
[RequireComponent(typeof(SMBTripleAttackState))]
[RequireComponent(typeof(SMBBelosHealingState))]
[RequireComponent(typeof(SMBBelosLighningChainsState))]
[RequireComponent(typeof(DeathState))]
public class BelosBossBehaviour : BossBehaviour
{
    private int m_NumberOfAttacksBeforeTrap;
    private Coroutine m_PlayerDetectionCoroutine;
    private enum Phase { ONE, TWO }
    private Phase m_CurrentPhase;
    [SerializeField] private GameObject m_HealingParticles;

    private new void Awake()
    {
        base.Awake();
        m_CurrentPhase = Phase.ONE;
        GetComponent<SMBParriedState>().OnRecomposited = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBSingleAttackState>().OnStopDetectingPlayer = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBSingleAttackState>().OnAttackParried = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
        GetComponent<SMBSingleAttackState>().OnAttackStopped = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBDoubleAttackState>().OnAttackParried = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
        GetComponent<SMBDoubleAttackState>().OnAttackStopped = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBTripleAttackState>().OnAttackParried = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
        GetComponent<SMBTripleAttackState>().OnAttackStopped = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        transform.GetChild(0).GetComponent<BossAttackDamage>().OnAttackParried = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
        GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBBossStunState>().OnStopStun = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBParalized>().OnStopParalized = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        m_NumberOfAttacksBeforeTrap = Random.Range(1, 6);
        m_HealthController.onHurt += CheckPhase;
    }

    private void CheckPhase()
    {
        if (m_CurrentPhase == Phase.ONE && m_HealthController.HP <= m_HealthController.HPMAX / 2)
        {
            print("Cambio de fase");
            m_CurrentPhase = Phase.TWO;
        }
    }
    private void Start()
    {
        m_StateMachine.ChangeState<SMBIdleState>();
    }

    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        m_StateMachine.ChangeState<SMBIdleState>();
        m_PlayerDetectionCoroutine = StartCoroutine(PlayerDetectionCoroutine());
        OnPlayerInSala?.Invoke();
    }

    private void SetAttack()
    {
        if(m_NumberOfAttacksBeforeTrap <= 0 && m_CurrentPhase == Phase.TWO)
        {
            m_NumberOfAttacksBeforeTrap = Random.Range(1, 6);
            m_StateMachine.ChangeState<SMBBelosLighningChainsState>();
            return;
        }

        float rng = Random.value;

        if (rng > 0 && rng < 0.5)
        {
            m_NumberOfAttacksBeforeTrap--;
            m_StateMachine.ChangeState<SMBSingleAttackState>();
        }
        else if (rng >= 0.5 && rng < 0.8)
        {
            m_NumberOfAttacksBeforeTrap--;
            m_StateMachine.ChangeState<SMBDoubleAttackState>();
        }
        else if (rng >= 0.8)
        {
            m_NumberOfAttacksBeforeTrap--;
            m_StateMachine.ChangeState<SMBTripleAttackState>();
        }
    }

    private void SetChase()
    {
        m_StateMachine.ChangeState<SMBChaseState>();
    }

    private void SetPhase(Phase phaseToSet)
    {
        m_CurrentPhase = phaseToSet;
    }

    private IEnumerator PlayerDetectionCoroutine()
    {
        while (m_IsAlive)
        {
            if (m_CurrentPhase == Phase.TWO && m_HealthController.HP <= ((m_HealthController.HPMAX * 10f) / 100)) //Y la vida caiga por debajo del 5%
            {
                m_StateMachine.ChangeState<SMBBelosHealingState>();
            }

            RaycastHit2D hitInfo;
            Vector3 m_PivoteUso;

            if (m_PlayerAttackDetectionAreaType == CollisionType.CIRCLE)
            {
                if (transform.localEulerAngles.y == 180)
                    m_PivoteUso = -m_Pivote;
                else
                    m_PivoteUso = m_Pivote;
                hitInfo = Physics2D.CircleCast(transform.position + m_PivoteUso, m_AreaRadius, transform.position, m_AreaRadius, m_LayersToCheck);
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
                if (transform.localEulerAngles.y == 180)
                    m_PivoteUso = -m_Pivote;
                else
                    m_PivoteUso = m_Pivote;

                hitInfo = Physics2D.BoxCast(transform.position + m_PivoteUso, m_BoxArea, transform.rotation.z, transform.position);
                if (hitInfo.collider.CompareTag("Player") && !m_IsBusy)
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
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        
    }
    private void MatarBoss()
    {
        SpawnEconomy();
        Destroy(gameObject);
    }

    protected override void VidaCero()
    {
        base.VidaCero();
        if (m_IsAlive)
        {
            m_IsAlive = false;
            m_BloodController.PlayDeathBlood();
            StopAllCoroutines();
            m_HealingParticles.SetActive(false);
            GetComponentInParent<SalaBoss>().OnPlayerIn -= Init;
            m_StateMachine.ChangeState<DeathState>();
            OnBossDeath?.Invoke();
            if (m_BossFinalSala)
                m_BossMuertoEvent.Raise();
        }
    }
}
