using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SMBHarpyChaseState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;

    private Transform m_Target;

    [Header("Flying animation")]
    [SerializeField]
    private string m_FlyingHarpyAnimationname;

    [Header("Flying speed")]
    [SerializeField]
    private float m_FlyingSpeed;

    [Header("GameEvent to call on death")]
   
    private NavMeshAgent m_NavMeshAgent;

    [SerializeField] private bool m_TwoDirections;


    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Animator = GetComponent<Animator>();
        m_Boss = GetComponent<BossBehaviour>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_Boss.OnPlayerInSala += SetTarget;
    }

    private void SetTarget()
    {
        m_Target = m_Boss.Target;
    }

    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(false);
        m_NavMeshAgent.acceleration = m_FlyingSpeed;
        m_NavMeshAgent.speed = m_FlyingSpeed;
        m_Animator.Play(m_FlyingHarpyAnimationname);
        
        //Esto en realidad tendrá que ir cuando el bicho muera
        
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    private void Update()
    {
        if (m_Target != null)
        {
            if (m_TwoDirections)
            {
                if (m_Target.position.x - transform.position.x < 0)
                    transform.localEulerAngles = new Vector3(0, 180, 0);
                else
                    transform.localEulerAngles = Vector3.zero;
            }
            else
            {

            }
            /*Vector2 posicionPlayer = m_Target.position - transform.position;
            float angulo = Mathf.Atan2(posicionPlayer.y, posicionPlayer.x);
            angulo = Mathf.Rad2Deg * angulo - 90;
            transform.localEulerAngles = new Vector3(0, 0, angulo);*/
        }
    }

    private void FixedUpdate()
    {
        if (m_Target != null)
        {
            m_NavMeshAgent.SetDestination(m_Target.position);
        }
        else
        {
            //Debug.LogError("No target");
        }
    }
}
