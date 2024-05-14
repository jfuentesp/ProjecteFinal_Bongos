using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    protected float m_TimeUntilDestroyed;
    [SerializeField]
    protected float m_SizeRadius;
    [SerializeField]
    protected float m_Speed;
    [SerializeField]
    protected float m_Damage;
    [SerializeField]
    protected string m_AnimationName;
    protected Vector2 m_Size;
    protected Rigidbody2D m_Rigidbody;
    protected BossAttackDamage m_AttackDamage;
    protected Animator m_Animator;

    protected void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_AttackDamage = GetComponent<BossAttackDamage>();
    }

    public virtual void Init(Vector2 direction)
    {
        if (m_AnimationName != string.Empty)
            m_Animator.Play(m_AnimationName);
        transform.up = direction;
        m_Size = new Vector2(m_SizeRadius, m_SizeRadius);
        transform.localScale = m_Size;
        m_Rigidbody.velocity = transform.up * m_Speed;
        m_AttackDamage.SetDamage(m_Damage);
        StartCoroutine(ReturnToPoolCoroutine());
    }
    private void Update()
    {

    }
    protected virtual IEnumerator ReturnToPoolCoroutine()
    {
        yield return new WaitForSeconds(m_TimeUntilDestroyed);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled)
            return;
        if (collision.gameObject.CompareTag("MechanicObstacle"))
        {
            StopCoroutine(ReturnToPoolCoroutine());
            DisableBullet();
        }
    }

    protected void DisableBullet()
    {
        Bullet[] comps = gameObject.GetComponents<Bullet>();
        foreach (Bullet comp in comps)
        {
            comp.enabled = false;
        }
        transform.localScale = m_Size;
        this.gameObject.SetActive(false);
    }
}
