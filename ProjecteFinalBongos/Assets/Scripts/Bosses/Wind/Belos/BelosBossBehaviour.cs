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

    private new void Awake()
    {
        base.Awake();
        m_PlayerDetectionCoroutine = StartCoroutine(PlayerDetectionCoroutine());
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

        m_NumberOfAttacksBeforeTrap = Random.Range(1, 6);
    }

    private void Start()
    {
        m_StateMachine.ChangeState<SMBIdleState>();
    }

    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        OnPlayerInSala.Invoke();
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
        if (m_CurrentPhase == Phase.ONE && m_HealthController.HP <= m_HealthController.HPMAX / 2)
        {
            print("Cambio de fase");
            m_CurrentPhase = Phase.TWO;
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
