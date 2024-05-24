using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KrakenParalizingAttack : SMState
{
    [SerializeField] private string m_ParalizingAnimation;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;
    private Transform m_Target;
    public Action<GameObject> onAttackStopped;
    public Action<GameObject> onAttackDestroyed;
    private NavMeshAgent m_NavMeshAgent;
    private float countAttack = 5;

    private new void Awake()
    {
     
        base.Awake();
     
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        m_Boss.OnPlayerInSala += GetTarget;
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        
    }

    private void GetTarget()
    {
        m_Target = m_Boss.Target;

    }

    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true);
        m_Animator.Play(m_ParalizingAnimation);
    }


    public void Destroyed() {
        onAttackDestroyed?.Invoke(gameObject);
        PJSMB.Instance.PlayerEstadosController.UnStuckFunction();
    }
    public void Finish() {
        PJSMB.Instance.PlayerEstadosController.UnStuckFunction();
        onAttackStopped?.Invoke(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enabled)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHitBox"))
            {
                countAttack--;
                if (countAttack <= 0)
                {
                    Destroyed();
                }
            }
        }
   
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
