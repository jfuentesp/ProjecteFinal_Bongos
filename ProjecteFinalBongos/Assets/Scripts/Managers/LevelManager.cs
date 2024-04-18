using GeneracionSalas;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    private static LevelManager m_Instance;
    public static LevelManager Instance => m_Instance;

    private GeneracionSalasMatriz m_GeneracionSalasMatriz;

    [SerializeField] private Pool m_SplashPool;
    [HideInInspector] public Pool _SplashPool;
    [SerializeField] private Pool m_BulletPool;
    [HideInInspector] public Pool _BulletPool;
    [SerializeField] private List<BossDisponible> m_ListaBossesDisponibles = new();
    [SerializeField] private GameEvent m_GuardarPartidaEvent;

    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_GeneracionSalasMatriz = GetComponent<GeneracionSalasMatriz>();
        TodosLosBossesDisponibles();
    }

    private void TodosLosBossesDisponibles()
    {
        for (int i = 0; i < m_ListaBossesDisponibles.Count; i++)
        {
            BossDisponible bossTemporal = m_ListaBossesDisponibles[i];
            bossTemporal.m_BossDisponible = true;
            m_ListaBossesDisponibles[i] = bossTemporal;
        }
    }

    public int GetAvailableBoss()
    {
        int numero = Random.Range(0, 8);
        if (m_ListaBossesDisponibles[numero].m_BossDisponible)
        {
            return numero;
        }
        else
        {
            return GetAvailableBoss();
        }

    }

    public GameObject GetBossToSpawn(int numBoss)
    {
        return m_ListaBossesDisponibles[numBoss].m_BossPrefab;
    }

    public void Init()
    {
        m_GeneracionSalasMatriz.Init();
        m_GuardarPartidaEvent.Raise();
    }

    [Serializable]
    public struct BossDisponible
    {
        public GameObject m_BossPrefab;
        public bool m_BossDisponible;

        public BossDisponible(GameObject _BossPrefab, bool _BossDisponible)
        {
            m_BossDisponible = _BossDisponible;
            m_BossPrefab = _BossPrefab;
        }
    }
}
