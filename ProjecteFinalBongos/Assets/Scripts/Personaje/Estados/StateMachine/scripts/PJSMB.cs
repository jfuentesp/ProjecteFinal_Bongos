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
        private Animator m_Animator;
        [SerializeField]
        private InputActionAsset m_InputAsset;
        private InputActionAsset m_Input;
        public InputActionAsset Input => m_Input;
        private InputAction m_MovementAction;
        public InputAction MovementAction => m_MovementAction;
        private HealthController m_HealthController;
        public PJSMB instance;
        private EstadosAlterados m_estado = EstadosAlterados.Normal;

        [Header("Modificadores")]
        private float m_WetModifier;
        private float m_StrengthModifier;
        private float m_VelocityModifier;
        private float m_ParalizedLifeModifier;
        private float m_WrathLifeModifier;
        private float m_WrathSpeedModifier;
        private float m_WrathStrengthModifier;
        [Header("BaseStats")]
        private float m_BaseVelocity;
        private float m_BaseAttackTime;
        private float m_BaseStrongAttack;
        private float m_BaseWeakAttack;
        private float m_BaseDefense;
        private float m_BaseStrength;
        [Header("Stats")]
        [SerializeField]
        private float m_Velocity;
        public float Velocity => m_Velocity;
        [SerializeField]
        private float m_AttackTime;
        [SerializeField]
        private float m_Strength;
        [SerializeField]
        private float m_Defense;
        [SerializeField]
        private float m_StrongAttack;
        [SerializeField]
        private float m_WeakAttack; 

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            m_BaseVelocity = 5f;
            m_BaseAttackTime = 0f;
            m_BaseStrongAttack = 5f;
            m_BaseWeakAttack = 3f;
            m_BaseDefense = 10f;
            m_BaseStrength = 10f;
            Assert.IsNotNull(m_InputAsset);
            m_Input = Instantiate(m_InputAsset);
            m_MovementAction = m_Input.FindActionMap("PlayerActions").FindAction("Movement");
            m_Input.FindActionMap("PlayerActions").Enable();
            m_StateMachine = GetComponent<FiniteStateMachine>();
            m_HealthController = GetComponent<HealthController>();  
            m_Animator = GetComponent<Animator>();  
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            m_StateMachine.ChangeState<SMBIdleState>();
            m_HealthController.OnChange += AlternarEstado;
            m_Velocity = m_BaseVelocity;
            m_Strength = m_BaseStrength;
            m_Defense = m_BaseDefense;
            m_AttackTime = m_BaseAttackTime;
        }

        public void AlternarEstado(EstadosAlterados estado) {
            switch (estado)
            {
            case EstadosAlterados.Adormit:
                     
                    break;
            case EstadosAlterados.Atordit:
                  
                    break;
            case EstadosAlterados.Mullat:
            
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
                case EstadosAlterados.Invencible:
               
                        break;
                case EstadosAlterados.Ira:
                 
                    break;
                case EstadosAlterados.Normal:
                    m_estado = estado;
                    break;
                default:
                    break;

            }
        }
        public void recibirDaño(float Daño)
        {
            switch (m_estado)
            {
                case EstadosAlterados.Paralitzat:

                    break;
                case EstadosAlterados.Cremat:

                    break;
                case EstadosAlterados.Invencible:

                    break;
                case EstadosAlterados.Ira:

                    break;
                default:
                    m_HealthController.Damage(Daño);
                    break;
            }

        }
        public void PararCorrutina( ) { 
            AlternarEstado(EstadosAlterados.Normal);
        }
    }

 
}
