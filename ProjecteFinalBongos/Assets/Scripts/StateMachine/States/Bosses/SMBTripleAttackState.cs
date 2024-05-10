using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBTripleAttackState : SMBBasicAttackState
{

    [Header("Attack Animation")]
    [SerializeField]
    private string m_TripleAttackAnimationName;

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
        if (m_TripleAttackAnimationName == String.Empty)
            m_TripleAttackCoroutine = StartCoroutine(AttackCoroutine(transform.position + transform.up, 0.5f, 0.5f, 1f));
        else
            AttackAnimation();
    }

    private void AttackAnimation()
    {
        if (m_TwoDirections)
        {
            m_Animator.Play(m_TripleAttackAnimationName);

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

    private void EndTripleAttackAttack()
    {
        if (!m_Boss.IsPlayerDetected)
        {
            OnAttackStopped?.Invoke(gameObject);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        if (m_TripleAttackCoroutine != null)
            StopCoroutine(m_TripleAttackCoroutine);
    }

    private Coroutine m_TripleAttackCoroutine;
    public IEnumerator AttackCoroutine(Vector2 position, float attack1Delay, float attack2Delay, float attack3Delay)
    {
        while (true)
        {
            m_Rigidbody.velocity = Vector3.zero;
            m_AttackHitbox.transform.position = position;
            Vector2 posicionPlayer = m_Target.position - transform.position;
            float angulo = Mathf.Atan2(posicionPlayer.y, posicionPlayer.x);
            angulo = Mathf.Rad2Deg * angulo - 90;
            transform.localEulerAngles = new Vector3(0, 0, angulo);
            m_AttackHitbox.gameObject.SetActive(true);
            yield return new WaitForSeconds(attack1Delay);
            m_AttackHitbox.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            m_AttackHitbox.gameObject.SetActive(true);
            yield return new WaitForSeconds(attack2Delay);
            m_AttackHitbox.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            m_AttackHitbox.gameObject.SetActive(true);
            yield return new WaitForSeconds(attack3Delay);
            m_AttackHitbox.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            if (!m_Boss.IsPlayerDetected)
                OnStopDetectingPlayer?.Invoke(gameObject);
        }
    }
    private void Update()
    {
        if (derecha)
            transform.localEulerAngles = Vector3.zero;
        else
            transform.localEulerAngles = new Vector3(0, 180, 0);
    }
}
