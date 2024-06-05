using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DracMBullet : Bullet
{
    [SerializeField] private Pool m_pool;
    private bool boss;
    public new void Init(Vector2 direction)
    {
        transform.up = direction;
        m_Size = new Vector2(m_SizeRadius, m_SizeRadius);
        transform.localScale = m_Size;
        m_Rigidbody.velocity = transform.up * m_Speed;
        m_pool = LevelManager.Instance._SplashPool;
        boss = false;
        m_AttackDamage.SetDamage(m_Damage);
        m_AttackDamage.SetEstado(EstadosAlterados.Normal);
        m_AttackDamage.SetTime(0);
        gameObject.tag = "Untagged";
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void MeChoque()
    {
        gameObject.layer = LayerMask.NameToLayer("AllHitBox");
        gameObject.tag = "Bullet";
        DisableBullet();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enabled) {
            if (collision.gameObject.CompareTag("MechanicObstacle"))
            {
                GameObject vaporCrash = m_pool.GetElement();
                vaporCrash.transform.position = transform.position;
                vaporCrash.SetActive(true);
                DracMVaporSplash vaporsplash = vaporCrash.GetComponent<DracMVaporSplash>();
                vaporsplash.enabled = true;
                vaporsplash.Init();
                vaporsplash.ChangeLayer("BossHitBox");
                MeChoque();
            }
            else if (collision.gameObject.CompareTag("Player"))
            {
                if (collision.gameObject.GetComponent<SMBPlayerParryState>().parry)
                {
                    gameObject.tag = "DracPlayerBullet";
                    m_Rigidbody.velocity = -transform.up * (m_Speed * 2);
                    boss = true;
                }
                else
                {
                    GameObject vaporCrash = m_pool.GetElement();
                    vaporCrash.transform.position = transform.position;
                    vaporCrash.SetActive(true);
                    DracMVaporSplash vaporsplash = vaporCrash.GetComponent<DracMVaporSplash>();
                    vaporsplash.enabled = true;
                    vaporsplash.Init();
                    vaporsplash.ChangeLayer("BossHitBox");
                    MeChoque();
                }
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("BossHurtBox") && boss)
            {
                GameObject vaporCrash = m_pool.GetElement();
                vaporCrash.transform.position = transform.position;
                vaporCrash.SetActive(true);
                DracMVaporSplash vaporsplash = vaporCrash.GetComponent<DracMVaporSplash>();
                vaporsplash.enabled = true;
                vaporsplash.Init();
                vaporsplash.ChangeLayer("PlayerHitBox");
                vaporsplash.tag = "DracPlayerSplash";
                MeChoque();
            }
        }

    }
}
