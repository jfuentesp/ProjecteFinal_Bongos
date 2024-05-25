using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SMBChaosState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;
    [SerializeField] private string m_AnimationName;
    public Action empezarContador;

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
        m_Boss.SetBusy(true);
        m_Rigidbody.velocity = Vector3.zero;
        empezarContador?.Invoke();
        m_Animator.Play(m_AnimationName);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
