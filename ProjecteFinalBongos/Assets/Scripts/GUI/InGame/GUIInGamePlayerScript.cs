using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GUIInGamePlayerScript : MonoBehaviour
{
    private PJSMB m_Player;
    private bool m_GameParado;

    [Header("Variables Pausa")]
    [SerializeField] private GameObject m_PausaPanel;
    [SerializeField] private Button m_ResumeGameButton;

    void Start()
    {
        m_Player = PJSMB.Instance;
        if (m_ResumeGameButton) m_ResumeGameButton.onClick.AddListener(ResumeGame);
        m_PausaPanel.SetActive(false);
        m_Player.Input.FindActionMap("PlayerActions").FindAction("Pause").started += PararLaPartida;
    }

    private void ResumeGame()
    {
        m_PausaPanel.SetActive(m_GameParado);
    }

    private void PararLaPartida(InputAction.CallbackContext context)
    {
        if (!m_GameParado)
        {
            m_GameParado = true;
            GameManager.Instance.PauseGame(m_GameParado);
            m_PausaPanel.SetActive(m_GameParado);
        }
        else
        {
            m_GameParado = false;
            GameManager.Instance.PauseGame(m_GameParado);
            ResumeGame();
        }
    }

    private void OnDestroy()
    {
        m_Player.Input.FindActionMap("PlayerActions").FindAction("Pause").started -= PararLaPartida;
    }
}
