using UnityEngine;
using UnityEngine.InputSystem;

namespace m17
{
    public class SMBIdleState : MBState
    {
        private PJSMB m_PJ;
        private Rigidbody2D m_Rigidbody;
        private Animator m_Animator;
        private FiniteStateMachine m_StateMachine;

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
            m_Rigidbody.velocity = Vector2.zero;
            m_Animator.Play("idlePlayer");
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
            if (m_PJ.MovementAction.ReadValue<Vector2>() != Vector2.zero)
                m_StateMachine.ChangeState<SMBWalkState>();
        }
    }
}
