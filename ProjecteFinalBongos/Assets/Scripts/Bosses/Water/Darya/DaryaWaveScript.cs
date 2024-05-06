using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DaryaWaveScript : MonoBehaviour
{
    private Transform m_Boss;
    private Rigidbody2D m_RigidBody;
    [SerializeField] private float m_Speed;
    private bool m_Parreado;

    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_Parreado = false;
    }

    public void Init(Transform m_Target, Transform _Boss)
    {
        m_Boss = _Boss;
        Vector2 posicionPlayer = m_Target.transform.position - transform.position;
        float angulo = Mathf.Atan2(posicionPlayer.y, posicionPlayer.x);
        angulo = Mathf.Rad2Deg * angulo - 90;
        transform.localEulerAngles = new Vector3(0, 0, angulo);
        m_RigidBody.velocity = transform.up * m_Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_Parreado)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("BossHurtBox"))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
            {
                if (collision.gameObject.GetComponent<SMBPlayerParryState>().parry)
                {
                    gameObject.layer = LayerMask.NameToLayer("PlayerHitBox");
                    m_Parreado = true;
                    Vector2 posicionPlayer = m_Boss.transform.position - transform.position;
                    float angulo = Mathf.Atan2(posicionPlayer.y, posicionPlayer.x);
                    angulo = Mathf.Rad2Deg * angulo - 90;
                    transform.localEulerAngles = new Vector3(0, 0, angulo);
                    m_RigidBody.velocity = transform.up * m_Speed;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
