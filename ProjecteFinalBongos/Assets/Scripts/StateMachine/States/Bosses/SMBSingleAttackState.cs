using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SMBSingleAttackState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;

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
    private string m_SingleAttackAnimationName;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();

        //Checking what kind of Collider we have set on the hitbox and giving it the required size
        if(m_AttackHitbox is BoxCollider2D) 
        {
            SetBoxColliderSize(m_AttackHitbox);
        }
        if(m_AttackHitbox is  CircleCollider2D)
        {
            SetCircleColliderSize(m_AttackHitbox);
        }
    }


    public override void InitState()
    {
        base.InitState();
        m_SingleAttackCoroutine = StartCoroutine(AttackCoroutine(transform.position + transform.up, 1f));
    }

    public override void ExitState()
    {
        base.ExitState();
        if(m_SingleAttackCoroutine != null)
            StopCoroutine(m_SingleAttackCoroutine);
    }

    private Coroutine m_SingleAttackCoroutine;
    public IEnumerator AttackCoroutine(Vector2 position, float attackDelay)
    {
        while(true)
        { 
            m_Rigidbody.velocity = Vector3.zero;
            m_AttackHitbox.transform.position = position;
            transform.up = m_Target.transform.position - transform.position;
            m_AttackHitbox.gameObject.SetActive(true);
            yield return new WaitForSeconds(attackDelay);
            m_AttackHitbox.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            if (!m_Boss.IsPlayerDetected)
                m_StateMachine.ChangeState<SMBChaseState>();
        }
    }

    public void SetCircleColliderSize(Collider2D collider)
    {
        CircleCollider2D circle;
        collider.TryGetComponent<CircleCollider2D>(out circle);
        circle.radius = m_HitboxRadius;
    }

    public void SetBoxColliderSize(Collider2D collider)
    {
        BoxCollider2D box;
        collider.TryGetComponent<BoxCollider2D>(out box);
        box.size = new Vector2(m_HitboxWideness, m_HitboxLength);
    }
}
