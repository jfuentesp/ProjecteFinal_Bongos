using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBChaseState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;

    [Header("Chase speed")]
    [SerializeField]
    private float m_ChaseSpeed;

    private Transform m_Target;

    [Header("Chase animation")]
    [SerializeField]
    private string m_ChaseAnimationName;

    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        m_Target = m_Boss.Target;
    }

    public override void InitState()
    {
        base.InitState();

        m_Boss.SetBusy(false);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    private void Update()
    {
        //To face the target
        if (m_Target != null)
            transform.up = m_Target.position - transform.position;
    }

    private void FixedUpdate()
    {
        m_Rigidbody.velocity = Vector3.zero;
        Vector3 direction = (m_Target.position - transform.position).normalized;
        m_Rigidbody.velocity = direction * m_ChaseSpeed;
    }
}
