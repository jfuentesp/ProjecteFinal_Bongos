using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DracMBullet : Bullet
{
    [SerializeField] private Pool m_pool;
    private bool boss;
    [SerializeField] private float m_damage;
    public float Damage => m_damage;
    public new void Init(Vector2 direction)
    {
        transform.up = direction;
        m_Size = new Vector2(m_SizeRadius, m_SizeRadius);
        transform.localScale = m_Size;
        m_Rigidbody.velocity = transform.up * m_Speed;
        m_pool = LevelManager.Instance._SplashPool;
        boss = false;
        gameObject.tag = "Untagged";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!enabled)
            return;
        if (collision.gameObject.CompareTag("MechanicObstacle"))
        {
            GameObject vaporCrash = m_pool.GetElement();
            vaporCrash.transform.position = transform.position;
            vaporCrash.SetActive(true);
            DracMVaporSplash vaporsplash = vaporCrash.GetComponent<DracMVaporSplash>();
            vaporsplash.enabled = true;
            vaporCrash.GetComponent<DracMVaporSplash>().Init();
            DisableBullet();
        }
        else if (collision.gameObject.CompareTag("Player")) {
            if (collision.gameObject.GetComponent<SMBPlayerParryState>().parry)
            {
                gameObject.tag = "DracPlayerBullet";
                m_Rigidbody.velocity = -transform.up * (m_Speed * 2);
                boss = true;
            }
            else {
                collision.GetComponent<PJSMB>().recibirDaño(m_damage);
                GameObject vaporCrash = m_pool.GetElement();
                vaporCrash.transform.position = transform.position;
                vaporCrash.SetActive(true);
                DracMVaporSplash vaporsplash = vaporCrash.GetComponent<DracMVaporSplash>();
                vaporsplash.enabled = true;
                vaporCrash.GetComponent<DracMVaporSplash>().Init();
                DisableBullet();    
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("BossHurtBox") && boss) {
            GameObject vaporCrash = m_pool.GetElement();
            vaporCrash.transform.position = transform.position;
            vaporCrash.SetActive(true);
            DracMVaporSplash vaporsplash = vaporCrash.GetComponent<DracMVaporSplash>();
            vaporsplash.enabled = true;
            vaporCrash.GetComponent<DracMVaporSplash>().Init();
            vaporCrash.tag = "DracPlayerSplash";
            DisableBullet();
        }
    }
}
