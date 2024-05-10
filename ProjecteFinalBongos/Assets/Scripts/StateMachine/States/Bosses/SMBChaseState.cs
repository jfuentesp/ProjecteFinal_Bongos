using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SMBChaseState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;

    [Header("Chase speed")]
    [SerializeField]
    private float m_ChaseSpeed;

    private Transform m_Target;

    [Header("Chase animation")]
    [SerializeField]
    private string m_ChaseAnimationName;

    private NavMeshAgent m_NavMeshAgent;

    public Action OnStartChase;

    [SerializeField] private bool m_TwoDirections;

    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        m_NavMeshAgent= GetComponent<NavMeshAgent>();
        m_Boss.OnPlayerInSala += GetTarget;
    }

    private void GetTarget()
    {
        m_Target = m_Boss.Target;
    }

    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(false);
        m_Boss.SetAttackingHurtBox(false);
        m_NavMeshAgent.isStopped = false;
        OnStartChase?.Invoke();
        if(m_ChaseAnimationName != String.Empty)
            m_Animator.Play(m_ChaseAnimationName);
    }

    public override void ExitState()
    {
        base.ExitState();
        m_NavMeshAgent.isStopped = true;
        m_NavMeshAgent.ResetPath();
    }

    private void Update()
    {
      
        //To face the target
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
            /*m_Rigidbody.velocity = Vector3.zero;
            Vector3 direction = (m_Target.position - transform.position).normalized;
            m_Rigidbody.velocity = direction * m_ChaseSpeed;*/
        }
    }
}
