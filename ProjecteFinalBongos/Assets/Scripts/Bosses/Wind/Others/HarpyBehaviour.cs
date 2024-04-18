using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBHarpyChaseState))]
[RequireComponent(typeof(SMBSingleAttackState))]
public class HarpyBehaviour : BossBehaviour
{
    protected new void Awake()
    {
        base.Awake();
        m_IsBusy = false;
        GetComponent<SMBSingleAttackState>().OnStopDetectingPlayer = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBHarpyChaseState>();
        };
        GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBHarpyChaseState>();
        };

        GetComponent<SMBIdleState>().OnPlayerEnter += EmpezarCorutina;
        m_StateMachine.ChangeState<SMBIdleState>();

    }

    private void EmpezarCorutina(GameObject @object)
    {
        m_PlayerDetectionCoroutine = StartCoroutine(PlayerDetectionCoroutine());
    }

    public override void Init(Transform target)
    {
        m_Target = target;
        OnPlayerInSala.Invoke();
    }

    private Coroutine m_PlayerDetectionCoroutine;
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
                    Attack();
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
                    Attack();
                }
                else
                {
                    m_IsPlayerDetected = false;
                }
            }
            yield return new WaitForSeconds(m_CheckingPlayerTimelapse);
        }
    }
    protected override void VidaCero()
    {
        base.VidaCero();
        m_IsAlive = false;
        Destroy(gameObject);
    }
    private void Attack()
    {
        m_StateMachine.ChangeState<SMBSingleAttackState>();
    }

}