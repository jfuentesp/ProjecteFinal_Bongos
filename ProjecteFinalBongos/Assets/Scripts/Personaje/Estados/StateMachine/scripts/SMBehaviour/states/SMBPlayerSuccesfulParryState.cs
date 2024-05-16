using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SMBPlayerSuccesfulParryState : MBState
{
    private PJSMB m_PJ;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private Ability m_parry;


    private void Awake()
    {
        m_PJ = GetComponent<PJSMB>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Animator.speed = 1.0f;

    }

    public override void InitState()
    {
        base.InitState();
        m_PJ.Input.FindActionMap("PlayerActions").FindAction("Parry").performed += Parry;
        if (m_PJ.direccion == 0)
        {
            m_Animator.Play("parriedPose");
        }
        else if (m_PJ.direccion == 1)
        {
            m_Animator.Play("parriedPoseDown");
        }
        else if (m_PJ.direccion == 2)
        {
            m_Animator.Play("parriedPoseUp");
        }
        m_Rigidbody.velocity = Vector2.zero;
        m_parry = m_PJ.PlayerAbilitiesController.Parry;
    }
    public override void ExitState()
    {
        base.ExitState();
        m_PJ.Input.FindActionMap("PlayerActions").FindAction("Parry").performed -= Parry;
    }
    public void Exit()
    {
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
        GetComponent<FloatGameEventListener>().enabled = true;
    }
    private void Parry(InputAction.CallbackContext context)
    {
        m_StateMachine.ChangeState<SMBPlayerParryState>();
    }
}
