using m17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBParalitzatState : MBState
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
        
        m_Animator.Play("paralizedPlayer");
        m_Rigidbody.velocity = Vector2.zero;
        StartCoroutine(StunSeconds());

    }   
    IEnumerator StunSeconds()
    {
        yield return new WaitForSeconds(9f);
        m_event.Raise();
        m_StateMachine.ChangeState<SMBIdleState>();
    }
    public override void Exit()
    {
        base.Exit();
        StopAllCoroutines();
    }
}
