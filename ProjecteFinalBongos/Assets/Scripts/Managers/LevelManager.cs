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

    public enum MundoActual
    {
        MUNDO_UNO, MUNDO_DOS, MUNDO_TRES
    };
    [SerializeField]
    private MundoActual m_MundoActual;
    public MundoActual MundoActualJugador => m_MundoActual;

    [SerializeField] private Pool m_SplashPool;
    public Pool _SplashPool => m_SplashPool;
    [SerializeField] private Pool m_BulletPool;
    public Pool _BulletPool => m_BulletPool;
    [SerializeField] private List<BossDisponible> m_ListaBossesDisponibles = new();
    [SerializeField] private GameEvent m_GuardarPartidaEvent;
    [SerializeField] private GameEvent m_CargarPartidaEvent;

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
        int numero = Random.Range(0, m_ListaBossesDisponibles.Count);
        if (m_ListaBossesDisponibles[numero].m_BossDisponible)
        {
            BossDisponible bossTemporal = m_ListaBossesDisponibles[numero];
            bossTemporal.m_BossDisponible = false;
            m_ListaBossesDisponibles[numero] = bossTemporal;

            return numero;
        }
        else
        {
             numero = GetAvailableBoss();
        }
        return numero;
    }

    public GameObject GetBossToSpawn(int numBoss)
    {
        return m_ListaBossesDisponibles[numBoss].m_BossPrefab;
    }

    public void Init()
    {
        if (GameManager.Instance.NuevaPartida)
        {
            m_GeneracionSalasMatriz.Init();
            m_GuardarPartidaEvent.Raise();
        }
        else
        {
            m_CargarPartidaEvent.Raise();
        }
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
