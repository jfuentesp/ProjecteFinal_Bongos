using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBChaseState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;

    [Header("Chase speed")]
    [SerializeField]
    private float m_ChaseSpeed;

    [Header("Target to chase")]
    [SerializeField]
    private GameObject m_Target;

    [Header("Chase animation")]
    [SerializeField]
    private string m_ChaseAnimationName;

    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
    }

    public override void InitState()
    {
        base.InitState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    private void Update()
    {
        //To face the target
        if (m_Target != null)
            transform.up = m_Target.transform.position - transform.position;
    }

    private void FixedUpdate()
    {
        m_Rigidbody.velocity = Vector3.zero;
        Vector3 direction = (m_Target.transform.position - transform.position).normalized;
        m_Rigidbody.velocity = direction * m_ChaseSpeed;
    }
}
