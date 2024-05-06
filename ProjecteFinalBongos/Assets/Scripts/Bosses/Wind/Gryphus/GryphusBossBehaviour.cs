using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBParriedState))]
[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(SMBSingleAttackState))]
[RequireComponent(typeof(SMBDoubleAttackState))]
[RequireComponent(typeof(SMBTripleAttackState))]
public class GryphusBossBehaviour : BossBehaviour
{
    private Coroutine m_PlayerDetectionCoroutine;
    private enum Phase { ONE, TWO }
    private Phase m_CurrentPhase;

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
        m_CurrentPhase = Phase.ONE;
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

        if(collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
        {
            if (m_CurrentPhase == Phase.ONE)
                m_HealthController.Heal(20);
        }
        if (m_CurrentPhase == Phase.ONE && m_HealthController.HP <= m_HealthController.HPMAX / 2)
        {
            print("Cambio de fase");
            m_CurrentPhase = Phase.TWO;
        }
    }

    private void SetAttack()
    {
        float rng = Random.value;
        /*if (si la vida cae al 50%)
        {
            m_CurrentPhase = Phase.TWO;
            return;
        }*/
        switch (rng)
        {
            case < 0.5f:
                m_StateMachine.ChangeState<SMBSingleAttackState>();
                break;
            case < 0.65f:
                m_StateMachine.ChangeState<SMBDoubleAttackState>();
                break;
            case > 0.8f:
                m_StateMachine.ChangeState<SMBTripleAttackState>();
                break;
        }
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
    protected override void VidaCero()
    {
        base.VidaCero();
        m_IsAlive = false;
        OnBossDeath?.Invoke();
        m_BossMuertoEvent.Raise();
        Destroy(gameObject);
    }
}
