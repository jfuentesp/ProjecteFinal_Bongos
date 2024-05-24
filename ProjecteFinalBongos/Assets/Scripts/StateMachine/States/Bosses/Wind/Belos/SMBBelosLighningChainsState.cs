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
    [SerializeField] private string RayoAnimation;

    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        m_Pool = LevelManager.Instance._BulletPool;
    }
    private void Update()
    {

    }
    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true);
        m_Animator.Play(RayoAnimation);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public void ShootTrap()
    {
        GameObject bulletObject = m_Pool.GetElement();
        bulletObject.transform.position = transform.position;
        TrapBullet trap = bulletObject.GetComponent<TrapBullet>();
        trap.enabled = true;
        bulletObject.SetActive(true);
        trap.Init(m_Boss.Target.position - transform.position);
 
    }

    public void Finish() {
        m_StateMachine.ChangeState<SMBChaseState>();
    }
}
