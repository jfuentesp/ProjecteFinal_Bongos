using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Android;
using static UnityEngine.GraphicsBuffer;
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

    protected HealthController m_HealthController;
    protected FiniteStateMachine m_StateMachine;
    protected Rigidbody2D m_Rigidbody;
    protected Animator m_Animator;

    protected SalaBoss m_SalaPadre;
    public SalaBoss SalaPadre => m_SalaPadre;
    protected bool m_IsBusy;
    protected bool m_IsAlive;
    public bool IsAlive => m_IsAlive;
    protected bool m_IsPlayerDetected;
    public bool IsPlayerDetected => m_IsPlayerDetected;

    public Action OnPlayerInSala;

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

    //en el meu estat--
    public delegate void OnPlayerEnter(GameObject obj);
    private OnPlayerEnter onPlayerEnter;

    protected NavMeshAgent m_NavMeshAgent;

    protected void Awake()
    {
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_HealthController = GetComponent<HealthController>();
        m_HealthController.onDeath += VidaCero;
        if(m_PlayerAttackDetectionAreaType == CollisionType.BOX)
            m_BoxArea = new Vector2(m_AreaWideness, m_AreaLength);
        m_IsBusy = false;
        m_IsAlive = true;
        m_IsPlayerDetected = false;
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
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
            m_HealthController.Damage(collision.gameObject.GetComponent<AttackDamage>().Damage);
           
        }
    }

    public virtual void Init(Transform _Target)
    {
        m_Target = _Target;
    }

    public void SetBusy(bool status)
    {
        m_IsBusy = status;
    }

    public void CurarBoss(float _Heal)
    {
        m_HealthController.Heal(_Heal);
    }
}
