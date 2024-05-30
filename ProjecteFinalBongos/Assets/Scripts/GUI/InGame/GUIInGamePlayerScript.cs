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

    [Header("Paneles de la interfaz")]
    [SerializeField] private GameObject m_PanelInventory;
    [SerializeField] private GameObject m_PanelPlayerHUD;
    [SerializeField] private GameObject m_PanelAbilities;
    [SerializeField] private GameObject m_PanelStore;
    [SerializeField] private List<GameObject> m_PanelsList;

    private InventoryController m_Inventory;

    void Start()
    {
        m_Player = PJSMB.Instance;
        m_Inventory = LevelManager.Instance.InventoryController;
        if (m_ResumeGameButton) m_ResumeGameButton.onClick.AddListener(ResumeGame);
        m_PausaPanel.SetActive(false);
        m_Player.Input.FindActionMap("PlayerActions").FindAction("Pause").started += PararLaPartida;
        m_Player.Input.FindActionMap("PlayerActions").FindAction("OpenAbilities").performed += OpenAbilities;
        m_Player.Input.FindActionMap("PlayerActions").FindAction("OpenInventory").performed += OpenInventory;
    }


    private void ResumeGame()
    {
        m_GameParado = false;
        GameManager.Instance.PauseGame(m_GameParado);
        ClosePanelsInsteadOf(TypeOfPanels.INICIAL);
    }

    private void PararLaPartida(InputAction.CallbackContext context)
    {
        if (!m_GameParado)
        {
            m_GameParado = true;
            ClosePanelsInsteadOf(TypeOfPanels.PAUSE);
            GameManager.Instance.PauseGame(m_GameParado);
        }
        else
        {
            ResumeGame();
        }
    }

    private void OpenAbilities(InputAction.CallbackContext context)
    {
        if (m_GameParado)
            return;
        if (m_PanelAbilities.activeInHierarchy)
        {
            PJSMB.Instance.GetComponent<SMBPlayerStopState>().Exit();
            ClosePanelsInsteadOf(TypeOfPanels.INICIAL);
        }
        else {
            ClosePanelsInsteadOf(TypeOfPanels.ABILITIES);
            PJSMB.Instance.StopPlayer();
        }
           
    }

    private void OpenInventory(InputAction.CallbackContext context) 
    {
        if (m_GameParado)
            return;
        if (m_PanelInventory.activeInHierarchy)
        {
            PJSMB.Instance.GetComponent<SMBPlayerStopState>().Exit();
            ClosePanelsInsteadOf(TypeOfPanels.INICIAL);
        }
        else
        {
            PJSMB.Instance.StopPlayer();
            m_Inventory.RefreshInventoryGUI();
            ClosePanelsInsteadOf(TypeOfPanels.INVENTORY);
        }
    }

    private void OpenStore(InputAction.CallbackContext context)
    {
        ClosePanelsInsteadOf(TypeOfPanels.STORE);
    }

    private void OnDestroy()
    {
        m_Player.Input.FindActionMap("PlayerActions").FindAction("Pause").started -= PararLaPartida;
        m_Player.Input.FindActionMap("PlayerActions").FindAction("OpenAbilities").performed -= OpenAbilities;
        m_Player.Input.FindActionMap("PlayerActions").FindAction("OpenInventory").performed -= OpenInventory;
    }

    public void ClosePanelsInsteadOf(TypeOfPanels panel)
    {
        foreach (GameObject panelin in m_PanelsList)
            panelin.SetActive(false);

        switch (panel)
        {
            case TypeOfPanels.INICIAL:
                m_PanelPlayerHUD.SetActive(true);
                break;
            case TypeOfPanels.ABILITIES:
                m_PanelAbilities.SetActive(true);
                break;
            case TypeOfPanels.STORE:
                m_PanelStore.SetActive(true);
                break;
            case TypeOfPanels.INVENTORY:
                m_PanelInventory.SetActive(true);
                break;
            case TypeOfPanels.PAUSE:
                m_PausaPanel.SetActive(m_GameParado);
                break;
            case TypeOfPanels.OPTIONS:

                break;
        }
    }
}
