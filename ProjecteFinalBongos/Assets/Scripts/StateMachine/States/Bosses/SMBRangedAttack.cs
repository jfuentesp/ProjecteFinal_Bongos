using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBRangedAttack : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;

    [Header("Pool of projectiles/splats")]
    [SerializeField]
    private Pool m_Pool;

    private Transform m_Target;

    public delegate void OnStopAttacking(GameObject obj);
    public OnStopAttacking onAttackStopped;

    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
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
        lightning.GetComponent<AlteaBullet>().enabled = true;
        lightning.GetComponent<AlteaBullet>().Init(m_Target.position - transform.position);
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
