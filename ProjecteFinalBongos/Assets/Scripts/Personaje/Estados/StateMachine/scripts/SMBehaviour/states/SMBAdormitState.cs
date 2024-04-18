using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SMBAdormitState : SMState
{
    private PJSMB m_PJ;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    [SerializeField]
    private GameEvent m_event;
    private bool sleep;
    [SerializeField]
    private TimesScriptable playerTimes;
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
        m_Animator.Play("sleepyPlayer");
        m_Rigidbody.velocity = Vector2.zero;
        StartCoroutine(StunSeconds());

    }

   /*private void OnTriggerEnter2D() {
        if (sleep) {
            m_event.Raise();
            Debug.Log(playerTimes.m_SleepTime);
            m_StateMachine.ChangeState<SMBPlayerIdleState>();
        }
    }*/
    IEnumerator StunSeconds()
    {
        sleep = true;
        yield return new WaitForSeconds(playerTimes.m_SleepTime);
        Debug.Log(playerTimes.m_SleepTime);
        sleep = false;
        m_event.Raise();
        m_StateMachine.ChangeState<SMBPlayerIdleState>();

    }
    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();
    }
}
