using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBChaseState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;

    [SerializeField]
    private Animation m_WalkAnimation;
    [SerializeField]
    private float m_WalkSpeed;
    [SerializeField]
    private float m_WalkCoroutineTime;
    [SerializeField]
    [Tooltip("If this is set to true, it will randomly walk between zero and Walk Coroutine Time.")]
    private bool m_IsWalkCoroutineRandom;
    [SerializeField]
    private GameObject m_Target;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
    }

    public override void InitState()
    {
        base.InitState();
        //m_WalkCoroutine = StartCoroutine(WalkCoroutine(m_WalkCoroutineTime));
    }

    public override void ExitState()
    {
        base.ExitState();
        if(m_WalkCoroutine != null)
            StopCoroutine(m_WalkCoroutine);
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
        m_Rigidbody.velocity = direction * m_WalkSpeed;
    }

    private Coroutine m_WalkCoroutine;
    private IEnumerator WalkCoroutine(float coroutineTime)
    {
        while(true)
        {

        }
    }
}
