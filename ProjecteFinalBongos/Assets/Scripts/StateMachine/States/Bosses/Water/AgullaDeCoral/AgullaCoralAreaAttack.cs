using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AgullaCoralAreaAttack : SMBBasicAttackState
{

    [Header("Attack Animation")]
    [SerializeField]
    private string m_AgullaCoralAreaAttackAnimationName;
    [SerializeField]
    private string m_WaitAnimation;
    [SerializeField]
    private float minWaitTime;
    [SerializeField]
    private float maxWaitTime;

    private bool derecha;

    [SerializeField]
    private float m_ForceOfHit;
    
    public Action<GameObject> OnStopDetectingPlayer;
    public Action<GameObject> OnPlayerHitted;
    public Action<GameObject> OnParriedAttack;

    private Coroutine m_SingleAttackCoroutine;
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

    public override void ExitState()
    {
        base.ExitState();
        if (m_SingleAttackCoroutine != null)
            StopCoroutine(m_SingleAttackCoroutine);
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
            m_Animator.Play(m_AgullaCoralAreaAttackAnimationName);
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
    private void Update()
    {
        if (derecha)
            transform.localEulerAngles = Vector3.zero;
        else
            transform.localEulerAngles = new Vector3(0, 180, 0);

    }
    private void EndAttack()
    {
        if (!m_Boss.IsPlayerDetected)
        {
            OnStopDetectingPlayer?.Invoke(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (!collision.gameObject.GetComponent<SMBPlayerParryState>().parry)
            {
                if (collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D target))
                    target.AddForce((m_Target.position - transform.position).normalized * m_ForceOfHit, ForceMode2D.Impulse);
                OnPlayerHitted?.Invoke(gameObject);
            }
        }
    }
}
