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
    }

    private void Start()
    {
        m_StateMachine.ChangeState<SMBChaseState>();
        m_NumberOfAttacksBeforeTrap = Random.Range(1, 6);
        GetComponent<SMBParriedState>().OnRecomposited = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
    }

    private void SetAttack()
    {
        if(m_NumberOfAttacksBeforeTrap <= 0 && m_CurrentPhase == Phase.TWO)
        {
            Debug.Log("Entro en trampa.");
            m_NumberOfAttacksBeforeTrap = Random.Range(1, 6);
            m_StateMachine.ChangeState<SMBBelosLighningChainsState>();
            return;
        }

        float rng = Random.value;
        if(m_CurrentPhase == Phase.TWO && m_MaxHP > 5) //Y la vida caiga por debajo del 5%
        {
            m_StateMachine.ChangeState<SMBBelosHealingState>();
        }

        Debug.Log("Ataques restantes: " + m_NumberOfAttacksBeforeTrap);
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


}
