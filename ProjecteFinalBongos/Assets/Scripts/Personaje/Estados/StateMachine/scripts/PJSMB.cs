using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
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
    private PlayerEstadosController m_PlayerEstadosController;
    public PlayerEstadosController PlayerEstadosController => m_playerEstadosController;
    private SMBPlayerParryState m_SMBPlayerParryState;
    private static PJSMB m_Instance;
    public static PJSMB Instance => m_Instance;
    [SerializeField] private InventoryController m_Inventory;
    public InventoryController Inventory { get => m_Inventory; set => m_Inventory = value; }

    public Action m_CambiaElTarget;

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
        m_PlayerEstadosController = GetComponent<PlayerEstadosController>();
        m_SMBPlayerParryState = GetComponent<SMBPlayerParryState>();
        m_HealthController.onDeath += AcabarJuego;
        DontDestroyOnLoad(this.gameObject);
    }

    private void AcabarJuego()
    {
        GameManager.Instance.AcabarJuego();
    }

    private void Start()
    {
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_StateMachine.ChangeState<SMBPlayerIdleState>();

    }

    public void recibirDamage(float Daño)
    {
        if (m_playerEstadosController.Invencible)
            return;
        if (m_playerEstadosController.Burn)
        {
            m_HealthController.Damage(Daño);
            m_playerEstadosController.burntDamage = (Daño * m_playersStatsController.getModifier("Burnt")) / 100;
            m_HealthController.Damage(m_playerEstadosController.burntDamage);
        }
        if (m_playerEstadosController.Wrath && m_playerEstadosController.Paralized)
        {
            Daño += Daño * m_playersStatsController.getModifier("Paralized");
            m_HealthController.Damage(Daño);
        }
        else if (m_playerEstadosController.Wrath)
        {
            Daño += Daño * m_playersStatsController.getModifier("WrathLife");
            m_HealthController.Damage(Daño);
        }
        else if (m_playerEstadosController.Paralized)
        {
            Daño += Daño * m_playersStatsController.getModifier("Paralized");
            m_HealthController.Damage(Daño);
        }
        else
        {
            m_HealthController.Damage(Daño);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("BossHitBox") && !m_SMBPlayerParryState.parry)
        {
            if (collision.gameObject.TryGetComponent<BossAttackDamage>(out BossAttackDamage damageBoss))
            {
                m_HealthController.Damage(damageBoss.Damage);
                m_PlayerEstadosController.AlternarEstado(damageBoss.EstadoAlterado, damageBoss.StateTime);
            }
        }
    }

    public void GetDamage(float _Damage, EstadosAlterados estado, float time)
    {
        m_HealthController.Damage(_Damage);
        m_PlayerEstadosController.AlternarEstado(estado, time);
    }
}