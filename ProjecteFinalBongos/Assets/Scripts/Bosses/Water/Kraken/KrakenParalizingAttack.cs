using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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
    [SerializeField]
    private string m_WaitAnimation;
    [SerializeField]
    private float minWaitTime;
    [SerializeField]
    private float maxWaitTime;
    private bool derecha;
    public bool m_TwoDirections;

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
        StartCoroutine(AttackAnimationRoutine());
    }
    private IEnumerator AttackAnimationRoutine()
    {
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        if (m_TwoDirections)
        {
            m_Animator.Play(m_WaitAnimation);
            if (m_Target != null)
            {
                if (m_Target.position.x - transform.position.x < 0)
                {
                    derecha = false;
                }
                else
                {
                    derecha = true;
                }
            }
        }
        yield return new WaitForSeconds(waitTime);

        if (m_TwoDirections)
        {
            m_Animator.Play(m_ParalizingAnimation);
            if (m_Target != null)
            {
                if (m_Target.position.x - transform.position.x < 0)
                {
                    derecha = false;
                }
                else
                {
                    derecha = true;
                }
            }
        }
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
        StopAllCoroutines();
    }
    private void Update()
    {
        if (derecha)
            transform.localEulerAngles = Vector3.zero;
        else
            transform.localEulerAngles = new Vector3(0, 180, 0);

    }
}
