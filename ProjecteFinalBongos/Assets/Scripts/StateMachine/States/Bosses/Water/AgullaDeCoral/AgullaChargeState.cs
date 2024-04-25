using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgullaChargeState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;
    private NavMeshAgent m_NavMeshAgent;

    [Header("AnimationName")]
    [SerializeField] private string m_AnimationName;

    [Header("Charge speed")]
    [SerializeField]
    private float m_ChargeSpeed;


    private bool m_IsAiming;
    private bool m_IsCharging;

    private Vector2 m_PositionToCharge;

    private Transform m_Target;

    public Action<GameObject> OnChargeMissed;
    public Action<GameObject> OnChargePlayer;


    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_Boss.OnPlayerInSala += GetTarget;
    }

    private void GetTarget()
    {
        m_Target = m_Boss.Target;
    }

    public override void InitState()
    {
        base.InitState();
        m_NavMeshAgent.isStopped = false;
        m_IsAiming = false;
        m_IsCharging = false;
        m_Boss.SetBusy(true);
        StartCoroutine(ChargeCoroutine());
        m_NavMeshAgent.speed = m_ChargeSpeed;
        m_NavMeshAgent.ResetPath();
    }

    public override void ExitState()
    {
        base.ExitState();
        m_NavMeshAgent.isStopped = true;
    }

    Vector3 destino;
    private IEnumerator ChargeCoroutine()
    {
        m_IsAiming = true;
        yield return new WaitForSeconds(2f);
        m_IsAiming = false;
        m_IsCharging = true;
        m_NavMeshAgent.isStopped = false;
        destino = m_Target.position;
        m_NavMeshAgent.SetDestination(destino);
    }

    private void Update()
    {
        if (m_IsAiming)
        {
            m_Rigidbody.velocity = Vector3.zero;
            Vector2 posicionPlayer = m_Target.position - transform.position;
            float angulo = Mathf.Atan2(posicionPlayer.y, posicionPlayer.x);
            angulo = Mathf.Rad2Deg * angulo - 90;
            transform.localEulerAngles = new Vector3(0, 0, angulo);
        }
        if (m_IsCharging)
        {
            Vector2 posicionPlayer = destino - transform.position;
            float angulo = Mathf.Atan2(posicionPlayer.y, posicionPlayer.x);
            angulo = Mathf.Rad2Deg * angulo - 90;
            transform.localEulerAngles = new Vector3(0, 0, angulo);
            if(Vector2.Distance(destino, transform.position) < 0.2f)
                OnChargeMissed.Invoke(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enabled)
        {
            if (m_IsCharging)
            {
                print("Choque");
                m_IsCharging = false;
                m_NavMeshAgent.velocity = Vector3.zero;
                if (collision.gameObject.CompareTag("MechanicObstacle"))
                {
                    OnChargeMissed.Invoke(gameObject);
                }
                if (collision.gameObject.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                    OnChargePlayer.Invoke(gameObject);
                }
            }
        }
    }
}
