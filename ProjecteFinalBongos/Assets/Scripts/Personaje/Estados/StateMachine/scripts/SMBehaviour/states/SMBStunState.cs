
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
    [SerializeField]
    private AudioClip[] m_AudioStunClip;
    private AudioSource m_AudioSource;
    private float m_Time;
    private new void Awake()
    {
        base.Awake();
        m_PJ = GetComponent<PJSMB>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    public override void InitState()
    {
        base.InitState();
        m_Animator.Play("stunnedPlayer");
        StartCoroutine(StunSeconds());
        m_AudioSource.clip = m_AudioStunClip[Random.Range(0, m_AudioStunClip.Length)];
        m_AudioSource.Play();
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
    }

    public void ChangeTime(float time) {
        m_Time = time;
    }
}
