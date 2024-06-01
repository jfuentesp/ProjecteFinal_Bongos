using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(SMBSingleAttackState))]
[RequireComponent(typeof(SMBParriedState))]
[RequireComponent(typeof(SMBIdleState))]
public class EnemySnake : BossBehaviour
{
    private Transform m_AlteaTransform;
    private new void Awake()
    {
        base.Awake();

        GetComponent<SMBSingleAttackState>().OnStopDetectingPlayer = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        }; 
        GetComponent<SMBSingleAttackState>().OnAttackStopped = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBParriedState>().OnRecomposited = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        }; 
        transform.GetChild(0).GetComponent<BossAttackDamage>().OnAttackParried = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
        GetComponent<SMBBossStunState>().OnStopStun = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBParalized>().OnStopParalized = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
    }
    private void Start()
    {
        m_StateMachine.ChangeState<SMBIdleState>();
    }
    public void SetAlteaTransform(Transform _AlteaTransform)
    {
        m_AlteaTransform = _AlteaTransform;
        if (m_AlteaTransform.TryGetComponent<AlteaBossBehaviour>(out AlteaBossBehaviour altea))
            altea.OnBossDeath += VidaCero;
    }
    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        OnPlayerInSala?.Invoke();
        StartCoroutine(PlayerDetectionCoroutine());
    }

    private IEnumerator PlayerDetectionCoroutine()
    {
        while (m_IsAlive)
        {

            if (m_PlayerAttackDetectionAreaType == CollisionType.CIRCLE)
            {
                RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position + m_Pivote, m_AreaRadius, transform.position, m_AreaRadius, m_LayersToCheck);
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("Player") && !m_IsBusy)
                {
                    m_IsPlayerDetected = true;
                    //print("eo");
                    m_StateMachine.ChangeState<SMBSingleAttackState>();
                }
                else
                {
                    m_IsPlayerDetected = false;
                }
            }
            else
            {
                RaycastHit2D hitInfo = Physics2D.BoxCast(transform.position + m_Pivote, m_BoxArea, transform.rotation.z, transform.position);
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
    private void MatarBoss() 
    {
        if (m_AlteaTransform != null)
        {
            if (m_AlteaTransform.TryGetComponent<AlteaBossBehaviour>(out AlteaBossBehaviour altea))
                altea.OnBossDeath -= VidaCero;
        }
        Destroy(gameObject);
    }
    protected override void VidaCero()
    {
        base.VidaCero();
        m_BloodController.PlayDeathBlood();
        StopAllCoroutines();
        GetComponentInParent<SalaBoss>().OnPlayerIn -= Init;
        m_StateMachine.ChangeState<DeathState>();
        OnBossDeath?.Invoke();
        if(m_AlteaTransform != null)
        {
            if (m_AlteaTransform.TryGetComponent<AlteaBossBehaviour>(out AlteaBossBehaviour altea))
                altea.SnakeMuerta();
        }
        if (m_BossFinalSala)
            m_BossMuertoEvent.Raise();
        m_IsAlive = false;
    }
}
