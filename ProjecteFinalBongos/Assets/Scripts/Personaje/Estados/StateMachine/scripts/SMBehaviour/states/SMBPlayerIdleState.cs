using UnityEngine;
using UnityEngine.InputSystem;


    public class SMBPlayerIdleState : SMState
    {
        private PJSMB m_PJ;
        private Rigidbody2D m_Rigidbody;
        private Animator m_Animator;
        private FiniteStateMachine m_StateMachine;


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
            m_Rigidbody.velocity = Vector2.zero;

        if (m_PJ.direccion == 0)
        {
            m_Animator.Play("idlePlayer");
        }
        else if (m_PJ.direccion == 1) {
            m_Animator.Play("idleDown");
        }
        else if (m_PJ.direccion == 2)
        {
            m_Animator.Play("idleUp");
        }



    }

    public override void ExitState()
        {
            base.ExitState();
            if (m_PJ.Input != null)
            {
                m_PJ.Input.FindActionMap("PlayerActions").FindAction("Attack1").performed -= OnAttack1;
                m_PJ.Input.FindActionMap("PlayerActions").FindAction("Attack2").performed -= OnAttack2;
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

        private void Update()
        {
            if (m_PJ.MovementAction.ReadValue<Vector2>() != Vector2.zero)
                m_StateMachine.ChangeState<SMBPlayerWalkState>();
        }
    }

