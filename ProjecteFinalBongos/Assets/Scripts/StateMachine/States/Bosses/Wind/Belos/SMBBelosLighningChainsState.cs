using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBBelosLighningChainsState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;

    [SerializeField]
    private Pool m_Pool;

    [Header("Projectile speed")]
    [SerializeField]
    private float m_ChainSpeed;

    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
    }

    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true);
        ShootTrap();
    }

    public override void ExitState()
    {
        base.ExitState();

    }

    private void ShootTrap()
    {
        GameObject bulletObject = m_Pool.GetElement();
        TrapBullet trap = bulletObject.GetComponent<TrapBullet>();
        trap.Init(m_Boss.Target.position);
        m_StateMachine.ChangeState<SMBChaseState>();
    }
}
