using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace m17
{
    [RequireComponent(typeof(ComboHandler))]
    public abstract class SMBComboState : MBState
    {
        private PJSMB m_PJ;
        protected Rigidbody2D m_Rigidbody;
        protected Animator m_Animator;
        protected FiniteStateMachine m_StateMachine;
        private ComboHandler m_ComboHandler;
        private void Awake()
        {
        
            m_PJ = GetComponent<PJSMB>();
            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_Animator = GetComponent<Animator>();
            m_StateMachine = GetComponent<FiniteStateMachine>();
            m_ComboHandler = GetComponent<ComboHandler>();
        }

        public override void Init()
        {
            base.Init();
            m_PJ.Input.FindActionMap("PlayerActions").FindAction("Attack1").performed += OnAttack1;
            m_PJ.Input.FindActionMap("PlayerActions").FindAction("Attack2").performed += OnAttack2;
            m_Rigidbody.velocity = Vector2.zero;
            m_ComboHandler.enabled = true;
            m_ComboHandler.OnEndAction += OnEndAction;
        }

        public override void Exit()
        {
            base.Exit();
            m_ComboHandler.enabled = false;
            if (m_PJ.Input != null) {
                m_PJ.Input.FindActionMap("PlayerActions").FindAction("Attack1").performed -= OnAttack1;
                m_PJ.Input.FindActionMap("PlayerActions").FindAction("Attack2").performed -= OnAttack2;
            }
            m_ComboHandler.OnEndAction -= OnEndAction;
        }

        private void OnAttack1(InputAction.CallbackContext context)
        {
            if (m_ComboHandler.ComboAvailable)
                OnComboSuccessAction();
            else
                OnComboFailedAction();
        }
        private void OnAttack2(InputAction.CallbackContext context)
        {
            if (m_ComboHandler.ComboAvailable)
                OnComboSuccessActionAttack2();
            else
                OnComboFailedAction();
        }

        protected abstract void OnComboSuccessAction();
        protected abstract void OnComboSuccessActionAttack2();
        protected abstract void OnComboFailedAction();

        protected abstract void OnEndAction();
    }
}
