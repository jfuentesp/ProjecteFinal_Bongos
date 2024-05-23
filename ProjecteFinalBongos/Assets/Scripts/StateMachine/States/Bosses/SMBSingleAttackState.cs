using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SMBSingleAttackState : SMBBasicAttackState
{

    [Header("Attack Animation")]
    [SerializeField]
    private string m_SingleAttackAnimationName;
    [SerializeField]
    private string m_WaitAnimation;
    [SerializeField]
    private float minWaitTime;
    [SerializeField]
    private float maxWaitTime;

    public Action<GameObject> OnStopDetectingPlayer;
    public Action<GameObject> OnAttackStopped;
    public Action<GameObject> OnAttackParried;

    private bool derecha;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true);
        StartCoroutine(AttackAnimationRoutine());
    }

    private void AttackAnimation()
    {
        if (m_TwoDirections)
        {
            m_Animator.Play(m_SingleAttackAnimationName);
            if (m_Target != null)
            {
                if (m_Target.position.x - transform.position.x < 0)
                {
                    derecha = false;
                }
                else
                {
                    derecha = true;
                }
            }
        }
    }
    private IEnumerator AttackAnimationRoutine()
    {
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        if (m_TwoDirections)
        {
            m_Animator.Play(m_WaitAnimation);
            if (m_Target != null)
            {
                if (m_Target.position.x - transform.position.x < 0)
                {
                    derecha = false;
                }
                else
                {
                    derecha = true;
                }
            }
        }
        yield return new WaitForSeconds(waitTime);

        if (m_TwoDirections)
        {
            m_Animator.Play(m_SingleAttackAnimationName);
            if (m_Target != null)
            {
                if (m_Target.position.x - transform.position.x < 0)
                {
                    derecha = false;
                }
                else
                {
                    derecha = true;
                }
            }
        }
    }
    private void EndAttack()
    {
        if (!m_Boss.IsPlayerDetected)
        {
            OnAttackStopped?.Invoke(gameObject);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();
    }

    private void Update()
    {
        if (derecha)
            transform.localEulerAngles = Vector3.zero;
        else
            transform.localEulerAngles = new Vector3(0, 180, 0);

    }
}
