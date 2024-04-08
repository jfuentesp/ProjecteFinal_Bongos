using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    protected float m_TimeUntilDestroyed;
    [SerializeField]
    protected float m_SizeRadius;
    protected Vector2 m_Size;

    // Start is called before the first frame update
    void Start()
    {
        m_Size = new Vector2(m_SizeRadius, m_SizeRadius);
        transform.localScale = m_Size;
        StartCoroutine(ReturnToPoolCoroutine());
    }

    private IEnumerator ReturnToPoolCoroutine()
    {
        yield return new WaitForSeconds(m_TimeUntilDestroyed);
        this.gameObject.SetActive(false);
    }
}
