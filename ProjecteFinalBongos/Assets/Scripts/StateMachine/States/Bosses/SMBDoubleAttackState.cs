using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBDoubleAttackState : SMBBasicAttackState
{
    [Header("Attack Animation")]
    [SerializeField]
    private string m_DoubleAttackAnimationName;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true);
        m_DoubleAttackCoroutine = StartCoroutine(AttackCoroutine(transform.position + transform.up, 0.5f, 0.5f));
    }

    public override void ExitState()
    {
        base.ExitState();
        if (m_DoubleAttackCoroutine != null)
            StopCoroutine(m_DoubleAttackCoroutine);
    }

    private Coroutine m_DoubleAttackCoroutine;
    public IEnumerator AttackCoroutine(Vector2 position, float attack1Delay, float attack2Delay)
    {
        while (true)
        {
            m_Rigidbody.velocity = Vector3.zero;
            m_AttackHitbox.transform.position = position;
            transform.up = m_Target.position - transform.position;
            m_AttackHitbox.gameObject.SetActive(true);
            yield return new WaitForSeconds(attack1Delay);
            m_AttackHitbox.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            m_AttackHitbox.gameObject.SetActive(true);
            yield return new WaitForSeconds(attack2Delay);
            m_AttackHitbox.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            if (!m_Boss.IsPlayerDetected)
                m_StateMachine.ChangeState<SMBChaseState>();
        }
    }
}
