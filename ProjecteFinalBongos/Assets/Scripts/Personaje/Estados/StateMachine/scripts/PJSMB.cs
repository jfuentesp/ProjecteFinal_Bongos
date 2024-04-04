using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace m17
{
    [RequireComponent(typeof(HealthController))]
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
        private HealthController m_HealthController;
        public PJSMB instance;

        private float m_Velocity = 5f;
        private float m_VelocityBase = 5f;
        public float Velocity => m_Velocity;
        public bool mullat = false;
        private EstadosAlterados m_estado;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            Assert.IsNotNull(m_InputAsset);
            m_StateMachine = GetComponent<FiniteStateMachine>();
            m_HealthController = GetComponent<HealthController>();  
            m_Input = Instantiate(m_InputAsset);
            m_MovementAction = m_Input.FindActionMap("PlayerActions").FindAction("Movement");
            m_Input.FindActionMap("PlayerActions").Enable();
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            m_StateMachine.ChangeState<SMBIdleState>();
        }

        public void AlternarEstado(EstadosAlterados estado) { 
            switch (estado)
            {
            case EstadosAlterados.Adormit:
                    m_StateMachine.ChangeState<SMBAdormitState>();
                    break;
            case EstadosAlterados.Atordit:
                    m_StateMachine.ChangeState<SMBStunState>();
                    break;
            case EstadosAlterados.Mullat:
                    if(!mullat)
                    StartCoroutine(MullatRoutine());
                    break;
            case EstadosAlterados.Peus_Lleugers:
                    break;
            case EstadosAlterados.Forçut:
                    break;
            case EstadosAlterados.Paralitzat:
                    m_HealthController.CambiarEstado(estado);
                    m_StateMachine.ChangeState<SMBParalitzatState>();
                    break;
            case EstadosAlterados.Cremat:
                    m_HealthController.CambiarEstado(estado);
                    break;
            case EstadosAlterados.Enverinat:
                    m_HealthController.CambiarEstado(estado);
                    break;
                default:
                    break;

            }
        }
        private IEnumerator MullatRoutine()
        {
            mullat = true;
            m_Velocity = (m_Velocity * 50) / 100;
            yield return new WaitForSeconds(5f);
            mullat = false;
            m_Velocity = m_VelocityBase;
            PararCorrutina(EstadosAlterados.Mullat);
        }

        public void PararCorrutina(EstadosAlterados estado) {
            switch (estado)
            {
                case EstadosAlterados.Adormit:
                    break;
                case EstadosAlterados.Atordit:
                    break;
                case EstadosAlterados.Mullat:
                    StopCoroutine(MullatRoutine());
                    break;
                case EstadosAlterados.Peus_Lleugers:
                    break;
                case EstadosAlterados.Forçut:
                    break;
                case EstadosAlterados.Paralitzat:
                    break;
                case EstadosAlterados.Cremat:
                    break;
                case EstadosAlterados.Enverinat:
                    break;
                default:
                    break;

            }
        }
    }

 
}
