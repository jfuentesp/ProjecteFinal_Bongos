using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBChargeState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;

    [Header("Charge speed")]
    [SerializeField]
    private float m_ChargeSpeed;

    [Header("Charge force to apply on impact")]
    [SerializeField]
    private float m_ChargeForce;

    private bool m_IsAiming;
    private bool m_IsCharging;

    [Header("Boss target")]
    [SerializeField]
    private GameObject m_Target;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
    }

    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true);
        StartCoroutine(ChargeCoroutine());
    }

    public override void ExitState()
    {
        base.ExitState();
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
        if(m_IsAiming) 
        {
            m_Direction = (m_Target.transform.position - transform.position).normalized;
            m_Rigidbody.velocity = Vector3.zero;
            transform.up = m_Target.transform.position - transform.position;
        }
    }


    private void FixedUpdate()
    { 
        if (m_IsCharging)
            m_Rigidbody.velocity = m_Direction * m_ChargeSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(m_IsCharging)
        {
            m_IsCharging = false;
            m_Rigidbody.velocity = Vector3.zero;
            if(collision.gameObject.CompareTag("MechanicObstacle"))
                m_StateMachine.ChangeState<SMBParriedState>();
            if(collision.gameObject.CompareTag("Player"))
            {
                m_StateMachine.ChangeState<SMBChaseState>();
                Rigidbody2D target;
                collision.gameObject.TryGetComponent<Rigidbody2D>(out target);
                if(target != null)
                    target.AddForce(transform.up * m_ChargeSpeed, ForceMode2D.Impulse);
            }
            m_Boss.SetBusy(false);
        }
    }
}
