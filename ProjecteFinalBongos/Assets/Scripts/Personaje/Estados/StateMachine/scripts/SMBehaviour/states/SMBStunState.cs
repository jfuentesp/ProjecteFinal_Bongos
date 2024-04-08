using m17;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class SMBStunState : MBState
{
    private PJSMB m_PJ;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    [SerializeField]
    private GameEvent m_event;
    [SerializeField]
    private TimesScriptable playerTimes;
    private void Awake()
    {
        m_PJ = GetComponent<PJSMB>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
    }

    public override void Init()
    {
        base.Init();
        m_Animator.Play("stunnedPlayer");
        m_Rigidbody.velocity = Vector2.zero;
        StartCoroutine(StunSeconds());
        
    }
    IEnumerator StunSeconds() { 
        yield return new WaitForSeconds(playerTimes.m_StunTime);
        m_event.Raise();
        m_StateMachine.ChangeState<SMBIdleState>();
    }
    public override void Exit()
    {
        base.Exit();
        StopAllCoroutines();
    }
}
