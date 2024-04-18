using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(SMBFlyingState))]
[RequireComponent(typeof(SMBSetFlyingState))]
[RequireComponent(typeof(SMBLandingState))]
[RequireComponent(typeof(SMBSingleAttackState))]
[RequireComponent(typeof(SMBDoubleAttackState))]
[RequireComponent(typeof(SMBParriedState))]
[RequireComponent(typeof(HealthController))]
public class DamaBossBehaviour : BossBehaviour
{
    private int m_NumberOfAttacksBeforeFlying;
    private Coroutine m_PlayerDetectionCoroutine;
    private enum Phase { ONE, TWO }
    private Phase m_CurrentPhase;

    private bool m_IsFlying;

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
        GetComponent<SMBIdleState>().OnPlayerEnter += ActivateBoss;
        m_StateMachine.ChangeState<SMBIdleState>();
    }
    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        OnPlayerInSala.Invoke();
    }
    private void ActivateBoss(GameObject @object)
    {
        SetFlying();
    }

    public void SetFlying()
    {
        m_IsFlying = true;
        m_NumberOfAttacksBeforeFlying = Random.Range(1, 6);
        m_Rigidbody.velocity = Vector3.zero;
        transform.up = Vector3.zero;
        if (m_PlayerDetectionCoroutine != null)
            StopCoroutine(m_PlayerDetectionCoroutine);
        m_StateMachine.ChangeState<SMBSetFlyingState>();
    }

    public void SetFlyingShadow()
    {
        m_StateMachine.ChangeState<SMBFlyingState>();
    }

    public void SetChase()
    {
        m_IsFlying = false;
        m_PlayerDetectionCoroutine = StartCoroutine(PlayerDetectionCoroutine());
        m_StateMachine.ChangeState<SMBChaseState>();
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

    private void SetAttack()
    {
        Debug.Log("Entro en el ataque. Numero de ataques restante: " + m_NumberOfAttacksBeforeFlying);
        float rng = Random.value;
        if (m_NumberOfAttacksBeforeFlying <= 0)
        {
            SetFlying();
            return;
        }

        switch (rng)
        {
            case < 0.5f:
                m_NumberOfAttacksBeforeFlying--;
                m_StateMachine.ChangeState<SMBSingleAttackState>();
                break;
            case < 0.51f:
                m_NumberOfAttacksBeforeFlying--;
                m_StateMachine.ChangeState<SMBDoubleAttackState>();
                break;
        }
    }
    protected override void VidaCero()
    {
        base.VidaCero();
        m_IsAlive = false;
        OnBossDeath?.Invoke();
        Destroy(gameObject);
    }
}
