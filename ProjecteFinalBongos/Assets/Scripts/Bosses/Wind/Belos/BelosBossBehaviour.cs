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
        GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        m_StateMachine.ChangeState<SMBIdleState>();

        m_NumberOfAttacksBeforeTrap = Random.Range(1, 6);
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
        if(m_CurrentPhase == Phase.TWO) //Y la vida caiga por debajo del 5%
        {
            m_StateMachine.ChangeState<SMBBelosHealingState>();
            return;
        }

        switch (rng)
        {
            case < 0.5f:
                m_NumberOfAttacksBeforeTrap--;
                m_StateMachine.ChangeState<SMBSingleAttackState>();
                break;
            case < 0.65f:
                m_NumberOfAttacksBeforeTrap--;
                m_StateMachine.ChangeState<SMBDoubleAttackState>();
                break;
            case > 0.8f:
                m_NumberOfAttacksBeforeTrap--;
                m_StateMachine.ChangeState<SMBTripleAttackState>();           
                break;
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
    protected override void VidaCero()
    {
        base.VidaCero();
        m_IsAlive = false;
        OnBossDeath?.Invoke();
        m_BossMuertoEvent.Raise();
        Destroy(gameObject);
    }
}
