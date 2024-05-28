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
using static SaveLoadGame.SavePreSets;

public class GameManager : MonoBehaviour, ISaveablePreSetsData
{
    [Header("Testing")]
    [SerializeField] private bool m_Testing;
    [SerializeField] private string m_NombreDeTuEscena;
    public string NombreDeTuEscena => m_NombreDeTuEscena;
    public bool Testing => m_Testing;

    private float m_TimerPartida = 0;
    public float TimerPartida => m_TimerPartida;

    public Action<float> OnTimerUpdate;

    public void setTesting()
    {
        m_Testing = false;
    }

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

    [Header("Listed Abilities")]
    [SerializeField]
    private List<Ability> m_Tier1AbilitiesInitial;
    private List<Ability> m_Tier1AbilitiesCopy;
    public List<Ability> Tier1Abilities => m_Tier1AbilitiesCopy;
    [SerializeField]
    private List<Ability> m_Tier2AbilitiesInitial;
    private List<Ability> m_Tier2AbilitiesCopy;
    public List<Ability> Tier2Abilities => m_Tier2AbilitiesCopy;
    [SerializeField]
    private List<Ability> m_Tier3AbilitiesInitial;
    private List<Ability> m_Tier3AbilitiesCopy;
    public List<Ability> Tier3Abilities => m_Tier3AbilitiesCopy;

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

    public void AcabarJuego()
    {
        SceneManager.LoadScene("EscenaInicial");
        if(m_TimerCoroutine != null)
            StopCoroutine(m_TimerCoroutine);
        print("MuereCalvo");
        print(m_PlayerInGame == null);
        //Destroy(m_PlayerInGame);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        if (scene.name == "Mundo1")
        {
            m_PlayerInGame = Instantiate(m_PlayerPrefab);
            m_PlayerInGame.transform.position = Vector3.zero;
            m_TimerCoroutine = StartCoroutine(Timer());
        }
        if (scene.name == "Mundo2")
        {
            if (m_PlayerInGame == null)
            {
                m_PlayerInGame = Instantiate(m_PlayerPrefab);
                m_TimerCoroutine = StartCoroutine(Timer());
            }

            m_PlayerInGame.transform.position = Vector3.zero;
        }
        if (scene.name == m_NombreDeTuEscena)
        {
            m_PlayerInGame = Instantiate(m_PlayerPrefab);
            m_PlayerInGame.transform.position = Vector3.zero;
            m_TimerCoroutine = StartCoroutine(Timer());
        }
    }

