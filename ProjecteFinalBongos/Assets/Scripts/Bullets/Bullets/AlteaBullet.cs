using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlteaBullet : Bullet
{
    public new void Init(Vector2 direction)
    {
        transform.up = direction;
        m_Size = new Vector2(m_SizeRadius, m_SizeRadius);
        transform.localScale = m_Size;
        m_Rigidbody.velocity = transform.up * m_Speed;
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
                DisableBullet();
    }
}
