using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;


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
    private Animator m_Animator;
    [SerializeField]
    private InputActionAsset m_InputAsset;
    private InputActionAsset m_Input;
    public InputActionAsset Input => m_Input;
    private InputAction m_MovementAction;
    public int direccion;
    public InputAction MovementAction => m_MovementAction;
    private HealthController m_HealthController;
    public PJSMB instance;
    private EstadosAlterados m_estado = EstadosAlterados.Normal;
    private bool Invencible, Stun, Poison,Wet,Burn, Wrath, Speedy, StrongMan, Stuck, Paralized;
    [Header("Otros")]
    private float velocityBefore;
    private float strengthBefore;
    private const int poisonNum = 4;
    private int poisonCount = poisonNum;
    private float poisonDamage;
    private float burntDamage;
    [Header("Tiempos")]
    [SerializeField]
    private TimesScriptable m_playerTimes;
   
    [Header("Modificadores")]
    [SerializeField]
    private float m_ParalizedLifeModifier;
    [SerializeField]
    private float m_WrathLifeModifier;
    private float m_WrathSpeedModifier;
    private float m_WrathStrengthModifier;
    private float m_WetModifier;
    private float m_StrengthModifier;
    private float m_PoisonModifier;
    private float m_BurntModifier;
    private float m_VelocityModifier;
    [Header("BaseStats")]
    [SerializeField]
    private PlayerBase m_PlayerBaseStats;
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
        Assert.IsNotNull(m_InputAsset);
        m_Input = Instantiate(m_InputAsset);
        m_MovementAction = m_Input.FindActionMap("PlayerActions").FindAction("Movement");
        m_Input.FindActionMap("PlayerActions").Enable();
        m_HealthController = GetComponent<HealthController>();
        m_Animator = GetComponent<Animator>();
        Stun = false;
        Debug.Log("Existo?");
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
        m_Velocity = m_PlayerBaseStats.m_BaseVelocity;
        m_Strength = m_PlayerBaseStats.m_BaseStrength;
        m_Defense = m_PlayerBaseStats.m_BaseDefense;
        m_AttackTime = m_PlayerBaseStats.m_BaseAttackTime;
    }

    public void AlternarEstado(EstadosAlterados estado) {
        switch (estado)
        {
            case EstadosAlterados.Adormit:
                if (!Stun) {
                    Stun = true;
                    m_StateMachine.ChangeState<SMBAdormitState>();
                }
                break;
            case EstadosAlterados.Atordit:
                if (!Stun)
                {
                    Stun = true;
                    m_StateMachine.ChangeState<SMBStunState>();
                }
                break;
            case EstadosAlterados.Mullat:
                if(!Wet)
                    StartCoroutine(WetRoutine());
                break;
            case EstadosAlterados.Peus_Lleugers:
                if(!Speedy)
                    StartCoroutine(SpeedRoutine());
                break;
            case EstadosAlterados.Forçut:
                if(!StrongMan)
                    StartCoroutine(StrongRoutine());
                break;
            case EstadosAlterados.Paralitzat:
                if (!Stun)
                {
                    Stun = true;
                    m_StateMachine.ChangeState<SMBParalitzatState>();
                }
                break;
            case EstadosAlterados.Cremat:
                if(!Burn)
                    StartCoroutine(BurntRoutine());
                break;
            case EstadosAlterados.Enverinat:
                if (!Poison) {
                    StartCoroutine(PoisonRoutine());
                }
                      
                break;
            case EstadosAlterados.Invencible:
                if(!Invencible)
                    StartCoroutine(InvencibleRoutine());
                break;
            case EstadosAlterados.Ira:
                if(!Wrath)
                    StartCoroutine(WrathRoutine());
                break;
            case EstadosAlterados.Atrapat:
                if(!Stuck)
                    StartCoroutine(StuckRoutine());
                break;
            default:
                break;

        }
    }

    IEnumerator WetRoutine() {
        Wet = true;
        m_WetModifier = Random.Range(10f, 21f);
        velocityBefore = m_Velocity;
        m_Velocity -= (m_Velocity * m_WetModifier) / 100;
        yield return new WaitForSeconds(m_playerTimes.m_WetTime);
        Wet = false;
        m_Velocity = velocityBefore;
        PararCorrutina("WetRoutine");
    }
    IEnumerator SpeedRoutine() {
        Speedy = true;
        m_VelocityModifier = Random.Range(10f, 21f);
        velocityBefore = m_Velocity;
        m_Velocity += (m_Velocity * m_VelocityModifier) / 100;
        yield return new WaitForSeconds(m_playerTimes.m_VelocityTime);
        Speedy = false;
        m_Velocity = velocityBefore;
        PararCorrutina("SpeedRoutine");
    }
    IEnumerator StrongRoutine() {
        StrongMan = true;
        m_Strength = Random.Range(5f, 16f);
        strengthBefore = m_Strength;
        m_Strength += (m_Strength * m_StrengthModifier) / 100;
        yield return new WaitForSeconds(m_playerTimes.m_StrengthTime);
        StrongMan = false;
        m_Strength = strengthBefore;
        PararCorrutina("StrongRoutine");
    }
    IEnumerator BurntRoutine() {
        Burn = true;
        yield return new WaitForSeconds(m_playerTimes.m_BurnTime);
        Burn = false;
        PararCorrutina("BurntRoutine");
    }
    IEnumerator PoisonRoutine()
    {   Poison = true;
        while (poisonCount > 0)
        {
            yield return new WaitForSeconds(m_playerTimes.m_PoisonTime);
            m_PoisonModifier = Random.Range(2, 5);
            poisonDamage = (m_HealthController.HP * m_PoisonModifier) / 100;
            m_HealthController.Damage(poisonDamage);
            poisonCount--;
        }
        Poison = false;
        poisonCount = poisonNum;
        PararCorrutina("PoisonRoutine");
    }
    IEnumerator InvencibleRoutine() {
        Invencible = true;
        yield return new WaitForSeconds(m_playerTimes.m_InvencibleTime);
        Invencible = false;
        PararCorrutina("InvencibleRoutine");
    }
    IEnumerator WrathRoutine() {
        Wrath = true;
        m_WrathSpeedModifier = Random.Range(15f, 26f);
        velocityBefore = m_Velocity;
        m_Velocity += (m_Velocity * m_WrathSpeedModifier) / 100;
        m_WrathStrengthModifier = Random.Range(10f, 21f);
        strengthBefore = m_Strength;
        m_Strength += (m_Strength * m_WrathStrengthModifier) / 100;
        yield return new WaitForSeconds(m_playerTimes.m_WrathTime);
        Wrath = false;
        m_Strength = strengthBefore;
        m_Velocity = velocityBefore;
        PararCorrutina("WrathRoutine");
            
    }
    IEnumerator StuckRoutine() {
        Stuck = true;
        velocityBefore = m_Velocity;
        m_Velocity = 0;
        yield return new WaitForSeconds(m_playerTimes.m_StuckTime);
        Stuck = false;
        m_Velocity = velocityBefore;
        PararCorrutina("StuckRoutine");
    }
    public void recibirDaño(float Daño)
    {
        if (Invencible)
            return;
        if (Burn) {
            m_HealthController.Damage(Daño);
            m_BurntModifier = Random.Range(10, 31);
            burntDamage = (Daño * m_BurntModifier) / 100;
            m_HealthController.Damage(burntDamage);
        }
        if (Wrath && Paralized)
        {
            Daño += Daño * m_ParalizedLifeModifier;
            m_HealthController.Damage(Daño);
        }
        else if (Wrath)
        {
            Daño += Daño * m_WrathLifeModifier;
            m_HealthController.Damage(Daño);
        }
        else if (Paralized)
        {
            Daño += Daño * m_ParalizedLifeModifier;
            m_HealthController.Damage(Daño);
        }
        else {
            m_HealthController.Damage(Daño);
        }
    }
    private void PararCorrutina(string rutina) {
        StopCoroutine(rutina);
    }
    public void StopStun() {
        Stun = false;
    }
}

 

