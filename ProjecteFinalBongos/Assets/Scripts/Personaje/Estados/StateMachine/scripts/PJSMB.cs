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
    [RequireComponent(typeof(SMBParalitzatState))]
    [RequireComponent(typeof(SMBStunState))]
    [RequireComponent(typeof(SMBAdormitState))]

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
        private EstadosAlterados m_estado = EstadosAlterados.Normal;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            Assert.IsNotNull(m_InputAsset);
            m_Input = Instantiate(m_InputAsset);
            m_MovementAction = m_Input.FindActionMap("PlayerActions").FindAction("Movement");
            m_Input.FindActionMap("PlayerActions").Enable();
            m_StateMachine = GetComponent<FiniteStateMachine>();
            m_HealthController = GetComponent<HealthController>();  
        
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
                    if (m_estado.Equals(EstadosAlterados.Normal))
                        m_StateMachine.ChangeState<SMBAdormitState>();
                    break;
            case EstadosAlterados.Atordit:
                    if (m_estado.Equals(EstadosAlterados.Normal))
                        m_StateMachine.ChangeState<SMBStunState>();
                    break;
            case EstadosAlterados.Mullat:
                    if(m_estado.Equals(EstadosAlterados.Normal))
                        StartCoroutine(estadoRoutine());
                    break;
            case EstadosAlterados.Peus_Lleugers:

                    break;
            case EstadosAlterados.Forçut:
                    break;
            case EstadosAlterados.Paralitzat:
                    if (m_estado.Equals(EstadosAlterados.Normal))
                    {
                        Debug.Log("Entro");
                        m_estado = estado;
                        m_HealthController.CambiarEstado(estado);
                        m_StateMachine.ChangeState<SMBParalitzatState>();
                    }
                    break;
            case EstadosAlterados.Cremat:
                    if (m_estado.Equals(EstadosAlterados.Normal))
                        m_HealthController.CambiarEstado(estado);
                    break;
            case EstadosAlterados.Enverinat:
                    if (m_estado.Equals(EstadosAlterados.Normal))
                        m_HealthController.CambiarEstado(estado);
                    break;
                default:
                    break;

            }
        }
        private void Update()
        {
            //Debug.Log(m_estado);
        }
        private IEnumerator estadoRoutine()
        {
            if (m_estado.Equals(EstadosAlterados.Mullat)) {
                m_Velocity = (m_Velocity * 50) / 100;
                yield return new WaitForSeconds(5f);
                m_Velocity = m_VelocityBase;
                PararCorrutina(EstadosAlterados.Mullat);
            }
  
        }

        public void PararCorrutina(EstadosAlterados estado) {
            switch (estado)
            {
                case EstadosAlterados.Mullat:
                    StopCoroutine(estadoRoutine());
                    m_estado = EstadosAlterados.Normal;
                    break;
                case EstadosAlterados.Peus_Lleugers:
                    break;
                case EstadosAlterados.Forçut:
                    break;
                default:
                    break;

            }
        }
        public void EstadoNormal()
        {
            m_estado = EstadosAlterados.Normal;
            Debug.Log("Hola");
        }
    }

 
}
