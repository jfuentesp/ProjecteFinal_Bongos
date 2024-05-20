using NavMeshPlus.Extensions;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentOverride2d))]
[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(SMBRunAwayState))]
[RequireComponent(typeof(HealthController))]
[RequireComponent(typeof(DracMariRangedAttackState))]
[RequireComponent(typeof(DracMariMeteorShowerState))]
[RequireComponent(typeof(DracMariTornadoState))]


public class DracMariBossBehaviour : BossBehaviour
{
    [SerializeField] private int m_RangoHuirPerseguir;
    private Coroutine m_PlayerDetectionCoroutine;
    private enum Phase { ONE, TWO }
    private Phase m_CurrentPhase;

    private int CorazaCount = 5;
    private new void Awake()
    {
        base.Awake();
        m_Animator.Play("idleDracMari");
        m_CurrentPhase = Phase.ONE;
        EstadosController.Invencible = true;
        m_SalaPadre = GetComponentInParent<SalaBoss>();
        GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
        {
            
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBRunAwayState>().onStoppedRunningAway = (GameObject obj) =>
        {
            SetAttack();
        };
        GetComponent<DracMariMeteorShowerState>().OnShowerFinished = (GameObject obj) =>
        {
           
            SetAttack();
        };
        GetComponent<DracMariTornadoState>().OnTornadoFinished = (GameObject obj) =>
        {
            SetAttack();
        };
        GetComponent<DracMariRangedAttackState>().onAttackStopped = (GameObject obj) =>
        {
            PerseguirHuir();
        };
        GetComponent<SMBParalized>().OnStopParalized = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBBossStunState>().OnStopStun = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        m_StateMachine.ChangeState<SMBIdleState>();
        GetComponent<SMBIdleState>().OnPlayerEnter += EmpezarCorutina;
    }
    private void EmpezarCorutina(GameObject obj)
    {
        m_PlayerDetectionCoroutine = StartCoroutine(PlayerDetectionCoroutine());
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
    private void PerseguirHuir()
    {
        if (Vector2.Distance(transform.position, m_Target.position) > m_RangoHuirPerseguir)
            m_StateMachine.ChangeState<SMBChaseState>();
        else
            m_StateMachine.ChangeState<SMBRunAwayState>();
    }
    private void SetAttack()    
    {
        if (m_CurrentPhase == Phase.ONE)
        {
            m_StateMachine.ChangeState<DracMariRangedAttackState>();
        }
        else {
            float rng = Random.value;

            switch (rng)
            {
                case < 0.7f:
                    m_StateMachine.ChangeState<DracMariRangedAttackState>();
                    break;
                case < 0.8f:
                    m_StateMachine.ChangeState<DracMariMeteorShowerState>();
                    break;
                case > 0.95f:
                    m_StateMachine.ChangeState<DracMariTornadoState>();
                    break;
            }
        }
    }

    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        OnPlayerInSala?.Invoke();
    }
    private void MatarBoss() { 
        Destroy(gameObject);
    }
    protected override void VidaCero()
    {
        base.VidaCero();
        StopAllCoroutines();
        GetComponentInParent<SalaBoss>().OnPlayerIn -= Init;
        m_StateMachine.ChangeState<DeathState>();
        m_IsAlive = false;
        OnBossDeath?.Invoke();
        m_BossMuertoEvent.Raise();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)  
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("DracPlayerBullet"))
        {
            if (CorazaCount > 0)
            {
                CorazaCount--;
                if (CorazaCount <= 0) {
                    EstadosController.Invencible = false;
                    m_CurrentPhase = Phase.TWO;
                }
            }
            else
            {
                recibirDaño(collision.gameObject.GetComponent<DracMBullet>().Damage);
            }
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DracPlayerBullet"))
        {
        }
    }
}
