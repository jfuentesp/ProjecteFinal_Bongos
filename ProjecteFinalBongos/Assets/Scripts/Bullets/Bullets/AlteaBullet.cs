using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlteaBullet : Bullet
{
    private bool m_Devuelta;
    public new void Init(Vector2 direction)
    {
        transform.up = direction;
        m_Devuelta = false;
        m_Size = new Vector2(m_SizeRadius, m_SizeRadius);
        m_AttackDamage.SetDamage(m_Damage);
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

        if (!m_Devuelta)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
                if (collision.CompareTag("Player"))
                    if (collision.gameObject.TryGetComponent<SMBPlayerParryState>(out SMBPlayerParryState parry))
                    {
                        if (parry.parry)
                        {
                            print("Me parrearon");
                            m_Devuelta = true;
                            gameObject.layer = LayerMask.NameToLayer("PlayerHitBox");
                            m_Rigidbody.velocity = transform.up * -m_Speed;
                        }
                        else
                        {
                            print("No me parrearon");
                            DisableBullet();
                        }
                    }
            if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
                if (collision.CompareTag("MechanicObstacle"))
                {
                    print("Pared");
                    gameObject.layer = LayerMask.NameToLayer("BossHitBox");
                    DisableBullet();
                }   
        }
        else
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
                if (collision.CompareTag("MechanicObstacle"))
                {
                    print("Pared");
                    DisableBullet();
                }
            if (collision.gameObject.layer == LayerMask.NameToLayer("BossHurtBox"))
            {
                print("Golpee al boss");
                collision.gameObject.GetComponent<BossBehaviour>().recibirDaño(m_Damage);
                gameObject.layer = LayerMask.NameToLayer("BossHitBox");
                DisableBullet();
            }
        }
    }
}
