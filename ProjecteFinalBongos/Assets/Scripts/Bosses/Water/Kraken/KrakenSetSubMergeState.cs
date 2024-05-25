using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class KrakenSetSubMergeState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;
    private bool derecha;
    public bool m_TwoDirections;
    [Header("Set Submerge animation")]
    [SerializeField]
    private string m_SetSubmergeAnimationName;

    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Animator = GetComponent<Animator>();
        m_Boss = GetComponent<BossBehaviour>();
    }

    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true);
        transform.up = Vector3.zero;
        m_Animator.Play(m_SetSubmergeAnimationName);
    }

    public void SetSubmergeMode()
    {
        m_StateMachine.ChangeState<SubMergeState>();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
    private void Update()
    {
        if (derecha)
            transform.localEulerAngles = Vector3.zero;
        else
            transform.localEulerAngles = new Vector3(0, 180, 0);
    }
}
