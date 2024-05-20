using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBBossStunState : SMState
{
    private BossBehaviour m_BossBehaviour;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    [SerializeField]
    private TimesScriptable times;
    private float m_Time;
    public Action<GameObject> OnStopStun;

    [Header("Animation Name")]
    [SerializeField] private string m_NameAnimation;
    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_BossBehaviour = GetComponent<BossBehaviour>();

    }

    public override void InitState()
    {
        base.InitState();
        m_Time = times.m_StunTime;
        if(m_NameAnimation != String.Empty)
            m_Animator.Play(m_NameAnimation);
        m_Rigidbody.velocity = Vector2.zero;
        StartCoroutine(StunSeconds());

    }
    IEnumerator StunSeconds()
    {
        yield return new WaitForSeconds(m_Time);
        m_BossBehaviour.EstadosController.StopStun();
       OnStopStun?.Invoke(gameObject);
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
