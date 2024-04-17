using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_Instance;
    public static GameManager Instance => m_Instance;

    private bool m_NuevaPartida;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            m_NuevaPartida = true;
            LevelManager.Instance.Init();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            m_NuevaPartida = false;
            LevelManager.Instance.Init();
        }
    }
}
