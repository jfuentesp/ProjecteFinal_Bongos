using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] private int m_MinCoins;
    public int MinCoins => m_MinCoins;
    [SerializeField] private int m_MaxCoins;
    public int MaxCoins => m_MaxCoins;

    private int m_Coins;
    public int Coins => m_Coins;

    private void Awake()
    {
        m_Coins = Random.Range(m_MinCoins, m_MaxCoins + 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(collision.gameObject.TryGetComponent<PJSMB>(out PJSMB player))
            {
                Destroy(gameObject);
            }
        }
    }
}
