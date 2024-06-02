using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SMBDoubleAttackState : SMBBasicAttackState
{
    [Header("Attack Animation")]
    [SerializeField]
    private string m_DoubleAttackAnimationName;
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
            m_Animator.Play(m_DoubleAttackAnimationName);

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
    private IEnumerator AttackAnimationRoutine()
    {
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        if (m_TwoDirections)
        {
            m_Animator.Play(m_WaitAnimation);
            PlayWaitSound();
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
            m_Animator.Play(m_DoubleAttackAnimationName);
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
    private void EndSecondAttack()
    {
        if (!m_Boss.IsPlayerDetected)
        {
            OnAttackStopped?.Invoke(gameObject);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        if (m_DoubleAttackCoroutine != null)
            StopCoroutine(m_DoubleAttackCoroutine);
    }
    private void Update()
    {
        if (derecha)
            transform.localEulerAngles = Vector3.zero;
        else
            transform.localEulerAngles = new Vector3(0, 180, 0);
    }
    private Coroutine m_DoubleAttackCoroutine;
    public IEnumerator AttackCoroutine(Vector2 position, float attack1Delay, float attack2Delay)
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
            if (!m_Boss.IsPlayerDetected)
                m_StateMachine.ChangeState<SMBChaseState>();
        }
    }
    private void PlayDoubleSlashSound()
    {
        m_AudioSource.clip = m_SlashClipList[Random.Range(0, m_SlashClipList.Length)];
        m_AudioSource.Play();
    }
}
