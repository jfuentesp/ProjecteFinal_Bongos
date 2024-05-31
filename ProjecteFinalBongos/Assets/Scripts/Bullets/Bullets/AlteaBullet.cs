using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlteaBullet : Bullet
{
    private bool m_Devuelta;
    public new void Init(Vector2 direction)
    {
        gameObject.layer = LayerMask.NameToLayer("BossHitBox");
        if (m_AnimationName != string.Empty)
            m_Animator.Play(m_AnimationName);
        transform.up = direction;
        m_Devuelta = false;
        m_Size = new Vector2(m_SizeRadius, m_SizeRadius);
        m_AttackDamage.SetDamage(m_Damage);
        m_AttackDamage.SetEstado(m_EstadoAlterado);
        m_AttackDamage.SetTime(m_Time);
        transform.localScale = m_Size;
        m_Rigidbody.velocity = transform.up * m_Speed;
        if (m_AudioBullet)
        {
            m_AudioSource.clip = m_AudioBullet;
            m_AudioSource.Play();
        }
        StartCoroutine(ReturnToPoolCoroutine());
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
                            m_Devuelta = true;
                            transform.up = -transform.up;
                            m_Rigidbody.velocity = transform.up * m_Speed;
                            gameObject.layer = LayerMask.NameToLayer("AllHitBox");
                        }
                        else
                        {
                            collision.gameObject.GetComponent<PJSMB>().recibirDamage(m_Damage);
                            gameObject.layer = LayerMask.NameToLayer("AllHitBox");
                            DisableBullet();
                        }
                    }
        }
        else
        {
            m_Devuelta = false;
            if (collision.gameObject.layer == LayerMask.NameToLayer("BossHurtBox"))
            {
                collision.gameObject.GetComponent<BossBehaviour>().recibirDaño(m_Damage);
                gameObject.layer = LayerMask.NameToLayer("AllHitBox");
                DisableBullet();
            }
        }
    }
}
