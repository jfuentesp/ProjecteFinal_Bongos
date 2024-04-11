using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(SMBFlyingState))]
[RequireComponent(typeof(SMBSetFlyingState))]
public class DamaBossBehaviour : BossBehaviour
{
    private Coroutine m_PlayerDetectionCoroutine;
    private enum Phase { ONE, TWO }
    private Phase m_CurrentPhase;

    private bool m_IsFlying;

    private new void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetFlying();
    }

    public void SetFlying()
    {
        m_IsFlying = true;
        m_StateMachine.ChangeState<SMBSetFlyingState>();
    }

    public void SetFlyingShadow()
    {
        m_StateMachine.ChangeState<SMBFlyingState>();
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
        float rng = Random.value;
        if (m_CurrentPhase == Phase.TWO) //Y la vida caiga por debajo del 5%
        {
            m_StateMachine.ChangeState<SMBBelosHealingState>();
        }

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
}
