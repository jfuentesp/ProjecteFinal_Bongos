using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusitaBehaviour : MonoBehaviour
{
    private int m_KindOfMedusita;
    private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_UpdateDirectonTime;
    private Rigidbody2D m_RigidBody;
    private Transform m_Target;
    private bool m_Inmolando;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_Inmolando = false;
    }
    public void Init(int _kindMedusita, Transform _Target)
    {
        m_Target = _Target;
        m_KindOfMedusita = _kindMedusita;
        switch (m_KindOfMedusita)
        {
            case 1:
                m_SpriteRenderer.color = Color.green;
                m_Speed *= 1;
                m_UpdateDirectonTime /= 1;
                break;
            case 2:
                m_SpriteRenderer.color = Color.yellow;
                m_Speed *= 2;
                m_UpdateDirectonTime /= 2;
                break;
            case 3:
                m_SpriteRenderer.color = Color.red;
                m_Speed *= 3;
                m_UpdateDirectonTime /= 3;
                break;
            default:
                break;
        }
    }
    public void PlayerHoming()
    {
        StartCoroutine(Homing());
    }

    private IEnumerator Homing()
    {
        UpdateRotacion(1);
        while (true)
        {
            m_RigidBody.velocity = transform.up * m_Speed;
            UpdateRotacion(0.8f);
            yield return new WaitForSeconds(m_UpdateDirectonTime);
        }
    }

    private void UpdateRotacion(float max)
    {
        Vector2 posicionPlayer = m_Target.position - transform.position;
        float angulo = Mathf.Atan2(posicionPlayer.y, posicionPlayer.x);
        angulo = Mathf.Rad2Deg * angulo - 90;
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(0, 0, angulo), max); ;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_Inmolando)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
            {
                Destroy(gameObject);
            }
        }
    }
}
