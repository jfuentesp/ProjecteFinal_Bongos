using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class TrapBullet : Bullet
{
    [SerializeField]
    private GameObject m_CagePrefab;

    [Header("Growth duration")]
    [Tooltip("This bullet will increase its size during the given time")]
    [SerializeField]
    private float m_GrowthDuration;

    [Header("Growth size")]
    [SerializeField, Range(0, 0.9f)]
    private float m_GrowthSize;

    Vector3 m_CurrentScale;

    public override void Init(Vector2 direction)
    {
        base.Init(direction);
        m_CurrentScale = transform.localScale;
        growing = true;
    }

    float t = 0;
    bool growing = true;
    private void Update()
    {
        if (!growing)
            return;
        t += Time.deltaTime / m_GrowthDuration;
        Vector3 newScale = new Vector3(Mathf.Lerp(m_CurrentScale.x, transform.localScale.x * m_GrowthSize, t),
                                       Mathf.Lerp(m_CurrentScale.y, transform.localScale.y * m_GrowthSize, t), 0);
        transform.localScale = newScale;

        if(t > m_GrowthDuration) 
        {
            growing = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            SetPlayerOnCage(collision.gameObject);
    }

    private void SetPlayerOnCage(GameObject gameobject)
    {
        
    }
}
