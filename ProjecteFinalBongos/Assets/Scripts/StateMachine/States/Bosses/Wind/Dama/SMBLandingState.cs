using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SMBLandingState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;

    private Transform m_Target;

    [Header("Landing animation")]
    [SerializeField]
    private string m_LandingAnimationName;

    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        m_Target = m_Boss.Target;
    }

    public override void InitState()
    {
        base.InitState();
        m_Animator.Play(m_LandingAnimationName);
        m_Boss.SetBusy(false);
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
