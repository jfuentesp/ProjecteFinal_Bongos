using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ComboHandler))]
public abstract class SMBComboState : SMState
{
    protected PJSMB m_PJ;
    protected Rigidbody2D m_Rigidbody;
    protected Animator m_Animator;
    protected FiniteStateMachine m_StateMachine;
    private ComboHandler m_ComboHandler;
    protected float Strength;
    [SerializeField]
    protected float m_Damage;
    private AudioSource m_AudioSource;
    [SerializeField]
    private AudioClip[] m_AudioClips;
    private new void Awake()
    {
        base.Awake();
        m_PJ = GetComponent<PJSMB>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_ComboHandler = GetComponent<ComboHandler>();
        Strength = 10f;
        m_AudioSource = GetComponent<AudioSource>();

    }

    public override void InitState()
    {
        base.InitState();
        m_PJ.Input.FindActionMap("PlayerActions").FindAction("Attack1").performed += OnAttack1;
        m_PJ.Input.FindActionMap("PlayerActions").FindAction("Attack2").performed += OnAttack2;
        m_PJ.Input.FindActionMap("PlayerActions").FindAction("Parry").performed += Parry;
        m_Rigidbody.velocity = Vector2.zero;
        m_ComboHandler.enabled = true;
        m_ComboHandler.OnEndAction += OnEndAction;
        if(m_AudioClips != null)
        {
            m_AudioSource.clip = m_AudioClips[Random.Range(0, m_AudioClips.Length)];
            m_AudioSource.Play();
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        m_ComboHandler.enabled = false;
        if (m_PJ.Input != null) {
            m_PJ.Input.FindActionMap("PlayerActions").FindAction("Attack1").performed -= OnAttack1;
            m_PJ.Input.FindActionMap("PlayerActions").FindAction("Attack2").performed -= OnAttack2;
            m_PJ.Input.FindActionMap("PlayerActions").FindAction("Parry").performed -= Parry;
        }
        m_ComboHandler.OnEndAction -= OnEndAction;
    }
    private void Parry(InputAction.CallbackContext context)
    {
        m_StateMachine.ChangeState<SMBPlayerParryState>();
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
    protected virtual void SetDamage()
    {
        float damageFinal = m_Damage+((m_PJ.PlayerStatsController.m_Strength * Random.Range(50,101))/100);
        GetComponentInChildren<AttackDamage>().ChangeAttack(damageFinal);
    }
    protected abstract void OnComboSuccessAction();
    protected abstract void OnComboSuccessActionAttack2();
    protected abstract void OnComboFailedAction(); 
    protected abstract void OnEndAction();
}

