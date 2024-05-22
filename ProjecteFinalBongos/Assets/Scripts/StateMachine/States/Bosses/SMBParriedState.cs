using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SMBParriedState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;
    public delegate void OnRecomposition(GameObject obj);
    public OnRecomposition OnRecomposited;

    [Header("Parry duration")]
    [SerializeField] private float m_ParryDuration;

    [Header("Animation Name")]
    [SerializeField] private string m_AnimationParryName;

    private Transform m_Target;
    private bool derecha; 

    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>(); 
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
        if (m_AnimationParryName != String.Empty)
        {
            m_Animator.Play(m_AnimationParryName);
        }
        if (m_Target != null)
        {
            if (m_Target.position.x - transform.position.x < 0)
                derecha = false;
            else
                derecha = true;
        }
        StartCoroutine(ParriedCoroutine());
    }
    private void Update()
    {
        if (derecha)
            transform.localEulerAngles = Vector3.zero;
        else
            transform.localEulerAngles = new Vector3(0, 180, 0);
    }

    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();
    }

    private IEnumerator ParriedCoroutine()
    {
        yield return new WaitForSeconds(m_ParryDuration);
        //m_StateMachine.ChangeState<SMBChaseState>();
        OnRecomposited?.Invoke(gameObject);
    }
}
