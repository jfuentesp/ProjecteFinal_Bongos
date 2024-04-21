using GeneracionSalas;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    private static LevelManager m_Instance;
    public static LevelManager Instance => m_Instance;

    private GeneracionSalasMatriz m_GeneracionSalasMatriz;

    [Header("Variables Piccolo Chad")]
    [SerializeField] private GameObject dialoguePanel;
    public GameObject DialoguePanel => dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    public TextMeshProUGUI DialogueText => dialogueText;
    [SerializeField] private GameObject m_TiendaPanel;
    [SerializeField] private Button m_CloseShopButton;
    private int idPiccolo;
    public Action<int> onCloseShopOfPiccolo;
    private int piccoloConTiendaAbierta;

    public enum MundoActual
    {
        MUNDO_UNO, MUNDO_DOS, MUNDO_TRES
    };
    [Header("Variables Mundo")]
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

    [Header("Lista de objetos")]
    [SerializeField] private List<ObjetosDisponibles> m_ObjetosDisponiblesTienda;
    [SerializeField] private List<ObjetosDisponibles> m_ObjetosDisponiblesSalas;
    [SerializeField] private int numeroObjetosTienda;
    [SerializeField] private int numeroObjetosSalaObjetos;


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
        if (m_CloseShopButton) m_CloseShopButton.onClick.AddListener(CloseShop);
        TodosLosBossesDisponibles();
        m_TiendaPanel.SetActive(false);
    }

    public void OpenShop(int id)
    {
        piccoloConTiendaAbierta = id;
        m_TiendaPanel.SetActive(true);
    }

    private void CloseShop()
    {
        m_TiendaPanel.SetActive(false);
        onCloseShopOfPiccolo?.Invoke(piccoloConTiendaAbierta);
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

    public int GiveIdToPiccoloChad()
    {
        idPiccolo++;
        return idPiccolo - 1;
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

    public List<ObjetosDisponibles> GetObjetosTienda()
    {
        List<ObjetosDisponibles> objetitosParaPiccoloChad = new();
       

        for (int i = 0; i < numeroObjetosTienda; i++)
        {
            float numeroMinimo = 0;
            float random = Random.Range(0.0f, 100.0f);
            foreach (ObjetosDisponibles objeto in m_ObjetosDisponiblesTienda)
            {
                if (random >= numeroMinimo && random <= objeto.ratioAparicion + numeroMinimo)
                {
                    objetitosParaPiccoloChad.Add(objeto);
                    break;
                }
                else
                {
                    numeroMinimo += objeto.ratioAparicion;
                }
            }
        }
        return objetitosParaPiccoloChad;
    }
    public List<ObjetosDisponibles> GetObjetosSalaObjetos()
    {
        List<ObjetosDisponibles> objetitosParaSalaObjetos = new();


        for (int i = 0; i < numeroObjetosSalaObjetos; i++)
        {
            float numeroMinimo = 0;
            float random = Random.Range(0.0f, 100.0f);
            foreach (ObjetosDisponibles objeto in m_ObjetosDisponiblesSalas)
            {
                if (random >= numeroMinimo && random <= objeto.ratioAparicion + numeroMinimo)
                {
                    objetitosParaSalaObjetos.Add(objeto);
                    break;
                }
                else
                {
                    numeroMinimo += objeto.ratioAparicion;
                }
            }
        }
        return objetitosParaSalaObjetos;
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
    [Serializable]
    public struct ObjetosDisponibles
    {
        public int m_NumberOfObject;
        public float ratioAparicion;

        public ObjetosDisponibles(int _numberOfObject, float _RatioAparicion)
        {
            m_NumberOfObject = _numberOfObject;
            ratioAparicion = _RatioAparicion;
        }
    }
}
