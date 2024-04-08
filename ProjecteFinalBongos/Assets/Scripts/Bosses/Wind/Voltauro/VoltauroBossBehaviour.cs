using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBParriedState))]
[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(SMBSingleAttackState))]
[RequireComponent(typeof(SMBVoltauroTripleAttackState))]
[RequireComponent(typeof(SMBChargeState))]
[RequireComponent(typeof(SMBLightningSummonState))]
public class VoltauroBossBehaviour : BossBehaviour
{
    private int m_NumberOfAttacksBeforeCharge;
    private Coroutine m_PlayerDetectionCoroutine;
    private enum Phase { ONE, TWO }
    private Phase m_CurrentPhase;

    private new void Awake()
    {
        base.Awake();
        m_PlayerDetectionCoroutine = StartCoroutine(PlayerDetectionCoroutine());
        m_CurrentPhase = Phase.ONE;
    }

    void Start()
    {
        m_StateMachine.ChangeState<SMBChaseState>();
        m_NumberOfAttacksBeforeCharge = Random.Range(1, 6);
        GetComponent<SMBChargeState>().OnChargeMissed = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
    }

    private IEnumerator PlayerDetectionCoroutine()
    {
        while(m_IsAlive)
        {

            if (m_PlayerAttackDetectionAreaType == CollisionType.CIRCLE)
            {
                RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position, m_AreaRadius, transform.position, m_AreaRadius, m_LayersToCheck);
                Debug.Log("Entro 1");
                if(hitInfo.collider.CompareTag("Player") && !m_IsBusy)
                {
                    Debug.Log("Entro 2");
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

    private void SetAttack()
    {
        float rng = Random.value;
        if(m_NumberOfAttacksBeforeCharge <= 0)
        {
            SetCharge();
            return;
        } 

        switch(rng)
        {
            case < 0.5f:
                m_NumberOfAttacksBeforeCharge--;
                m_StateMachine.ChangeState<SMBSingleAttackState>();
                break;
            case > 0.51f:
                m_NumberOfAttacksBeforeCharge--;
                if (m_CurrentPhase == Phase.TWO && rng > 0.8f)
                {
                    m_StateMachine.ChangeState<SMBLightningSummonState>();
                }
                else
                {
                    m_StateMachine.ChangeState<SMBVoltauroTripleAttackState>();
                }
                break;
        }
    }

    private void SetChase()
    {
        m_StateMachine.ChangeState<SMBChaseState>();
    }

    private void SetCharge()
    {
        m_NumberOfAttacksBeforeCharge = Random.Range(1, 6);
        m_StateMachine.ChangeState<SMBChargeState>();
    }

    private void SetLightningSummon()
    {
        m_StateMachine.ChangeState<SMBLightningSummonState>();
    }  

    private void SetPhase(Phase phaseToSet)
    {
        m_CurrentPhase = phaseToSet;
    }
}
