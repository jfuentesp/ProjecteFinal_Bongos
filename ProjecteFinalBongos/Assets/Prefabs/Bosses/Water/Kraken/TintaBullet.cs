using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TintaBullet : Bullet
{
    [SerializeField] private GameEvent TintaEvent;
 
    public new void Init(Vector2 direction)
    {
        transform.up = direction;
        m_Size = new Vector2(m_SizeRadius, m_SizeRadius);
        transform.localScale = m_Size;
        m_Rigidbody.velocity = transform.up * m_Speed;
        GetComponent<SpriteRenderer>().color = Color.black;
       
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enabled) {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (collision.gameObject.GetComponent<SMBPlayerParryState>().parry)
                {
                    DisableBullet();

                }
                else
                {
                    LevelManager.Instance.GUIBossManager.SpawnTinta();
                    DisableBullet();
                }

            }
            else if (collision.gameObject.CompareTag("MechanicObstacle"))
            {
                DisableBullet();
            }
        }

     
    }
}
