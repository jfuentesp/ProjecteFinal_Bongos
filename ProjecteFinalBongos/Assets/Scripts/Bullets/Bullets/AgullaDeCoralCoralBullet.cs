using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgullaDeCoralCoralBullet : Bullet
{
    private Transform m_BossTransform;
    [SerializeField] private float m_TimeUntilReturn;
    [SerializeField] private float m_ReturnSpeed;
    public float Damage => m_Damage;
    private bool m_Returning;
    public void Init(Vector2 destino, Transform _BossTransform, float _Damage)
    {
        m_BossTransform = _BossTransform;
        m_Damage = _Damage; 

        m_Size = new Vector2(m_SizeRadius, m_SizeRadius);
        transform.localScale = m_Size;
        m_Returning = true;
        StartCoroutine(MoveToPosition(destino));
    }

    private IEnumerator MoveToPosition(Vector2 destino)
    {
        while(Vector2.Distance(transform.position, destino) > 0.1f)
        {
            Vector2 nuevaPosicion = Vector2.Lerp(transform.position, destino, m_Speed * Time.deltaTime);

            yield return new WaitForFixedUpdate();
            transform.position = nuevaPosicion;
        }
        transform.position = destino;
        GetComponent<CircleCollider2D>().enabled = true;
        StartCoroutine(WaitToMove());
    }

    private IEnumerator WaitToMove()
    {
        m_Returning = false;
        yield return new WaitForSeconds(m_TimeUntilReturn);
        m_Returning = true;
        while (Vector2.Distance(transform.position, m_BossTransform.position) > 0.5f)
        {
            m_Rigidbody.velocity = (m_BossTransform.position - transform.position).normalized * m_ReturnSpeed;
            yield return new WaitForSeconds(0.2f);
        }
        transform.position = m_BossTransform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled)
            return;

        if(collision.gameObject.layer == LayerMask.NameToLayer("PlayerHitBox") && !m_Returning)
        {
            DisableBullet();
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("BossHurtBox"))
        {
            DisableBullet();
        }
    }
}
