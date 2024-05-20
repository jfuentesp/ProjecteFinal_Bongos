using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SMBParriedState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;
    public delegate void OnRecomposition(GameObject obj);
    public OnRecomposition OnRecomposited;

    [Header("Parry duration")]
    [SerializeField] private float m_ParryDuration;

    [Header("Animation Name")]
    [SerializeField] private string m_AnimationParryName;


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
        if (m_AnimationParryName != String.Empty)
        {
            m_Animator.Play(m_AnimationParryName);
        }
        StartCoroutine(ParriedCoroutine());
    }
    private void Update()
    {
        
    }
    public override void ExitState()
    {
        base.ExitState();
    }

    private IEnumerator ParriedCoroutine()
    {
        yield return new WaitForSeconds(m_ParryDuration);
        //m_StateMachine.ChangeState<SMBChaseState>();
        OnRecomposited?.Invoke(gameObject);
    }
}
