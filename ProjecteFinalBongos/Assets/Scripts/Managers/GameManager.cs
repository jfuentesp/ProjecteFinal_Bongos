using SaveLoadGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_Instance;
    public static GameManager Instance => m_Instance;

    private const string playerAndWorldFile = "JugadoresGuardados.txt";
    [SerializeField]
    private List<SavePlayerAndWorld.NameAndWorld> m_PlayersAndTheirWorldsList = new();
    public List<SavePlayerAndWorld.NameAndWorld> PlayersAndTheirWorldsList => m_PlayersAndTheirWorldsList;

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
        GetPlayersAndTheirWorld();
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
        string rutaCompletaHastaCarpeta = Path.Combine(Application.persistentDataPath, "DataFiles", "SaveGame");
        string rutaCompleta = Path.Combine(Application.persistentDataPath, "DataFiles", "SaveGame", playerAndWorldFile);
        if (!Directory.Exists(rutaCompletaHastaCarpeta))
        {
            // Crea la carpeta si no existe
            Directory.CreateDirectory(rutaCompletaHastaCarpeta);
            File.WriteAllText(rutaCompleta, "{}");
            string jsonData = File.ReadAllText(rutaCompleta);
            SavePlayerAndWorld playerAndWorld = new();
            JsonUtility.FromJsonOverwrite(jsonData, playerAndWorld);
            m_PlayersAndTheirWorldsList ??= playerAndWorld.m_NameAndWorld.ToList();
            Debug.Log("Carpeta creada en: " + rutaCompletaHastaCarpeta);
        }
        else
        {
            string jsonData = File.ReadAllText(rutaCompleta);
            SavePlayerAndWorld playerAndWorld = new();
            JsonUtility.FromJsonOverwrite(jsonData, playerAndWorld);
            if(playerAndWorld.m_NameAndWorld != null)
            {
                m_PlayersAndTheirWorldsList = playerAndWorld.m_NameAndWorld.ToList();
                Debug.Log("La carpeta ya existe en: " + rutaCompletaHastaCarpeta);
            }
        }
    }

    public bool PlayerExists(string _PlayerName)
    {
        foreach (SavePlayerAndWorld.NameAndWorld playerAndWorld in m_PlayersAndTheirWorldsList)
        {
            if (playerAndWorld.m_Name == _PlayerName)
                return true;
        }
        return false;
    }

    public void SavePlayersAndTheirWorld(string _PlayerName)
    {

        // Combinar la ruta de la carpeta con el nombre del archivo
        SavePlayerAndWorld playerAndWorld = new();
        m_PlayersAndTheirWorldsList.Add(new SavePlayerAndWorld.NameAndWorld(_PlayerName, Mundo.MUNDO_UNO));
        playerAndWorld.m_NameAndWorld = m_PlayersAndTheirWorldsList.ToArray();

        string jsonData = JsonUtility.ToJson(playerAndWorld);
        string rutaCompleta = Path.Combine(Application.persistentDataPath, "DataFiles", "SaveGame", playerAndWorldFile);

        print(jsonData);

        File.WriteAllText(rutaCompleta, jsonData);
    }

    [Serializable]
    public class SavePlayerAndWorld
    {
        public NameAndWorld[] m_NameAndWorld;

        public void PopulatePlayerAndWorld(NameAndWorld[] _NameAndWorld)
        {
            m_NameAndWorld = _NameAndWorld;
        }

        [Serializable]
        public struct NameAndWorld
        {
            public string m_Name;
            public string m_Mundo;

            public NameAndWorld(string _Name, Mundo _Mundo)
            {
                m_Name = _Name;
                m_Mundo = _Mundo.ToString();
            }
        }
    }
    public enum Mundo { MUNDO_UNO, MUNDO_DOS };
}
