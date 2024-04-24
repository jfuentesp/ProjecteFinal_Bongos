using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgullaCoralAreaAttack : SMBBasicAttackState
{

    [Header("Attack Animation")]
    [SerializeField]
    private string m_AgullaCoralAreaAttackAnimationName;

    public Action OnStopDetectingPlayer;
    public Action OnPlayerHitted;
    public Action OnParriedAttack;

    private Coroutine m_SingleAttackCoroutine;

    [SerializeField]
    private float m_ForceOfHit;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true);
        m_SingleAttackCoroutine = StartCoroutine(AttackCoroutine(transform.position + transform.up, 1f));
    }

    public override void ExitState()
    {
        base.ExitState();
        if (m_SingleAttackCoroutine != null)
            StopCoroutine(m_SingleAttackCoroutine);
    }

    public IEnumerator AttackCoroutine(Vector2 position, float attackDelay)
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
            yield return new WaitForSeconds(attackDelay);
            m_AttackHitbox.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            if (!m_Boss.IsPlayerDetected)
            {
                OnStopDetectingPlayer?.Invoke();
            }
            //m_StateMachine.ChangeState<SMBChaseState>();
        }
    }
    private void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<SMBPlayerParryState>().parry)
            {
                OnParriedAttack?.Invoke();
            }
            else
            {
                OnPlayerHitted?.Invoke();
                Rigidbody2D target;
                collision.gameObject.TryGetComponent<Rigidbody2D>(out target);
                if (target != null)
                    target.AddForce(transform.up * m_ForceOfHit, ForceMode2D.Impulse);
            }
        }
    }
}