    public void AvanzarMundo(MundoEnum mundoEnum)
    {
        if (mundoEnum == MundoEnum.MUNDO_UNO)
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
    
    private void GetPlayersAndTheirWorld()
    {
        if (!Directory.Exists(rutaCompletaHastaCarpeta))
        {
            // Crea la carpeta si no existe
            Directory.CreateDirectory(rutaCompletaHastaCarpeta);

            BuildEmptyFile();

            GetPlayersAndWorldsListOfGameManager();
        }
        else
        {
            if (!File.Exists(rutaCompleta))
            {
                BuildEmptyFile();
            }

            GetPlayersAndWorldsListOfGameManager();
        }
    }

    private void GetPlayersAndWorldsListOfGameManager()
    {
        SaveAllGames playerAndWorld = GetPlayersFromFile();
        if (playerAndWorld.m_SavedGames != null)
        {
            foreach (SaveGame partida in playerAndWorld.m_SavedGames)
            {
                m_PlayersAndTheirWorldsList.Add(partida.m_NameAndWorld);
            }
        }
    }

    private void BuildEmptyFile()
    {
        SaveGame partidaVacia1 = new();
        partidaVacia1.m_NameAndWorld.m_Mundo = MundoEnum.VACIO;
        partidaVacia1.m_NameAndWorld.m_Name = "EMPTY";

        SaveGame partidaVacia2 = new();
        partidaVacia2.m_NameAndWorld.m_Mundo = MundoEnum.VACIO;
        partidaVacia2.m_NameAndWorld.m_Name = "EMPTY";

        SaveGame partidaVacia3 = new();
        partidaVacia3.m_NameAndWorld.m_Mundo = MundoEnum.VACIO;
        partidaVacia3.m_NameAndWorld.m_Name = "EMPTY";

        List<SaveGame> playerAndWorldList = new()
            {
                  partidaVacia1,
                  partidaVacia2,
                  partidaVacia3
            };


        SaveAllGames playerAndWorld = new();
        playerAndWorld.m_SavedGames = playerAndWorldList.ToArray();

        string jsonData = JsonUtility.ToJson(playerAndWorld);

        print(jsonData);

        File.WriteAllText(rutaCompleta, jsonData);
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

    public void SavePlayersAndTheirWorld(string _PlayerName, int id)
    {

        // Combinar la ruta de la carpeta con el nombre del archivo
        SaveAllGames saveAllGames = new SaveAllGames();
        SaveGame saveGame = new SaveGame();
        saveGame.m_NameAndWorld = new SaveGame.NameAndWorld(_PlayerName, MundoEnum.MUNDO_UNO);

        m_PlayersAndTheirWorldsList[id] = saveGame.m_NameAndWorld;

        List<SaveGame> playerAndWorld = GetPlayersFromFile().m_SavedGames.ToList();

        playerAndWorld[id] = saveGame;
        saveAllGames.m_SavedGames = playerAndWorld.ToArray();
        string jsonData = JsonUtility.ToJson(saveAllGames);

        File.WriteAllText(rutaCompleta, jsonData);

        /*
        m_PlayersAndTheirWorldsList.Add(saveGame.m_NameAndWorld);
        if (GetPlayersFromFile().m_SavedGames == null)
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
        */
    }

    private IEnumerator Timer()
    {
        while(m_PlayerInGame != null)
        {
            yield return new WaitForSeconds(1f);
            m_TimerPartida++;
            OnTimerUpdate.Invoke(m_TimerPartida);
        }
    }

    private Coroutine m_TimerCoroutine;
    public void CreateNewGameOfPlayer(string _PlayerName, string mundo)
    {
        m_PlayerName = _PlayerName;
        m_NuevaPartida = true;
        LoadAbilityLists();
        m_TimerPartida = 0;
        SceneManager.LoadScene(mundo);
    }

    public void LoadAbilityLists()
    {
        m_Tier1AbilitiesCopy = new List<Ability>();
        m_Tier2AbilitiesCopy = new List<Ability>();
        m_Tier3AbilitiesCopy = new List<Ability>();
        foreach(Ability ability in m_Tier1AbilitiesInitial)
        {
            m_Tier1AbilitiesCopy.Add(ability);
        }
        foreach(Ability ability in m_Tier2AbilitiesInitial)
        {
            m_Tier2AbilitiesCopy.Add(ability);
        }
        foreach(Ability ability in m_Tier3AbilitiesInitial)
        {
            m_Tier3AbilitiesCopy.Add(ability);
        }
    }

    public void LoadGameOfPlayer(string _PlayerName, string mundo)
    {
        m_PlayerName = _PlayerName;
        m_NuevaPartida = false;
        SceneManager.LoadScene(mundo);
    }

    public void DeletePlayerGame(string _PlayerName, int id)
    {
        //print($"Borrando la partida del jugador: {m_PlayersAndTheirWorldsList[i].m_Name}");
        List<SaveGame> playerAndWorldList = GetPlayersFromFile().m_SavedGames.ToList();
        SaveGame partida = new();
        partida.m_NameAndWorld.m_Mundo = MundoEnum.VACIO;
        partida.m_NameAndWorld.m_Name = "EMPTY";
        playerAndWorldList[id] = partida;
        m_PlayersAndTheirWorldsList[id] = partida.m_NameAndWorld;
        SaveAllGames playerAndWorld = new();
        playerAndWorld.m_SavedGames = playerAndWorldList.ToArray();

        string jsonData = JsonUtility.ToJson(playerAndWorld);

        print(jsonData);

        File.WriteAllText(rutaCompleta, jsonData);
        OnPlayerDeleted?.Invoke();
    }

    public PreSets Save()
    {
        throw new NotImplementedException();
    }

    public void Load(PreSets _salaData)
    {
        throw new NotImplementedException();
    }
}
