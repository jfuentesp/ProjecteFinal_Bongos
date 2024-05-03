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
    private string m_PlayerName;
    public string PlayerName => m_PlayerName;
    private const string playerAndWorldFile = "JugadoresGuardados.txt";
    private string rutaCompletaHastaCarpeta;
    public string RutaCompletaHastaCarpeta => rutaCompletaHastaCarpeta;
    private string rutaCompleta;
    public string RutaCompleta => rutaCompleta;
    [SerializeField]
    private List<SaveGame> m_PlayersAndTheirWorldsList = new();
    public List<SaveGame> PlayersAndTheirWorldsList => m_PlayersAndTheirWorldsList;

    [Header("Variables Mundo Generado")]
    private bool m_NuevaPartida;
    private bool m_MundoGenerado;
    public bool NuevaPartida => m_NuevaPartida;

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
        rutaCompletaHastaCarpeta = Path.Combine(Application.persistentDataPath, "DataFiles", "SaveGame");
        rutaCompleta = Path.Combine(Application.persistentDataPath, "DataFiles", "SaveGame", playerAndWorldFile);
        GetPlayersAndTheirWorld();
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
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
            m_PlayerInGame.transform.position = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.G) && !m_MundoGenerado)
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
        }*/
    }
    private void GetPlayersAndTheirWorld()
    {
        if (!Directory.Exists(rutaCompletaHastaCarpeta))
        {
            // Crea la carpeta si no existe
            Directory.CreateDirectory(rutaCompletaHastaCarpeta);
            File.WriteAllText(rutaCompleta, "{}");
            string jsonData = File.ReadAllText(rutaCompleta);
            SaveAllGames playerAndWorld = new();
            JsonUtility.FromJsonOverwrite(jsonData, playerAndWorld);
            m_PlayersAndTheirWorldsList ??= playerAndWorld.m_SavedGames.ToList();
            Debug.Log("Carpeta creada en: " + rutaCompletaHastaCarpeta);
        }
        else
        {
            if (!File.Exists(rutaCompleta))
                File.WriteAllText(rutaCompleta, "{}");


            string jsonData = File.ReadAllText(rutaCompleta);
            SaveAllGames playerAndWorld = new();
            JsonUtility.FromJsonOverwrite(jsonData, playerAndWorld);
            if(playerAndWorld.m_SavedGames != null)
            {
                m_PlayersAndTheirWorldsList = playerAndWorld.m_SavedGames.ToList();
                Debug.Log("La carpeta ya existe en: " + rutaCompletaHastaCarpeta);
            }
        }
    }

    public bool PlayerExists(string _PlayerName)
    {
        foreach (SaveGame playerAndWorld in m_PlayersAndTheirWorldsList)
        {
            if (playerAndWorld.m_NameAndWorld.m_Name == _PlayerName)
                return true;
        }
        return false;
    }

    public void SavePlayersAndTheirWorld(string _PlayerName)
    {

        // Combinar la ruta de la carpeta con el nombre del archivo
        SaveAllGames playerAndWorld = new();
        SaveGame saveGame = new SaveGame();
        saveGame.m_NameAndWorld = new SaveGame.NameAndWorld(_PlayerName, MundoEnum.MUNDO_UNO);
        m_PlayersAndTheirWorldsList.Add(saveGame);
        playerAndWorld.m_SavedGames = m_PlayersAndTheirWorldsList.ToArray();

        string jsonData = JsonUtility.ToJson(playerAndWorld);

        print(jsonData);

        File.WriteAllText(rutaCompleta, jsonData);
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
        foreach(SaveGame jugador in m_PlayersAndTheirWorldsList)
        {
            if (jugador.m_NameAndWorld.m_Name == _PlayerName)
            {
                break;
            }
            i++;
        }
        print($"Borrando la partida del jugador: {m_PlayersAndTheirWorldsList[i].m_NameAndWorld.m_Name}");
        m_PlayersAndTheirWorldsList.RemoveAt(i);
        SaveAllGames playerAndWorld = new();
        playerAndWorld.m_SavedGames = m_PlayersAndTheirWorldsList.ToArray();

        string jsonData = JsonUtility.ToJson(playerAndWorld);

        print(jsonData);

        File.WriteAllText(rutaCompleta, jsonData);
        OnPlayerDeleted?.Invoke();
    }
}
