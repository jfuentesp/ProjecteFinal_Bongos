using NavMeshPlus.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentOverride2d))]
[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(TritoArrowSummoningState))]
[RequireComponent(typeof(TritoWaterChainsState))]
[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(SMBTripleAttackState))]
[RequireComponent(typeof(SMBSingleAttackState))]
[RequireComponent(typeof(SMBDoubleAttackState))]
[RequireComponent(typeof(HealthController))]
public class TritoBossBehaviour : BossBehaviour
{
    [SerializeField] private float m_TiempoEntreSpawn;
    private new void Awake()
    {
        base.Awake();
        m_SalaPadre = GetComponentInParent<SalaBoss>();
        GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<TritoArrowSummoningState>();
        };
        GetComponent<TritoArrowSummoningState>().onArrowSummoned = () =>
        {
            m_StateMachine.ChangeState<SMBIdleState>();
        };
        GetComponent<TritoArrowSummoningState>().onArrowSummoned += EmpezarCuentaAtras;
        m_StateMachine.ChangeState<SMBIdleState>();
    }
    private void Start()
    {
    }
    private void EmpezarCuentaAtras()
    {
        StartCoroutine(TiempoSpawneo());
    }

    private IEnumerator TiempoSpawneo()
    {
        yield return new WaitForSeconds(m_TiempoEntreSpawn);
        m_StateMachine.ChangeState<TritoArrowSummoningState>();
    }

    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        OnPlayerInSala?.Invoke();
    }


}
