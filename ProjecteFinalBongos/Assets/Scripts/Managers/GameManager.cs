using multilanguaje;
using SaveLoadGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Testing")]
    [SerializeField] private bool m_Testing;
    public bool Testing => m_Testing;

    [Header("Variables GameManager")]
    private static GameManager m_Instance;
    public static GameManager Instance => m_Instance;

    [Header("Variables Jugador")]
    [SerializeField]
    private GameObject m_PlayerPrefab;
    private GameObject m_PlayerInGame;
    public GameObject PlayerInGame => m_PlayerInGame;

    [Header("Variables Ficheros")]
    public Action OnPlayerDeleted;
    public Action onGetPlayers;
    [SerializeField] private string m_PlayerName;
    public string PlayerName => m_PlayerName;
    private const string playerAndWorldFile = "JugadoresGuardados.txt";
    private string rutaCompletaHastaCarpeta;
    public string RutaCompletaHastaCarpeta => rutaCompletaHastaCarpeta;
    private string rutaCompleta;
    public string RutaCompleta => rutaCompleta;
    [SerializeField]
    private List<SaveGame.NameAndWorld> m_PlayersAndTheirWorldsList = new();
    public List<SaveGame.NameAndWorld> PlayersAndTheirWorldsList => m_PlayersAndTheirWorldsList;

    [Header("Variables Mundo Generado")]
    private bool m_NuevaPartida;
    private bool m_MundoGenerado;
    [SerializeField] private IdiomaEnum m_IdiomaJuego;
    public IdiomaEnum IdiomaJuego => m_IdiomaJuego;
    public bool NuevaPartida => m_NuevaPartida;

    [Header("Variables Idioma")]
    private MultiLanguageManager m_LanguageManager;
    public MultiLanguageManager LanguageManager => m_LanguageManager;

    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        m_MundoGenerado = false;
    }
    private void Start()
    {
        if (!m_Testing)
        {
            m_LanguageManager = GetComponent<MultiLanguageManager>();
            rutaCompletaHastaCarpeta = Path.Combine(Application.persistentDataPath, "DataFiles", "SaveGame");
            rutaCompleta = Path.Combine(Application.persistentDataPath, "DataFiles", "SaveGame", playerAndWorldFile);
            GetPlayersAndTheirWorld();
        }
        else
        {
            m_LanguageManager = GetComponent<MultiLanguageManager>();
            m_PlayerInGame = Instantiate(m_PlayerPrefab);
            m_PlayerInGame.transform.position = Vector3.zero;
        }
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SetNamePlayer(string _NamePlayer)
    {
        m_PlayerName = _NamePlayer;
    }

    public void PauseGame(bool juegoPausado)
    {
        if (juegoPausado)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    public void ChangeLanguage(IdiomaEnum idioma)
    {
        m_IdiomaJuego = idioma;
        m_LanguageManager.getIdioma();  
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        if(scene.name == "Mundo1")
        {
            m_PlayerInGame = Instantiate(m_PlayerPrefab);
            m_PlayerInGame.transform.position = Vector3.zero;
          
        }
        if(scene.name == "Mundo2")
        {
            if(!m_NuevaPartida)
                m_PlayerInGame = Instantiate(m_PlayerPrefab);
            m_PlayerInGame.transform.position = Vector3.zero;
        }
    }

    public void AvanzarMundo(MundoEnum mundoEnum)
    {
        if(mundoEnum == MundoEnum.MUNDO_UNO)
        {
            SceneManager.LoadScene("Mundo2");
        }
        else
        {
            print("Te lo pasaste bro!");
        }
    }
    public void AlCargarMundo()
    {
        m_NuevaPartida = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (m_Testing)
        {
            if (Input.GetKeyDown(KeyCode.G) && !m_MundoGenerado)
            {
                m_NuevaPartida = true;
                m_MundoGenerado = true;
                LevelManager.Instance.Init();
            }
            if (Input.GetKeyDown(KeyCode.C) && !m_MundoGenerado)
            {
                m_NuevaPartida = false;
                m_MundoGenerado = true;
                LevelManager.Instance.Init();
            }
        }
    }
    private void GetPlayersAndTheirWorld()
    {
        if (!Directory.Exists(rutaCompletaHastaCarpeta))
        {
            // Crea la carpeta si no existe
            Directory.CreateDirectory(rutaCompletaHastaCarpeta);
            File.WriteAllText(rutaCompleta, "");
            SaveAllGames playerAndWorld = GetPlayersFromFile();
            if (playerAndWorld.m_SavedGames != null)
            {
                foreach (SaveGame partida in playerAndWorld.m_SavedGames)
                {
                    m_PlayersAndTheirWorldsList.Add(partida.m_NameAndWorld);
                }
            }
            Debug.Log("Carpeta creada en: " + rutaCompletaHastaCarpeta);
        }
        else
        {
            if (!File.Exists(rutaCompleta))
                File.WriteAllText(rutaCompleta, "");

            SaveAllGames playerAndWorld = GetPlayersFromFile();
            if (playerAndWorld.m_SavedGames != null)
            {
                foreach (SaveGame partida in playerAndWorld.m_SavedGames)
                {
                    m_PlayersAndTheirWorldsList.Add(partida.m_NameAndWorld);
                }
            }
            onGetPlayers?.Invoke();
            Debug.Log("La carpeta ya existe en: " + rutaCompletaHastaCarpeta);
        }
    }

    private SaveAllGames GetPlayersFromFile()
    {
        string jsonData = File.ReadAllText(rutaCompleta);
        SaveAllGames playerAndWorld = new();
        JsonUtility.FromJsonOverwrite(jsonData, playerAndWorld);
        //print(playerAndWorld.m_SavedGames[0].m_NameAndWorld.m_Name);
        return playerAndWorld;
    }

    public bool PlayerExists(string _PlayerName)
    {
        foreach (SaveGame.NameAndWorld playerAndWorld in m_PlayersAndTheirWorldsList)
        {
            if (playerAndWorld.m_Name == _PlayerName)
                return true;
        }
        return false;
    }

    public void SavePlayersAndTheirWorld(string _PlayerName)
    {

        // Combinar la ruta de la carpeta con el nombre del archivo
        SaveAllGames saveAllGames = new SaveAllGames();
        SaveGame saveGame = new SaveGame();
        saveGame.m_NameAndWorld = new SaveGame.NameAndWorld(_PlayerName, MundoEnum.MUNDO_UNO);


        m_PlayersAndTheirWorldsList.Add(saveGame.m_NameAndWorld);
        if(GetPlayersFromFile().m_SavedGames == null)
        {
            List<SaveGame> playerAndWorld = new List<SaveGame> { saveGame };
            saveAllGames.m_SavedGames = playerAndWorld.ToArray();
            string jsonData = JsonUtility.ToJson(saveAllGames);

            print(jsonData);

            File.WriteAllText(rutaCompleta, jsonData);
        }
        else
        {
            List<SaveGame> playerAndWorld = GetPlayersFromFile().m_SavedGames.ToList();
            
            playerAndWorld.Add(saveGame);
            saveAllGames.m_SavedGames = playerAndWorld.ToArray();
            string jsonData = JsonUtility.ToJson(saveAllGames);

            print(jsonData);

            File.WriteAllText(rutaCompleta, jsonData);
        }
    }

    public void CreateNewGameOfPlayer(string _PlayerName, string mundo)
    {
        m_PlayerName = _PlayerName;
        m_NuevaPartida = true;
        SceneManager.LoadScene(mundo);
    }
    public void LoadGameOfPlayer(string _PlayerName, string mundo)
    {
        m_PlayerName = _PlayerName;
        m_NuevaPartida = false;
        SceneManager.LoadScene(mundo);
    }

    public void DeletePlayerGame(string _PlayerName)
    {
        int i = 0;
        foreach(SaveGame.NameAndWorld jugador in m_PlayersAndTheirWorldsList)
        {
            if (jugador.m_Name == _PlayerName)
            {
                break;
            }
            i++;
        }
        print($"Borrando la partida del jugador: {m_PlayersAndTheirWorldsList[i].m_Name}");
        m_PlayersAndTheirWorldsList.RemoveAt(i);
        List<SaveGame> playerAndWorldList = GetPlayersFromFile().m_SavedGames.ToList();
        playerAndWorldList.RemoveAt(i);
        SaveAllGames playerAndWorld = new();
        playerAndWorld.m_SavedGames = playerAndWorldList.ToArray();

        string jsonData = JsonUtility.ToJson(playerAndWorld);

        print(jsonData);

        File.WriteAllText(rutaCompleta, jsonData);
        OnPlayerDeleted?.Invoke();
    }
}
