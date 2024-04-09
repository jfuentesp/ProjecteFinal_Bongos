using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class SMBPlayerWalkState : SMState
{
    private PJSMB m_PJ;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;

    private Vector2 m_Movement;

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
        m_PJ.Input.FindActionMap("PlayerActions").FindAction("Attack1").performed += OnAttack1;
        m_PJ.Input.FindActionMap("PlayerActions").FindAction("Attack2").performed += OnAttack2;
        m_PJ.Input.FindActionMap("PlayerActions").FindAction("Parry").performed += Parry;

    }

    public override void ExitState()
    {
        base.ExitState();
        if (m_PJ.Input) {
            m_PJ.Input.FindActionMap("PlayerActions").FindAction("Attack1").performed -= OnAttack1;
            m_PJ.Input.FindActionMap("PlayerActions").FindAction("Attack2").performed -= OnAttack2;
            m_PJ.Input.FindActionMap("PlayerActions").FindAction("Parry").performed += Parry;
        }

    }

    private void OnAttack1(InputAction.CallbackContext context)
    {
        m_StateMachine.ChangeState<SMBHit1State>();
    }
    private void OnAttack2(InputAction.CallbackContext context)
    {
        m_StateMachine.ChangeState<SMBHit2State>();
    }
    private void Parry(InputAction.CallbackContext context)
    {
        m_StateMachine.ChangeState<SMBPlayerParryState>();
    }

    private void Update()
    {

        m_Movement = m_PJ.MovementAction.ReadValue<Vector2>();

        if(m_Movement ==  Vector2.zero)
            m_StateMachine.ChangeState<SMBPlayerIdleState>();
    }

    private void FixedUpdate()
    {
        if (m_Movement.x > 0) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            m_PJ.direccion = 0;
            m_Animator.Play("walkPlayer");
        }
        else if (m_Movement.x < 0)
        {
            m_PJ.direccion = 0;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            m_Animator.Play("walkPlayer");
        }
        if (m_Movement.y < 0 && m_Movement.x == 0)
        {
            m_PJ.direccion = 1;
            m_Animator.Play("walkDown");
        }
        else if (m_Movement.y > 0 && m_Movement.x == 0)
        {
            m_PJ.direccion = 2;
            m_Animator.Play("walkUp");
        }

        m_Rigidbody.velocity = m_Movement * m_PJ.Velocity;
    }
}

