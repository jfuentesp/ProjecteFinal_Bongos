using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBParriedState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;

    [Header("Parry duration")]
    [SerializeField]
    private float m_ParryDuration;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
    }

    public override void InitState()
    {
        base.InitState();
        StartCoroutine(ParriedCoroutine());
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    private IEnumerator ParriedCoroutine()
    {
        yield return new WaitForSeconds(m_ParryDuration);
        m_StateMachine.ChangeState<SMBChaseState>();
    }


}