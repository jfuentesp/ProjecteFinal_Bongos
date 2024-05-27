using NavMeshPlus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentOverride2d))]
[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(SMBRunAwayState))]
[RequireComponent(typeof(HealthController))]
[RequireComponent(typeof(DracMariRangedAttackState))]
public class MiniDracMariBehaviour : BossBehaviour
{
    [SerializeField] private int m_RangoHuirPerseguir;
    private Coroutine m_PlayerDetectionCoroutine;
    private enum Phase { ONE, TWO }
    private Phase m_CurrentPhase;

    private int CorazaCount = 5;
    [SerializeField] private BubbleProtectionScript m_Bubble;
    private new void Awake()
    {
        base.Awake();
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
     
        GetComponent<SMBIdleState>().OnPlayerEnter += EmpezarCorutina;
    }

    private void Start()
    {
        m_StateMachine.ChangeState<SMBIdleState>();
        m_Bubble.Init(CorazaCount);
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
        m_StateMachine.ChangeState<DracMariRangedAttackState>();
    }

    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        OnPlayerInSala?.Invoke();
    }
    private void MatarBoss()
    {
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
        if (m_BossFinalSala)
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
                m_Bubble.SetVida(CorazaCount);
                if (CorazaCount <= 0)
                {
                    EstadosController.Invencible = false;
                    m_CurrentPhase = Phase.TWO;
                }
            }
            else
            {
                recibirDaño(collision.gameObject.GetComponent<BossAttackDamage>().Damage);
            }
        }

    }
}
