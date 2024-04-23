using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBBossStunState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    [SerializeField]
    private GameEvent m_event;
    [SerializeField]
    private TimesScriptable times;
    private float m_Time;
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
        m_Time = times.m_StunTime;
        m_Animator.Play("stunned");
        m_Rigidbody.velocity = Vector2.zero;
        StartCoroutine(StunSeconds());

    }
    IEnumerator StunSeconds()
    {
        yield return new WaitForSeconds(m_Time);
        m_event.Raise();
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
    }
    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();
    }

    public void ChangeTime(float time)
    {
        m_Time = time;
    }
}
