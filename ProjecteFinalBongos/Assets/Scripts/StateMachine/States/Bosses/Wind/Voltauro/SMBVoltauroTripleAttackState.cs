using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBVoltauroTripleAttackState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;

    [Header("Attack detection area settings")]
    [SerializeField]
    private Collider2D m_AttackArea;
    [Header("If the area is a rectangle collider")]
    [SerializeField]
    private float m_AreaLength;
    [SerializeField]
    private float m_AreaWideness;
    [Header("If the area is a circle collider")]
    [SerializeField]
    private float m_AreaRadius;

    [Header("Attack hitbox settings")]
    [SerializeField]
    private Collider2D m_AttackHitbox;
    [Header("If the hitbox is a rectangle collider")]
    [SerializeField]
    private float m_HitboxLength;
    [SerializeField]
    private float m_HitboxWideness;
    [Header("If the hitbox is a circle collider")]
    [SerializeField]
    private float m_HitboxRadius;

    [Header("Target gameObject")]
    [SerializeField]
    private GameObject m_Target;

    [Header("Attack Animation")]
    [SerializeField]
    private string m_VoltauroTripleAttackAnimationName;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
    }

    public override void InitState()
    {
        base.InitState();
        m_TripleAttackCoroutine = StartCoroutine(AttackCoroutine(transform.position + transform.up, 0.5f, 0.5f, 1f));
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
            transform.up = m_Target.transform.position - transform.position;
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
        }
    }
}
