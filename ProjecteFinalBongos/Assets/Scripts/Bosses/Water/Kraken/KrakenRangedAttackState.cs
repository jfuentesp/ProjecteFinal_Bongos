using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenRangedAttackState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;
    [Header("Pool of projectiles/splats")]
    [SerializeField]
    private Pool m_Pool;
    [SerializeField]
    private string m_ShootAnimation;
    [SerializeField]
    private Transform m_ShootPoint;
    private Transform m_Target;

    public Action<GameObject> onAttackStopped;

    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        m_Pool = LevelManager.Instance._BulletPool;
        m_Boss.OnPlayerInSala += GetTarget;
    }

    private void GetTarget()
    {
        m_Target = m_Boss.Target;
    }

    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true);
        m_Animator.Play(m_ShootAnimation);       
        m_Rigidbody.velocity = Vector3.zero;
    }
    private void Shoot() {
        GameObject lightning = m_Pool.GetElement();
        lightning.transform.position = new Vector3(m_ShootPoint.position.x, m_ShootPoint.position.y, 0);
        lightning.SetActive(true);
        lightning.GetComponent<TintaBullet>().enabled = true;
        lightning.GetComponent<TintaBullet>().Init(m_Target.position - transform.position);
    }

    private void End() {
        onAttackStopped.Invoke(gameObject);
    }

    public override void ExitState()
    {
        base.ExitState();

    }
    // Update is called once per frame
    void Update()
    {

    }
}
