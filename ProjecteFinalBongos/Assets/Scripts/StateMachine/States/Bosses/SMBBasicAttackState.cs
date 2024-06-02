using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBBasicAttackState : SMState
{
    protected Rigidbody2D m_Rigidbody;
    protected FiniteStateMachine m_StateMachine;
    protected Animator m_Animator;
    protected BossBehaviour m_Boss;

    [Header("Attack hitbox settings")]
    [SerializeField]
    protected Collider2D m_AttackHitbox;
    [Header("If the hitbox is a rectangle collider")]
    [SerializeField]
    protected float m_HitboxLength;
    [SerializeField]
    protected float m_HitboxWideness;
    [Header("If the hitbox is a circle collider")]
    [SerializeField]
    protected float m_HitboxRadius;

    [Header("Animation Two Directions")]
    [SerializeField] protected bool m_TwoDirections;

    [Header("Sounds")]
    protected AudioSource m_AudioSource;
    [SerializeField] protected AudioClip m_WaitClip;
    [SerializeField] protected AudioClip[] m_SlashClipList;

    protected Transform m_Target;

    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_AudioSource = GetComponent<AudioSource>();
        m_Boss = GetComponent<BossBehaviour>();
        m_Boss.OnPlayerInSala += GetTarget;
        /*
        //Checking what kind of Collider we have set on the hitbox and giving it the required size
        if (m_AttackHitbox is BoxCollider2D)
        {
            SetBoxColliderSize(m_AttackHitbox);
        }
        if (m_AttackHitbox is CircleCollider2D)
        {
            SetCircleColliderSize(m_AttackHitbox);
        }*/
    }
    private void GetTarget()
    {
        m_Target = m_Boss.Target;
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

    protected void PlayWaitSound()
    {
        m_AudioSource.clip = m_WaitClip;
        m_AudioSource.Play();
    }

    protected void PlaySlashSound()
    {
        m_AudioSource.clip = m_SlashClipList[Random.Range(0, m_SlashClipList.Length)];
        m_AudioSource.Play();
    }
}
