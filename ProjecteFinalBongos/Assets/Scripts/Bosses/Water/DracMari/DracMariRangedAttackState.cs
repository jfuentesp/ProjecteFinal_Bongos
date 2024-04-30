using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DracMariRangedAttackState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;

    [Header("Pool of projectiles/splats")]
    [SerializeField]
    private Pool m_Pool;

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
        Disparar();
        m_Rigidbody.velocity = Vector3.zero;
    }
        
    private void Disparar()
    {
        GameObject lightning = m_Pool.GetElement();
        lightning.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        lightning.SetActive(true);
        lightning.GetComponent<DracMBullet>().enabled = true;
        lightning.GetComponent<DracMBullet>().Init(m_Target.position - transform.position);
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