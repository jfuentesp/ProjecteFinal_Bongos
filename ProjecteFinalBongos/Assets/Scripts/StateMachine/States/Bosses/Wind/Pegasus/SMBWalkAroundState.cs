using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    private float m_WalkingDuration;

    [Header("Boss target")]
    [SerializeField]
    private GameObject m_Target;
    float m_CurrentDuration;


    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
    }
    public override void InitState()
    {
        base.InitState();
        m_CurrentDuration = 0;
        m_Boss.SetBusy(true);
        m_Rigidbody.velocity = Vector2.up;
    }

   
    public override void ExitState()
    {
        base.ExitState();
    }

    void Update()
    {
        Vector2 moveDirection = transform.position - m_Target.transform.position;

        moveDirection.Normalize();

        m_Rigidbody.velocity = moveDirection * m_WalkSpeed;

        m_CurrentDuration += Time.deltaTime;
        if (m_CurrentDuration >= m_WalkingDuration)
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        }
    }
}
