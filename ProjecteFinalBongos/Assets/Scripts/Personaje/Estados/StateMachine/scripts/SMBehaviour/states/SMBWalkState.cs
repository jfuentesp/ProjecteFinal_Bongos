using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace m17
{
    public class SMBWalkState : MBState
    {
        private PJSMB m_PJ;
        private Rigidbody2D m_Rigidbody;
        private Animator m_Animator;
        private FiniteStateMachine m_StateMachine;

        private Vector2 m_Movement;

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
            m_PJ.Input.FindActionMap("PlayerActions").FindAction("Attack1").performed += OnAttack1;
            m_PJ.Input.FindActionMap("PlayerActions").FindAction("Attack2").performed += OnAttack2;
            m_Animator.Play("walkPlayer");
        }

        public override void Exit()
        {
            base.Exit();
            m_PJ.Input.FindActionMap("PlayerActions").FindAction("Attack1").performed -= OnAttack1;
            m_PJ.Input.FindActionMap("PlayerActions").FindAction("Attack2").performed -= OnAttack2;
        }

        private void OnAttack1(InputAction.CallbackContext context)
        {
            m_StateMachine.ChangeState<SMBHit1State>();
        }
        private void OnAttack2(InputAction.CallbackContext context)
        {
            m_StateMachine.ChangeState<SMBHit2State>();
        }

        private void Update()
        {

            m_Movement = m_PJ.MovementAction.ReadValue<Vector2>();

            if(m_Movement ==  Vector2.zero)
                m_StateMachine.ChangeState<SMBIdleState>();
        }

        private void FixedUpdate()
        {
            if (m_Movement.x > 0) {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (m_Movement.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            m_Rigidbody.velocity = m_Movement * m_PJ.Velocity;
        }
    }
}
