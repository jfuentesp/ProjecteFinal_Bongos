using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBHarpyChaseState))]
[RequireComponent(typeof(SMBSingleAttackState))]
[RequireComponent(typeof(DeathState))]
[RequireComponent(typeof(SMBParriedState))]
public class HarpyBehaviour : BossBehaviour
{
    [SerializeField]
    private GameEvent m_OnDeathEvent;
    private Transform m_DamaTransform;

    [Header("Variables Harpias")]
    [SerializeField] private int m_MaximumHarpies;
    [SerializeField] private int m_CurrentHarpies;
    protected new void Awake()
    {
        base.Awake();
        m_IsBusy = false;
        GetComponent<SMBSingleAttackState>().OnStopDetectingPlayer = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBHarpyChaseState>();
        };
        GetComponent<SMBSingleAttackState>().OnAttackStopped = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBHarpyChaseState>();
        };
        GetComponent<SMBSingleAttackState>().OnAttackParried = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
        GetComponent<SMBParriedState>().OnRecomposited = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBHarpyChaseState>();
        };
        GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBHarpyChaseState>();
        };
        transform.GetChild(0).GetComponent<BossAttackDamage>().OnAttackParried = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
        GetComponent<SMBBossStunState>().OnStopStun = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBHarpyChaseState>();
        };
        GetComponent<SMBParalized>().OnStopParalized = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBHarpyChaseState>();
        };

        GetComponent<SMBIdleState>().OnPlayerEnter += EmpezarCorutina;
    }
    private void Start()
    {
        m_StateMachine.ChangeState<SMBIdleState>();
    }

    public void SetDamaTransform(Transform _AlteaTransform)
    {
        m_DamaTransform = _AlteaTransform;
        if (m_DamaTransform.TryGetComponent<DamaBossBehaviour>(out DamaBossBehaviour altea))
            altea.OnBossDeath += VidaCero;
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
                RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position + m_Pivote, m_AreaRadius, transform.position, m_AreaRadius, m_LayersToCheck);             
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
                RaycastHit2D hitInfo = Physics2D.BoxCast(transform.position + m_Pivote, m_BoxArea, transform.rotation.z, transform.position);
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
    private void MatarBoss()
    {
        if (m_DamaTransform != null)
        {
            if (m_DamaTransform.TryGetComponent<AlteaBossBehaviour>(out AlteaBossBehaviour altea))
                altea.OnBossDeath -= VidaCero;
        }
        Destroy(gameObject);
    }
    protected override void VidaCero()
    {
        base.VidaCero();
        if (m_IsAlive)
        {
            m_IsAlive = false;
            m_BloodController.PlayDeathBlood();
            StopAllCoroutines();
            GetComponentInParent<SalaBoss>().OnPlayerIn -= Init;
            m_StateMachine.ChangeState<DeathState>();
            OnBossDeath?.Invoke();
            m_OnDeathEvent.Raise();
            if (m_BossFinalSala)
                m_BossMuertoEvent.Raise();
        }
    }
    private void Attack()
    {
        m_StateMachine.ChangeState<SMBSingleAttackState>();
    }

}
