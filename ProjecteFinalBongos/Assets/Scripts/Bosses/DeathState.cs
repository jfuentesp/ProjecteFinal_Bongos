using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeathState : SMState
{
    [Header("Nombre Animacion")]
    [SerializeField] private string m_AnimationName;

    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;
    private NavMeshAgent m_NavMesh;

    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        m_NavMesh = GetComponent<NavMeshAgent>();
        //Initialize(); //Initializes parameters on a given bullet
    }
    public override void InitState()
    {
        base.InitState();
        m_NavMesh.velocity = Vector3.zero;
        m_Boss.SetBusy(true);
        m_Animator.Play(m_AnimationName);
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
