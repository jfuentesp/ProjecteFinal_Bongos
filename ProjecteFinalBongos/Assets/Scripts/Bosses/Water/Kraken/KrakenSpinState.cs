using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenSpinState : SMState
{
    [SerializeField] private string m_SpinAnimation;
    public bool trapped;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;
    private Transform m_Target;
    public Action<GameObject> onAttackStopped;

    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        m_Boss.OnPlayerInSala += GetTarget;
    }

    private void GetTarget()
    {
        m_Target = m_Boss.Target;
        
    }
    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true);
        m_Rigidbody.velocity = Vector3.zero;
        m_Animator.Play(m_SpinAnimation);
    }

    public void FinishSpin() {
        onAttackStopped?.Invoke(gameObject);
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
