using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_Instance;
    public static GameManager Instance => m_Instance;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && !m_MundoGenerado)
        {
            m_NuevaPartida = true;
            m_MundoGenerado = true;
            LevelManager.Instance.Init();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            m_NuevaPartida = false;
            LevelManager.Instance.Init();
        }
    }
}
