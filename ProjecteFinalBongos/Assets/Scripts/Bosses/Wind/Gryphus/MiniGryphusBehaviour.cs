using NavMeshPlus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentOverride2d))]
[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBParriedState))]
[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(SMBSingleAttackState))]
public class MiniGryphusBehaviour : BossBehaviour
{
    private Coroutine m_PlayerDetectionCoroutine;

    private new void Awake()
    {
        base.Awake();
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
        GetComponent<SMBIdleState>().OnPlayerEnter += EmpezarCorutina;
    }
    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        OnPlayerInSala.Invoke();
    }
    private void EmpezarCorutina(GameObject obj)
    {
        m_PlayerDetectionCoroutine = StartCoroutine(PlayerDetectionCoroutine());
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
              m_HealthController.Heal(20);
  
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
                    m_StateMachine.ChangeState<SMBSingleAttackState>();
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
                    m_StateMachine.ChangeState<SMBSingleAttackState>();
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
        OnBossDeath?.Invoke();
        m_BossMuertoEvent.Raise();
        Destroy(gameObject);
    }
}
