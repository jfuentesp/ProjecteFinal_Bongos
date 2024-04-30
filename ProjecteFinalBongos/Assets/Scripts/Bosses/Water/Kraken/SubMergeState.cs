using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SubMergeState : SMState
{

    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;
    private Transform m_Target;
    [SerializeField]
    private LayerMask m_LayerMask;
    [SerializeField]
    private float m_SubmergeSpeed;
    [SerializeField]
    private float m_DirectionTime;
    [SerializeField]
    private int Steps = 5;
    [SerializeField]
    private string m_SubmergeAnimationName;
    private NavMeshAgent m_NavmeshAgent;

    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        m_Boss.OnPlayerInSala += GetTarget;
        m_NavmeshAgent = GetComponent<NavMeshAgent>();
    }

    private void GetTarget()
    {
        m_Target = m_Boss.Target;
    }

    private void RandomWalk()
    {
        
        Vector3 direccion = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direccion, 5, m_LayerMask);
        if (hit.collider != null)
        {

            print("Me choco");
            RandomWalk();
        }
        else
        {

            m_NavmeshAgent.velocity = direccion.normalized * m_SubmergeSpeed;
           


        }
    }
    private IEnumerator Merge() {
        while (Steps > 0) {
            Steps--;
            yield return new WaitForSeconds(1f);
            if (Steps <= 0)
            {
                m_StateMachine.ChangeState<KrakenMergeState>();
            }
        }
    
    }
    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true);
        Steps = 20;
        m_Animator.Play(m_SubmergeAnimationName);
        StartCoroutine(Merge());
    }
    private float m_TimeChangeDirection = 0;
    private void FixedUpdate()
    {
        m_TimeChangeDirection += Time.fixedDeltaTime;
        if (m_TimeChangeDirection > m_DirectionTime)
        {
            RandomWalk();
            m_TimeChangeDirection = 0;
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();
    }

}
