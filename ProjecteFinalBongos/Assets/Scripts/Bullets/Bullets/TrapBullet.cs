using System.Collections;
using System.Collections.Generic;
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
    [SerializeField, Range(0, 1f)]
    private float m_GrowthSize;

    Vector3 m_CurrentScale;

    private void OnEnable()
    {
        m_GrowthCoroutine = StartCoroutine(GrowthCoroutine());
    }

    private void OnDisable()
    {
        if(m_GrowthCoroutine != null)
            StopCoroutine(m_GrowthCoroutine);
    }

    public override void Init(Vector2 direction)
    {
        base.Init(direction);
    }

    float t = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled)
            return;
        if (collision.CompareTag("Player"))
            SetPlayerOnCage(collision.gameObject);
    }

    private void SetPlayerOnCage(GameObject gameobject)
    {
        
    }

    private Coroutine m_GrowthCoroutine;
    private IEnumerator GrowthCoroutine()
    {
        while(t < m_GrowthDuration)
        {
            float size = Mathf.Lerp(m_CurrentScale.x, transform.localScale.x / m_GrowthSize, t);
            Vector3 newScale = new Vector3(size, size, 0);
            transform.localScale = newScale;
            yield return new WaitForSeconds(0.2f);
            t += 0.2f;
        }
    }
}
