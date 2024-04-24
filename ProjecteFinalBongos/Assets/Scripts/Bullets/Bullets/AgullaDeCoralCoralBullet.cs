using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgullaDeCoralCoralBullet : Bullet
{
    private Transform m_BossTransform;
    [SerializeField] private float m_TimeUntilReturn;
    public void Init(Vector2 destino, Transform _BossTransform)
    {
        m_BossTransform = _BossTransform;

        m_Size = new Vector2(m_SizeRadius, m_SizeRadius);
        transform.localScale = m_Size;
        
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
        StartCoroutine(WaitToMove());
    }

    private IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds(m_TimeUntilReturn);
        while(Vector2.Distance(transform.position, m_BossTransform.position) > 0.5f)
        {
            m_Rigidbody.velocity = (m_BossTransform.position - transform.position).normalized * m_Speed;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Galileo");
    }
}
