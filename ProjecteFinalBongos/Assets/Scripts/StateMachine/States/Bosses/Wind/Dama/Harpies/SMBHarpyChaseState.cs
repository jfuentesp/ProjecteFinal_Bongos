using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBHarpyChaseState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;

    private Transform m_Target;

    [Header("Flying animation")]
    [SerializeField]
    private string m_FlyingHarpyAnimationname;

    [Header("Flying speed")]
    [SerializeField]
    private float m_FlyingSpeed;

    [Header("GameEvent to call on death")]
    [SerializeField]
    private GameEvent m_OnDeathEvent;

    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Animator = GetComponent<Animator>();
        m_Boss = GetComponent<BossBehaviour>();
    }

    public override void InitState()
    {
        base.InitState();
        m_Target = m_Boss.Target;
        //Esto en realidad tendrá que ir cuando el bicho muera
        m_OnDeathEvent.Raise();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    private void Update()
    {
        if (m_Target != null)
            transform.up = m_Target.position - transform.position;
    }

    private void FixedUpdate()
    {
        m_Rigidbody.velocity = Vector3.zero;
        Vector3 direction = (m_Target.position - transform.position).normalized;
        m_Rigidbody.velocity = direction * m_FlyingSpeed;
    }
}
