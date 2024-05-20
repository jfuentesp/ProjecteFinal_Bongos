using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KrakenParalizingAttack : SMState
{
    [SerializeField] private string m_SpinAnimation;
    [SerializeField] private GameObject m_tentacle;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;
    private Transform m_Target;
    public Action<GameObject> onAttackStopped;
    private NavMeshAgent m_NavMeshAgent;

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
        print("tentaculo");
        m_Boss.SetBusy(true);
        m_tentacle.SetActive(true);
        StartCoroutine(aasdad());
    }

    IEnumerator aasdad() { 
        yield return new WaitForSeconds(5f);
        m_tentacle.SetActive(false);
        Finish();
    }
    
    public void Finish() {
       // m_tentacle.GetComponent<ParalazingTentacleBehaviour>().Finish();
        onAttackStopped?.Invoke(gameObject);
    }
  




    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();
    }
}
