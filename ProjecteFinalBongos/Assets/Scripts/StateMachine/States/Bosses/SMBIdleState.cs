using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SMBIdleState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    public Action<GameObject> OnPlayerEnter;

    [Header("Idle animation")]
    [SerializeField]
    private string m_IdleAnimationName;

    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>(); 
        
        GetComponent<BossBehaviour>().OnPlayerInSala += GetTarget;
    }

    private void GetTarget()
    {
        OnPlayerEnter?.Invoke(gameObject);
    }
    private void Update()
    {
        
    }
    public override void InitState()
    {
        base.InitState();
        m_Rigidbody.velocity = Vector3.zero;
    }

    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();
    }
}
