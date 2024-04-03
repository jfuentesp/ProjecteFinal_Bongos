using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBParriedState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;

    [Header("Parry duration")]
    [SerializeField]
    private float m_ParryDuration;

    public override void InitState()
    {
        base.InitState();
        StartCoroutine(ParriedCoroutine());
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    private IEnumerator ParriedCoroutine()
    {
        yield return new WaitForSeconds(m_ParryDuration);
    }


}
