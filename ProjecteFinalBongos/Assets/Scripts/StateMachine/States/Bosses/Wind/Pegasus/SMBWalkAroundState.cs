using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SMBWalkAroundState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;

    [Header("Walk speed")]
    [SerializeField]
    private float m_WalkSpeed;

    [Header("Walking duration")]
    [SerializeField]
    private float m_minimumWalkingDuration;
    [SerializeField]
    private float m_maximumWalkingDuration;
    private float m_WalkingDuration;

    [Header("Chase animation")]
    [SerializeField]
    private string m_WalkAnimationName;

    private Transform m_Target;
    float m_CurrentDuration;

    [SerializeField] private bool m_TwoDirections;

    private NavMeshAgent m_NavMeshAgent;


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
        m_WalkingDuration = Random.Range(m_minimumWalkingDuration, m_maximumWalkingDuration);
        m_CurrentDuration = 0;
        m_Boss.SetBusy(true);
        m_Rigidbody.velocity = Vector2.up;
        m_NavMeshAgent.isStopped = false;
        if (m_WalkAnimationName != String.Empty)
            m_Animator.Play(m_WalkAnimationName);
    }

   
    public override void ExitState()
    {
        base.ExitState();
        m_NavMeshAgent.isStopped = true;
    }

    void Update()
    {
        Vector2 moveDirection = transform.position - m_Target.transform.position;

        moveDirection.Normalize();

        m_NavMeshAgent.SetDestination(moveDirection * 5);

        if (m_TwoDirections)
        {
            if (moveDirection.x - transform.position.x < 0)
                transform.localEulerAngles = new Vector3(0, 180, 0);
            else
                transform.localEulerAngles = Vector3.zero;
        }
        else
        {

        }

        //m_Rigidbody.velocity = moveDirection * m_WalkSpeed;

        m_CurrentDuration += Time.deltaTime;
        if (m_CurrentDuration >= m_WalkingDuration)
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        }
    }
}
