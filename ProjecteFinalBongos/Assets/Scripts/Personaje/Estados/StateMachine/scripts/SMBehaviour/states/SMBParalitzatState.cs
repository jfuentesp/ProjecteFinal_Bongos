using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBParalitzatState : SMState
{
    private PJSMB m_PJ;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    [SerializeField]
    private GameEvent m_event;
    private float m_time;
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
        
        m_Animator.Play("paralizedPlayer");
        m_Rigidbody.velocity = Vector2.zero;
        StartCoroutine(StunSeconds());

    }   
    IEnumerator StunSeconds()
    {
        yield return new WaitForSeconds(m_time);
        m_event.Raise();
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
    }
    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();
    }
    public void ChangeTime(float time) { 
        m_time = time;
    }
}
