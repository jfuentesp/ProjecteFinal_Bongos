using NavMeshPlus.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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
    [SerializeField] private float m_TiempoParaRed;
    private Coroutine m_TiempoSpawneoCoroutine;
    private Coroutine m_DeteccionJugadorCoroutine;
    private Coroutine m_LanzarRedCoroutine;
    private new void Awake()
    {
        base.Awake();
        m_SalaPadre = GetComponentInParent<SalaBoss>();
        GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<TritoArrowSummoningState>().onArrowSummoned = () =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<TritoWaterChainsState>().onChainSummoned = () =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBSingleAttackState>().OnStopDetectingPlayer = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };

        GetComponent<SMBChaseState>().OnStartChase += EmpezarCuentaAtras;
        m_StateMachine.ChangeState<SMBIdleState>();
    }


    private void EmpezarCuentaAtras()
    {
        if (m_DeteccionJugadorCoroutine == null)
        {
            m_DeteccionJugadorCoroutine = StartCoroutine(PlayerDetectionCoroutine());
        }
        if (m_TiempoSpawneoCoroutine == null)
        {
            m_TiempoSpawneoCoroutine = StartCoroutine(TiempoSpawneo());
        }
        if (m_LanzarRedCoroutine == null)
        {
            m_LanzarRedCoroutine = StartCoroutine(TiempoLanzarRed());
        }
        else
        {
            print("NULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULONULO ");
            //m_LanzarRedCoroutine = StartCoroutine(TiempoLanzarRed());
        }


    }

    private IEnumerator TiempoLanzarRed()
    {
        print("LANZATE PEDAZO DE PUTA");
        yield return new WaitForSeconds(m_TiempoParaRed);
        m_StateMachine.ChangeState<TritoWaterChainsState>();
    }

    private IEnumerator TiempoSpawneo()
    {
        while (m_IsAlive)
        {
            yield return new WaitForSeconds(m_TiempoEntreSpawn);
            m_StateMachine.ChangeState<TritoArrowSummoningState>();
        }
    }

    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        OnPlayerInSala?.Invoke();
    }
    private IEnumerator PlayerDetectionCoroutine()
    {
        while (m_IsAlive)
        {
            if (m_PlayerAttackDetectionAreaType == CollisionType.CIRCLE)
            {
                RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position, m_AreaRadius, transform.position, m_AreaRadius, m_LayersToCheck);
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("Player") && !m_IsBusy)
                {
                    m_IsPlayerDetected = true;
                    SetAttack();
                }
                else
                {
                    m_IsPlayerDetected = false;
                }
            }
            else
            {
                RaycastHit2D hitInfo = Physics2D.BoxCast(transform.position, m_BoxArea, transform.rotation.z, transform.position);
                if (hitInfo.collider.CompareTag("Player") && !m_IsBusy)
                {
                    m_IsPlayerDetected = true;
                    SetAttack();
                }
                else
                {
                    m_IsPlayerDetected = false;
                }
            }
            yield return new WaitForSeconds(m_CheckingPlayerTimelapse);
        }
    }
    private void SetAttack()
    {

        StopCoroutine(m_LanzarRedCoroutine);
        m_LanzarRedCoroutine = null;
        float rng = Random.value;

        switch (rng)
        {
            case < 0.5f:
                m_StateMachine.ChangeState<SMBSingleAttackState>();
                break;
            case < 0.65f:
                m_StateMachine.ChangeState<SMBDoubleAttackState>();
                break;
            case > 0.8f:
                m_StateMachine.ChangeState<SMBTripleAttackState>();
                break;
        }
    }

}
