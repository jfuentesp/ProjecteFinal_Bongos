using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SMBGroundHitState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;
    [SerializeField]
    private GameObject m_OndaExpansiva;
    [Header("Chase animation")]
    [SerializeField]
    private string m_AnimationName;

    [SerializeField]
    float m_GroundHitDuration;
    float m_CurrentDuration;
    private new void Awake()
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
        m_CurrentDuration = 0;
        m_Boss.SetBusy(true);
        if (m_AnimationName != String.Empty)
            m_Animator.Play(m_AnimationName);
        m_Rigidbody.velocity = Vector2.zero;
        HitGround();
    }


    public override void ExitState()
    {
        base.ExitState();
    }
    void Update()
    {
        m_CurrentDuration += Time.deltaTime;
        if (m_CurrentDuration >= m_GroundHitDuration)
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        }
    }

    private void HitGround()
    {
        GameObject ondaExpansiva = Instantiate(m_OndaExpansiva);
        ondaExpansiva.transform.position = transform.position;
    }
}
