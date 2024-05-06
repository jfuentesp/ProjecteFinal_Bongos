using GeneracionSalas;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [Header("Testing")]
    [SerializeField] private bool m_Testing;
    private static LevelManager m_Instance;
    public static LevelManager Instance => m_Instance;

    private GeneracionSalasMatriz m_GeneracionSalasMatriz;
    [SerializeField] private ConsumablesDataBase m_ConsumableDataBase;
    public ConsumablesDataBase ConsumableDataBase => m_ConsumableDataBase;

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

    private GUIBossManager m_GUIBossManager;
    public GUIBossManager GUIBossManager => m_GUIBossManager;

    [Header("Variables Mundo")]
    [SerializeField]
    private MundoEnum m_MundoActual;
    public MundoEnum MundoActualJugador => m_MundoActual;
    private int m_BossesMuertos;

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

    private EventSystem m_eventSystem;
    public EventSystem EventSystem => m_eventSystem;
    private InputSystemUIInputModule m_InputSystemUIInputModule;
    public InputSystemUIInputModule InputSystemUIInputModule => m_InputSystemUIInputModule;
    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        m_GeneracionSalasMatriz = GetComponent<GeneracionSalasMatriz>();
        m_GUIBossManager = GetComponent<GUIBossManager>();
        m_eventSystem = GetComponent<EventSystem>();
        m_InputSystemUIInputModule = GetComponent<InputSystemUIInputModule>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        print("Start Level Manager");
        if (m_CloseShopButton) m_CloseShopButton.onClick.AddListener(CloseShop);
        TodosLosBossesDisponibles();
        m_TiendaPanel.SetActive(false);
        idPiccolo = 0;
        m_BossesMuertos = 0;
        if (!GameManager.Instance.NuevaPartida)
        {
            print("Cargando partida");
            m_CargarPartidaEvent.Raise();
        }
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
        return idPiccolo;
    }
    public int GetAvailableBoss()
    {
        if (m_Testing)
        {
            int numero = Random.Range(0, m_ListaBossesDisponibles.Count);
            if (m_ListaBossesDisponibles[numero].m_BossDisponible)
            {
                return numero;
            }
            else
            {
                numero = GetAvailableBoss();
            }
            return numero;
        }
        else
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
    }

    public GameObject GetBossToSpawn(int numBoss)
    {
        return m_ListaBossesDisponibles[numBoss].m_BossPrefab;
    }

    public void GuardarPartida()
    {
        m_GuardarPartidaEvent.Raise();
    }

    public List<Consumable> GetObjetosTienda()
    {
        List<Consumable> objetitosParaPiccoloChad = new();


        for (int i = 0; i < numeroObjetosTienda; i++)
        {
            float numeroMinimo = 0;
            float random = Random.Range(0.0f, 100.0f);
            foreach (ObjetosDisponibles objeto in m_ObjetosDisponiblesTienda)
            {
                if (random >= numeroMinimo && random <= objeto.ratioAparicion + numeroMinimo)
                {
                    objetitosParaPiccoloChad.Add(objeto.m_Object);
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
    public List<Consumable> GetObjetosSalaObjetos()
    {
        List<Consumable> objetitosParaSalaObjetos = new();


        for (int i = 0; i < numeroObjetosSalaObjetos; i++)
        {
            float numeroMinimo = 0;
            float random = Random.Range(0.0f, 100.0f);
            foreach (ObjetosDisponibles objeto in m_ObjetosDisponiblesSalas)
            {
                if (random >= numeroMinimo && random <= objeto.ratioAparicion + numeroMinimo)
                {
                    objetitosParaSalaObjetos.Add(objeto.m_Object);
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

    public void BossMuerto()
    {
        m_BossesMuertos++;
        print(m_BossesMuertos);

        if (m_BossesMuertos == 1)
        {
            GameManager.Instance.AlCargarMundo();
        }
        if (m_BossesMuertos == 7)
        {
            GameManager.Instance.AvanzarMundo(m_MundoActual);
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
    [Serializable]
    public struct ObjetosDisponibles
    {
        public Consumable m_Object;
        public float ratioAparicion;

        public ObjetosDisponibles(Consumable _Object, float _RatioAparicion)
        {
            m_Object = _Object;
            ratioAparicion = _RatioAparicion;
        }
    }
}
