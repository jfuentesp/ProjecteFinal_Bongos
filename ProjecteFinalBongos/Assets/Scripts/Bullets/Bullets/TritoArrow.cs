using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TritoArrow : Bullet
{
    Transform m_Target;
    public void Init(Vector2 direction, Transform _Transform)
    {
        m_Animator.Play(m_AnimationName);
        transform.up = direction;
        m_Size = new Vector2(m_SizeRadius, m_SizeRadius);
        transform.localScale = m_Size;
        m_Rigidbody.velocity = transform.up * m_Speed;
        StartCoroutine(ReturnToPoolCoroutine());
        m_Target = _Transform;
        GetComponent<BossAttackDamage>().SetDamage(m_Damage);
        if (m_AudioBullet)
        {
            m_AudioSource.clip = m_AudioBullet;
            m_AudioSource.Play();
        }
        gameObject.layer = LayerMask.NameToLayer("BossHitBox");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_Target != null)
        {
            Vector2 posicionPlayer = transform.position - m_Target.position;
            float angulo = Mathf.Atan2(posicionPlayer.y, posicionPlayer.x);
            angulo = Mathf.Rad2Deg * angulo - 90;
            LevelManager.Instance.GUIBossManager.UploadFlecha(angulo);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!enabled)
            return;
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.layer = LayerMask.NameToLayer("AllHitBox");
            DisableBullet();
        }
    }
}
