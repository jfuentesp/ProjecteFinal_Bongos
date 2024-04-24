using NavMeshPlus.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentOverride2d))]
[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBChargeState))]
[RequireComponent(typeof(AgullaDeCoralFlipState))]
[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(SMBTripleAttackState))]
[RequireComponent(typeof(SMBSingleAttackState))]
[RequireComponent(typeof(SMBDoubleAttackState))]
[RequireComponent(typeof(HealthController))]
public class AgullesDeCoralBossBehaviour : BossBehaviour
{
    private Pool m_Pool;
    private new void Awake()
    {
        base.Awake();
        m_SalaPadre = GetComponentInParent<SalaBoss>();
        /*GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
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
        };*/

        m_Pool = LevelManager.Instance._BulletPool;

        m_StateMachine.ChangeState<SMBIdleState>();
    }

    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        OnPlayerInSala?.Invoke();
    }
    protected new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHitBox"))
        {

            recibirDaño(collision.gameObject.GetComponent<AttackDamage>().Damage);
            SoltarCoral(collision.gameObject.GetComponent<AttackDamage>().Damage);
        }
    }

    private void SoltarCoral(float damage)
    {
        print("SoltandoCoral");
        Vector2 posicionHuevo = m_SalaPadre.GetPosicionAleatoriaEnSala();
        if (m_Target != null)
        {
            GameObject arrow = m_Pool.GetElement();
            arrow.transform.position = transform.position;
            arrow.SetActive(true);
            arrow.GetComponent<AgullaDeCoralCoralBullet>().enabled = true;
            arrow.GetComponent<AgullaDeCoralCoralBullet>().Init(posicionHuevo, transform);
        }

    }
}
