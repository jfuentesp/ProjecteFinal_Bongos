using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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
    [SerializeField] private Button m_OptionsButton;
    [SerializeField] private Button m_ExitGameButton;

    [Header("Botones Exit")]
    [SerializeField] private Button m_ExitConfirmationButton;
    [SerializeField] private Button m_ExitBackButton;

    [Header("Paneles de la interfaz")]
    [SerializeField] private GameObject m_PanelInventory;
    [SerializeField] private GameObject m_PanelPlayerHUD;
    [SerializeField] private GameObject m_PanelAbilities;
    [SerializeField] private GameObject m_PanelStore;
    [SerializeField] private GameObject m_PanelOptions;
    [SerializeField] private GameObject m_PanelExitConfirmation;
    [SerializeField] private List<GameObject> m_PanelsList;

    private InventoryController m_Inventory;

    void Start()
    {
        m_Player = PJSMB.Instance;
        m_Inventory = LevelManager.Instance.InventoryController;
        if (m_ResumeGameButton) m_ResumeGameButton.onClick.AddListener(ResumeGame);
        if (m_OptionsButton) m_OptionsButton.onClick.AddListener(OpenOptions);
        if (m_ExitGameButton) m_ExitGameButton.onClick.AddListener(OpenExit);
        if (m_ExitConfirmationButton) m_ExitConfirmationButton.onClick.AddListener(OnExitConfirmation);
        if (m_ExitBackButton) m_ExitBackButton.onClick.AddListener(CancelExit);
        ClosePanelsInsteadOf(TypeOfPanels.INICIAL);
        m_Player.Input.FindActionMap("PlayerActions").FindAction("Pause").started += PararLaPartida;
        m_Player.Input.FindActionMap("PlayerActions").FindAction("OpenAbilities").performed += OpenAbilities;
        m_Player.Input.FindActionMap("PlayerActions").FindAction("OpenInventory").performed += OpenInventory;
    }

    private void OnDestroy()
    {
        m_ResumeGameButton.onClick.RemoveAllListeners();
        m_OptionsButton.onClick.RemoveAllListeners();
        m_ExitGameButton.onClick.RemoveAllListeners();
        m_ExitConfirmationButton.onClick.RemoveAllListeners();
        m_ExitBackButton.onClick.RemoveAllListeners();
        m_Player.Input.FindActionMap("PlayerActions").FindAction("Pause").started -= PararLaPartida;
        m_Player.Input.FindActionMap("PlayerActions").FindAction("OpenAbilities").performed -= OpenAbilities;
        m_Player.Input.FindActionMap("PlayerActions").FindAction("OpenInventory").performed -= OpenInventory;
    }

    /* MENU GUI BUTTONS */

    private void ResumeGame()
    {
        m_GameParado = false;
        GameManager.Instance.PauseGame(m_GameParado);
        ClosePanelsInsteadOf(TypeOfPanels.INICIAL);
    }

    private void OpenOptions()
    {
        ClosePanelsInsteadOf(TypeOfPanels.OPTIONS);
    }

    private void OpenExit()
    {
        ClosePanelsInsteadOf(TypeOfPanels.NEW_GAME);
    }

    /* OPTIONS GUI BUTTONS */


    /* EXIT GUI BUTTONS */

    private void OnExitConfirmation()
    {
        m_GameParado = false;
        GameManager.Instance.PauseGame(m_GameParado);
        LevelManager.Instance.FundirNegro(false);
    }

    private void CancelExit()
    {
        ClosePanelsInsteadOf(TypeOfPanels.PAUSE);
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
                m_PanelOptions.SetActive(true);
                break;
            case TypeOfPanels.NEW_GAME:
                m_PanelExitConfirmation.SetActive(true);
                break;
        }
    }
}
