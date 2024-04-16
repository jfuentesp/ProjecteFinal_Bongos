
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class SMBStunState : SMState
{
    private PJSMB m_PJ;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    [SerializeField]
    private GameEvent m_event;
    [SerializeField]
    private TimesScriptable playerTimes;
    private float m_Time;
    private new void Awake()
    {
        base.Awake();
        m_PJ = GetComponent<PJSMB>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
    }

    public override void InitState()
    {
        base.InitState();
        m_Animator.Play("stunnedPlayer");
        m_Rigidbody.velocity = Vector2.zero;
        StartCoroutine(StunSeconds());
        
    }
    IEnumerator StunSeconds() { 
        yield return new WaitForSeconds(m_Time);
        m_event.Raise();
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
    }
    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();
        print("aaa ");
    }

    public void ChangeTime(float time) { 
        m_Time = time;
    }
}
