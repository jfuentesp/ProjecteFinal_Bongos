using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenSetSubMergeState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;

    [Header("Set Submerge animation")]
    [SerializeField]
    private string m_SetSubmergeAnimationName;

    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Animator = GetComponent<Animator>();
    }

    public override void InitState()
    {
        base.InitState();
        m_Animator.Play(m_SetSubmergeAnimationName);
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
