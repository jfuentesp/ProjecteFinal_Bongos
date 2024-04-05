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

    // Start is called before the first frame update
    void Start()
    {
        m_StateMachine.ChangeState<SMBChaseState>();
        m_NumberOfAttacksBeforeCharge = Random.Range(1, 6);
    }


    private IEnumerator PlayerDetectionCoroutine()
    {
        while(m_IsAlive)
        {
            RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position, m_AreaRadius, transform.position, m_AreaRadius, m_LayersToCheck);
            if (m_PlayerAttackDetectionAreaType == CollisionType.CIRCLE)
            {
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
        } 
        else
        {
            /*switch(rng)
            {
                case 0.5f && Phase.TWO
            }*/
            if (rng < 0.7f)
            {
                m_NumberOfAttacksBeforeCharge--;
                m_StateMachine.ChangeState<SMBSingleAttackState>();
            }
            else if( rng > 0.8f)
            {
                m_NumberOfAttacksBeforeCharge--;
                m_StateMachine.ChangeState<SMBVoltauroTripleAttackState>();
            }
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
}
