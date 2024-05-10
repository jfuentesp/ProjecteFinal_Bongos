using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBParalized : SMState
{
    private BossBehaviour m_BossBehaviour;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    [SerializeField]
    private TimesScriptable times;
    public Action<GameObject> OnStopParalized;
    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_BossBehaviour = GetComponent<BossBehaviour>();
    }

    public override void InitState()
    {
        base.InitState();
        m_Rigidbody.velocity = Vector2.zero;
        StartCoroutine(StunSeconds());

    }
    IEnumerator StunSeconds()
    {
        yield return new WaitForSeconds(times.m_ParalizedTime);
        m_BossBehaviour.EstadosController.StopStun();
        OnStopParalized?.Invoke(gameObject);
        //m_StateMachine.ChangeState<SMBIdleState>();
    }
    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();
    }
}
