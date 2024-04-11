using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class SMBMovementState : SMState
{
    protected PJSMB m_PJ;
    protected Rigidbody2D m_Rigidbody;
    protected Animator m_Animator;
    protected FiniteStateMachine m_StateMachine;
    protected private float movementDistance;
    protected private float movementReduction;

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
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    protected abstract void MovementAction();
}
