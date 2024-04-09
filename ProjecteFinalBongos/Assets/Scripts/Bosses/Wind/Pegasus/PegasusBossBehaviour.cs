using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBParriedState))]
[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(SMBSingleAttackState))]
[RequireComponent(typeof(SMBChargeState))]

public class PegasusBossBehaviour : BossBehaviour
{
    private Coroutine m_PlayerDetectionCoroutine;
    private int m_NumberOfAttacksBeforeCharge;
    private new void Awake()
    {
        base.Awake();
        m_PlayerDetectionCoroutine = StartCoroutine(PlayerDetectionCoroutine());
    }
    void Start()
    {
        m_StateMachine.ChangeState<SMBChaseState>();
        m_NumberOfAttacksBeforeCharge = Random.Range(5, 7);
        GetComponent<SMBChargeState>().OnChargeMissed = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
        GetComponent<SMBParriedState>().OnRecomposited = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBGroundHitState>();
        };
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
    private void SetAttack()
    {
       
        if (m_NumberOfAttacksBeforeCharge <= 0)
        {
            WalkAround();
            return;
        }
        m_NumberOfAttacksBeforeCharge--;
        m_StateMachine.ChangeState<SMBChargeState>();

       
    }
    private void WalkAround()
    {
        m_NumberOfAttacksBeforeCharge = Random.Range(5, 7);
        m_StateMachine.ChangeState<SMBWalkAroundState>();
    }
}
