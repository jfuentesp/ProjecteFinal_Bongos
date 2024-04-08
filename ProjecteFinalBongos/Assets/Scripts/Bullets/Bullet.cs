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
    protected Vector2 m_Size;
    protected Rigidbody2D m_Rigidbody;

    protected void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    public virtual void Init(Vector2 direction)
    {
        transform.up = direction;
        m_Size = new Vector2(m_SizeRadius, m_SizeRadius);
        transform.localScale = m_Size;
        m_Rigidbody.velocity = transform.up * m_Speed;
        StartCoroutine(ReturnToPoolCoroutine());
    }

    protected virtual IEnumerator ReturnToPoolCoroutine()
    {
        yield return new WaitForSeconds(m_TimeUntilDestroyed);
        this.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MechanicObstacle"))
        {
            StopCoroutine(ReturnToPoolCoroutine());
            this.gameObject.SetActive(false);
        }
    }
}
