using m17;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SMBAdormitState : MBState
{
    private PJSMB m_PJ;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    [SerializeField]
    private GameEvent m_event;
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
        m_Animator.Play("sleepyPlayer");
        m_Rigidbody.velocity = Vector2.zero;
        StartCoroutine(StunSeconds());

    }

   private void OnTriggerEnter2D() {
        if (this.enabled) {
            m_event.Raise();
            m_StateMachine.ChangeState<SMBIdleState>();
        }
    }
    IEnumerator StunSeconds()
    {
        yield return new WaitForSeconds(2f);
        m_event.Raise();
        m_StateMachine.ChangeState<SMBIdleState>();

    }
    public override void Exit()
    {
        base.Exit();
        StopAllCoroutines();
    }
}
