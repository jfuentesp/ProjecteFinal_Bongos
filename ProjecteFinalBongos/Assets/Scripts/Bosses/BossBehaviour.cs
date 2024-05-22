using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Android;
using static UnityEngine.GraphicsBuffer;
[RequireComponent(typeof(SMBParalized))]
[RequireComponent(typeof(SMBBossStunState))]
[RequireComponent(typeof(BossEstadosController))]
[RequireComponent(typeof(BossStatsController))]
[RequireComponent(typeof(FiniteStateMachine))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class BossBehaviour : MonoBehaviour
{
    [Header("Boss parameters")]
    [SerializeField]
    protected string m_BossName;
    [SerializeField]
    protected string m_Description;
    [SerializeField]
    protected Transform m_Target;
    public Transform Target => m_Target;

    [SerializeField]
    protected BloodController m_BloodController;

    protected HealthController m_HealthController;
    protected FiniteStateMachine m_StateMachine;
    public FiniteStateMachine StateMachine => m_StateMachine;
    protected Rigidbody2D m_Rigidbody;
    protected Animator m_Animator;
    private BossStatsController m_Stats;
    private BossEstadosController m_EstadosController;
    public BossEstadosController EstadosController => m_EstadosController;

    protected SalaBoss m_SalaPadre;
    public SalaBoss SalaPadre => m_SalaPadre;
    protected bool m_IsBusy;
    protected bool m_IsAlive;
    public bool IsAlive => m_IsAlive;
    protected bool m_IsPlayerDetected;
    public bool IsPlayerDetected => m_IsPlayerDetected;

    public Action OnPlayerInSala;

    public Action OnBossDeath;

    [SerializeField] protected bool m_BossFinalSala;

    protected enum CollisionType { CIRCLE, BOX }

    [Header("Attack detection area settings (CircleCast collider)")]
    [Tooltip("The range within the enemy will trigger main attacks")]
    [SerializeField]
    protected CollisionType m_PlayerAttackDetectionAreaType;
    [Header("If the area is checking as a rectangle collider")]
    [SerializeField]
    protected float m_AreaLength;
    [SerializeField]
    protected float m_AreaWideness;
    protected Vector2 m_BoxArea; 
    [Header("If the area is checking as a circle collider")]
    [SerializeField]
    protected float m_AreaRadius;

    [Header("Player check timelapse")]
    [SerializeField]
    protected float m_CheckingPlayerTimelapse;

    [Header("Layers to detect")]
    [SerializeField]
    protected LayerMask m_LayersToCheck;

    [Header("Event when boss dies")]
    [SerializeField] protected GameEvent m_BossMuertoEvent;

    //en el meu estat--
    public delegate void OnPlayerEnter(GameObject obj);
    private OnPlayerEnter onPlayerEnter;

    protected NavMeshAgent m_NavMeshAgent;
    public NavMeshAgent NavMeshAgent => m_NavMeshAgent;

    [SerializeField] private LayerMask m_layerMask;

    protected virtual void Awake()
    {
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_HealthController = GetComponent<HealthController>();
        m_Stats = GetComponent<BossStatsController>();
        m_EstadosController = GetComponent<BossEstadosController>();
        m_HealthController.onDeath += VidaCero;
        if(m_PlayerAttackDetectionAreaType == CollisionType.BOX)
            m_BoxArea = new Vector2(m_AreaWideness, m_AreaLength);
        m_IsBusy = false;
        m_IsAlive = true;
        m_IsPlayerDetected = false;
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        GetComponentInParent<SalaBoss>().OnPlayerIn += Init;

        if (!m_NavMeshAgent.isOnNavMesh)
        {
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                transform.position = hit.position;

                m_NavMeshAgent.Warp(hit.position);
            }
            else
            {
                Debug.Log("Could not find position on NavMesh close to the agent.");
                return;
            }
        }
        print("Awake");
        /* GetComponent<SMBPatrol>().OnPlayerEnter = (GameObject obj) =>
         {
             m_StateMachine.ChangeState<SMBAttack>();
         };*/
    }
    protected virtual void Update()
    {
        transform.localEulerAngles = new Vector3(0,0, transform.localEulerAngles.z);
        
    }
    private void FixedUpdate()
    {
        
    }
    protected virtual void VidaCero()
    {
        
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        m_Rigidbody.velocity = Vector2.zero;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHitBox"))
        {
            if (collision.gameObject.GetComponent<AttackDamage>()) {
                recibirDaño(collision.gameObject.GetComponent<AttackDamage>().Damage);
            } else if (collision.gameObject.GetComponent<Player2x2BulletBehaviour>()) {
                recibirDaño(collision.gameObject.GetComponent<Player2x2BulletBehaviour>().damage);
            }
        }
    }

    public virtual void Init(Transform _Target)
    {
        m_Target = _Target;
        OnPlayerInSala?.Invoke();
    }

    public void SetBusy(bool status)
    {
        m_IsBusy = status;
    }

    public void CurarBoss(float _Heal)
    {
        m_HealthController.Heal(_Heal);
    }
    public void recibirDaño(float Daño)
    {
        if (m_IsAlive)
        {
            if (EstadosController.Invencible)
                return;
            if (m_EstadosController.Burn)
            {
                m_HealthController.Damage(Daño);
                m_EstadosController.burntDamage = (Daño * m_Stats.getModifier("Burnt")) / 100;
                m_HealthController.Damage(m_EstadosController.burntDamage);
            }
            if (m_EstadosController.Wrath && m_EstadosController.Paralized)
            {
                Daño += Daño * m_Stats.getModifier("Paralized");
                m_HealthController.Damage(Daño);
            }
            else if (m_EstadosController.Wrath)
            {
                Daño += Daño * m_Stats.getModifier("WrathLife");
                m_HealthController.Damage(Daño);
            }
            else if (m_EstadosController.Paralized)
            {
                Daño += Daño * m_Stats.getModifier("Paralized");
                m_HealthController.Damage(Daño);
            }
            else
            {
                m_HealthController.Damage(Daño);
            }
            m_BloodController.PlayBlood();
        }
    }
    
    public  void ReTarget()
    {
        StartCoroutine(ReTargetCoroutine());
    }

    private IEnumerator ReTargetCoroutine()
    {
        yield return new WaitForSeconds(.1f);
        if (!typeof(SMBIdleState).IsInstanceOfType(m_StateMachine.CurrentState))
        {
            RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position, 50, transform.position, 50, m_LayersToCheck);
            if (hitInfo.collider != null && hitInfo.collider.CompareTag("Player"))
            {
                print("Hola");
                m_Target = hitInfo.collider.gameObject.transform;
                OnPlayerInSala?.Invoke();
            }
        }
    }

    internal void BossFinalSalaSpawn(Transform Target)
    {
        StartCoroutine(SpawnFinalBoss(Target));
    }

    private IEnumerator SpawnFinalBoss(Transform Target)
    {
        yield return new WaitForSeconds(.1f);
        if (!m_NavMeshAgent.isOnNavMesh)
        {
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                transform.position = hit.position;

                m_NavMeshAgent.Warp(hit.position);
            }
            else
            {
                Debug.Log("Could not find position on NavMesh close to the agent.");
            }
        }
        Init(Target);
    }
}
