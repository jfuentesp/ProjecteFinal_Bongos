using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPointScript : MonoBehaviour
{
    [SerializeField] private int m_MinPoints;
    public int MinPoints => m_MinPoints;
    [SerializeField] private int m_MaxPoints;
    public int MaxPoints => m_MaxPoints;

    private int m_Points;
    public int Points => m_Points;

    private void Awake()
    {
        m_Points = Random.Range(m_MinPoints, m_MaxPoints + 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<PJSMB>(out PJSMB player))
            {
                Destroy(gameObject);
            }
        }
    }
}
