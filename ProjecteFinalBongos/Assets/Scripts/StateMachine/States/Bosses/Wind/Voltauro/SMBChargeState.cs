using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SMBChargeState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;
    private NavMeshAgent m_NavMeshAgent;
        

    [Header("Charge speed")]
    [SerializeField]
    private float m_ChargeSpeed;

    [Header("Charge force to apply on impact")]
    [SerializeField]
    private float m_ChargeForce;

    private bool m_IsAiming;
    private bool m_IsCharging;

    private Transform m_Target;

    public Action<GameObject> OnChargeMissed;
    public Action<GameObject> OnChargeParried;
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
        m_IsAiming = false;
        m_IsCharging = false;
        m_Boss.SetBusy(true);
        StartCoroutine(ChargeCoroutine());
        m_NavMeshAgent.ResetPath();
    }

    public override void ExitState()
    {
        base.ExitState();
        m_NavMeshAgent.isStopped = true;
    }

    private IEnumerator ChargeCoroutine()
    {
        m_IsAiming = true;
        yield return new WaitForSeconds(2f);
        m_IsAiming = false;
        m_IsCharging = true;
    }

    Vector3 m_Direction;
    private void Update()
    {
        if (m_IsAiming)
        {
            m_Direction = (m_Target.transform.position - transform.position).normalized;
            m_Rigidbody.velocity = Vector3.zero;
            Vector2 posicionPlayer = m_Target.position - transform.position;
            float angulo = Mathf.Atan2(posicionPlayer.y, posicionPlayer.x);
            angulo = Mathf.Rad2Deg * angulo - 90;
            transform.localEulerAngles = new Vector3(0, 0, angulo);
        }
    }

    private void FixedUpdate()
    {
        if (m_IsCharging)
        {
            m_NavMeshAgent.isStopped = false;
            m_NavMeshAgent.velocity = m_Direction * m_ChargeSpeed;
        }
        //m_Rigidbody.velocity = m_Direction * m_ChargeSpeed;
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
                    if (collision.gameObject.GetComponent<SMBPlayerParryState>().parry)
                    {
                        OnChargeParried.Invoke(gameObject);
                    }
                    else
                    {
                        OnChargePlayer.Invoke(gameObject);
                        Rigidbody2D target;
                        collision.gameObject.TryGetComponent<Rigidbody2D>(out target);
                        if (target != null)
                            target.AddForce(transform.up * m_ChargeSpeed, ForceMode2D.Impulse);
                    }
                }
            }
        }
    }
}
