using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace m17
{
    [RequireComponent(typeof(FiniteStateMachine))]
    [RequireComponent(typeof(SMBIdleState))]
    [RequireComponent(typeof(SMBWalkState))]
    [RequireComponent(typeof(SMBHit1State))]
    [RequireComponent(typeof(SMBHit1x2State))]
    [RequireComponent(typeof(SMBHit1x3State))]
    [RequireComponent(typeof(SMBHit1x4State))]
    [RequireComponent(typeof(SMBHit1AplastanteState))]
    [RequireComponent(typeof(SMBHit2State))]
    [RequireComponent(typeof(SMBHit2x2State))]
    [RequireComponent(typeof(SMBHit2x3State))]
    [RequireComponent(typeof(SMBHit2AereoState))]

    public class PJSMB : MonoBehaviour
    {
        private FiniteStateMachine m_StateMachine;

        [SerializeField]
        private InputActionAsset m_InputAsset;
        private InputActionAsset m_Input;
        public InputActionAsset Input => m_Input;
        private InputAction m_MovementAction;
        public InputAction MovementAction => m_MovementAction;

        private void Awake()
        {
            Assert.IsNotNull(m_InputAsset);
            m_StateMachine = GetComponent<FiniteStateMachine>();

            m_Input = Instantiate(m_InputAsset);
            m_MovementAction = m_Input.FindActionMap("PlayerActions").FindAction("Movement");
            m_Input.FindActionMap("PlayerActions").Enable();
        }

        private void Start()
        {
            m_StateMachine.ChangeState<SMBIdleState>();
        }
    }
}
