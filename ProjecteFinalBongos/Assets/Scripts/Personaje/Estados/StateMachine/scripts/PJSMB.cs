using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlayerAbilitiesController))]
[RequireComponent(typeof(PlayerStatsController))]
[RequireComponent(typeof(PlayerEstadosController))]
[RequireComponent(typeof(HabilidadDeMovimientoState))]
[RequireComponent(typeof(SMBPlayerParryState))]
[RequireComponent(typeof(SMBPlayerSuccesfulParryState))]
[RequireComponent(typeof(HealthController))]
[RequireComponent(typeof(SMBPlayerIdleState))]
[RequireComponent(typeof(SMBPlayerWalkState))]
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
[RequireComponent(typeof(FiniteStateMachine))]

public class PJSMB : MonoBehaviour
{
    private FiniteStateMachine m_StateMachine;
    [SerializeField]
    private InputActionAsset m_InputAsset;
    private InputActionAsset m_Input;
    public InputActionAsset Input => m_Input;
    private InputAction m_MovementAction;
    public int direccion;
    public InputAction MovementAction => m_MovementAction;
    private HealthController m_HealthController;
    private PlayerAbilitiesController m_playerAbilitiesController;
    public PlayerAbilitiesController PlayerAbilitiesController => m_playerAbilitiesController;
    private PlayerEstadosController m_playerEstadosController;
    private PlayerStatsController m_playersStatsController;
    public PlayerStatsController PlayerStatsController => m_playersStatsController;

    private static PJSMB m_Instance;
    public static PJSMB Instance => m_Instance;


    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        Assert.IsNotNull(m_InputAsset);
        m_Input = Instantiate(m_InputAsset);
        m_MovementAction = m_Input.FindActionMap("PlayerActions").FindAction("Movement");
        m_Input.FindActionMap("PlayerActions").Enable();
        m_HealthController = GetComponent<HealthController>();
        m_playerAbilitiesController = GetComponent<PlayerAbilitiesController>();
        m_playerEstadosController = GetComponent<PlayerEstadosController>();
        m_playersStatsController = GetComponent<PlayerStatsController>();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_StateMachine.ChangeState<SMBPlayerIdleState>();

    }

    public void recibirDamage(float Da�o)
    {
        if (m_playerEstadosController.Invencible)
            return;
        if (m_playerEstadosController.Burn)
        {
            m_HealthController.Damage(Da�o);
            m_playerEstadosController.burntDamage = (Da�o * m_playersStatsController.getModifier("Burnt")) / 100;
            m_HealthController.Damage(m_playerEstadosController.burntDamage);
        }
        if (m_playerEstadosController.Wrath && m_playerEstadosController.Paralized)
        {
            Da�o += Da�o * m_playersStatsController.getModifier("Paralized");
            m_HealthController.Damage(Da�o);
        }
        else if (m_playerEstadosController.Wrath)
        {
            Da�o += Da�o * m_playersStatsController.getModifier("WrathLife");
            m_HealthController.Damage(Da�o);
        }
        else if (m_playerEstadosController.Paralized)
        {
            Da�o += Da�o * m_playersStatsController.getModifier("Paralized");
            m_HealthController.Damage(Da�o);
        }
        else
        {
            m_HealthController.Damage(Da�o);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


    }
}