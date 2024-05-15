using JetBrains.Annotations;
using SaveLoadGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

namespace GUIScripts
{
    public class GUIMenuInicialScript : MonoBehaviour
    {
        public enum TypeOfPanels { INICIAL, NEW_GAME, LOAD_GAME, OPTIONS, RANKING, START_GAME };


        [Header("Menu Inicial")]
        [SerializeField] private GameObject m_MenuInicialPanel;
        [SerializeField] private Button m_NewGameButton;
        [SerializeField] private Button m_LoadGameButton;
        [SerializeField] private Button m_OptionsButton;
        [SerializeField] private Button m_RankingsButton;
        [SerializeField] private Button m_ExitButton;
        [SerializeField] private Button m_StartButton;

        [Header("Menu Start Slots")]
        [SerializeField] private GameObject m_MenuSlotsGamesPanel;
        [SerializeField] private GameObject[] m_PartidasSlots = new GameObject[3];
        [SerializeField] private Button m_BackSlotsButton;


        [Header("Menu Nueva Partida")]
        [SerializeField] private GameObject m_MenuNewGamePanel;
        [SerializeField] private Button m_StartNewGameButton;
        [SerializeField] private Button m_BackNewGameButton;
        [SerializeField] private TMP_InputField m_NewNameInput;

        [Header("Menu Cargar Partida")]
        [SerializeField] private GameObject m_MenuLoadGamePanel;
        [SerializeField] private Button m_BackLoadGameButton;
        [SerializeField] private Transform m_SavedGamesGridParent;
        [SerializeField] private GameObject m_SavedPlayerPrefab;

        [Header("Menu Opciones")]
        [SerializeField] private GameObject m_MenuOptionsPanel;
        [SerializeField] private Button m_BackOptionsButton;

        [Header("Menu Ranking")]
        [SerializeField] private GameObject m_MenuRankingPanel;
        [SerializeField] private Button m_BackRankingButton;

        private List<GameObject> m_PanelsList;

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.OnPlayerDeleted += RefreshPlayersFromStartGame;
            GameManager.Instance.onGetPlayers += RefreshPrincipalButtons;
            if (m_NewGameButton) m_NewGameButton.onClick.AddListener(NewGame);
            if (m_LoadGameButton) m_LoadGameButton.onClick.AddListener(LoadGame);
            if (m_OptionsButton) m_OptionsButton.onClick.AddListener(Options);
            if (m_RankingsButton) m_RankingsButton.onClick.AddListener(Ranking);
            if (m_ExitButton) m_ExitButton.onClick.AddListener(CloseGame);
            if (m_StartButton) m_StartButton.onClick.AddListener(SlotsGame);
            if (m_StartNewGameButton) m_StartNewGameButton.onClick.AddListener(GuardarJugador);
            if (m_BackSlotsButton) m_BackSlotsButton.onClick.AddListener(BackMainMenu);
            if (m_BackNewGameButton) m_BackNewGameButton.onClick.AddListener(BackMainMenu);
            if (m_BackLoadGameButton) m_BackLoadGameButton.onClick.AddListener(BackMainMenu);
            if (m_BackOptionsButton) m_BackOptionsButton.onClick.AddListener(BackMainMenu);
            if (m_BackRankingButton) m_BackRankingButton.onClick.AddListener(BackMainMenu);
            m_NewNameInput.onValueChanged.AddListener(ValidateIpInputField);
            m_StartNewGameButton.interactable = false;

            m_PanelsList = new()
            {
                m_MenuInicialPanel,
                m_MenuNewGamePanel,
                m_MenuLoadGamePanel,
                m_MenuOptionsPanel,
                m_MenuRankingPanel
            };

            ClosePanelsInsteadOf(TypeOfPanels.INICIAL);
        }

        private void SlotsGame()
        {
            ClosePanelsInsteadOf(TypeOfPanels.START_GAME);
        }

        void RefreshPrincipalButtons()
        {
            if (GameManager.Instance.PlayersAndTheirWorldsList.Count > 2)
                m_NewGameButton.interactable = (false);
            else
                m_NewGameButton.interactable = (true);
            if(GameManager.Instance.PlayersAndTheirWorldsList.Count == 0)
                m_LoadGameButton.interactable = (false);
            else
                m_LoadGameButton.interactable = (true);
        }

       

        private void ValidateIpInputField(string text)
        {
            if (text.Length >= 2 && text.Length <= 10)
            {
                m_StartNewGameButton.interactable = true;
            }
            else
            {
                m_StartNewGameButton.interactable = false;
            }
        }

        private void GuardarJugador()
        {
            if (GameManager.Instance.PlayerExists(m_NewNameInput.text))
            {
                print("Este tio existe bro");
            }
            else
            {
                GameManager.Instance.SavePlayersAndTheirWorld(m_NewNameInput.text);
                GameManager.Instance.CreateNewGameOfPlayer(m_NewNameInput.text, GameManager.Instance.NombreDeTuEscena);
            }
        }

        private void BackMainMenu()
        {
            ClosePanelsInsteadOf(TypeOfPanels.INICIAL);
        }

        private void ClosePanelsInsteadOf(TypeOfPanels panel)
        {
            foreach (GameObject panelin in m_PanelsList)
                panelin.SetActive(false);

            switch (panel)
            {
                case TypeOfPanels.INICIAL:
                    m_MenuInicialPanel.SetActive(true);
                    RefreshPrincipalButtons();
                    break;
                case TypeOfPanels.NEW_GAME:
                    m_MenuNewGamePanel.SetActive(true);
                    break;
                case TypeOfPanels.OPTIONS:
                    m_MenuOptionsPanel.SetActive(true);
                    break;
                case TypeOfPanels.RANKING:
                    m_MenuRankingPanel.SetActive(true);
                    break;
                case TypeOfPanels.START_GAME:
                    m_MenuSlotsGamesPanel.SetActive(true);
                    RefreshPlayersFromStartGame();
                    break;
            }
        }
        private void RefreshPlayersFromStartGame()
        {
            for (int i = 0; i < 3; i++)
            {
                m_PartidasSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameManager.Instance.PlayersAndTheirWorldsList[i].m_Name;
                m_PartidasSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = GameManager.Instance.PlayersAndTheirWorldsList[i].m_Mundo.ToString();
                m_PartidasSlots[i].transform.GetChild(2).GetComponent<LoadDeleteGame>().Init(false, GameManager.Instance.PlayersAndTheirWorldsList[i].m_Name, GameManager.Instance.PlayersAndTheirWorldsList[i].m_Mundo);
                m_PartidasSlots[i].transform.GetChild(3).GetComponent<LoadDeleteGame>().Init(true, GameManager.Instance.PlayersAndTheirWorldsList[i].m_Name, GameManager.Instance.PlayersAndTheirWorldsList[i].m_Mundo);
            }
        }

        private void NewGame()
        {
            ClosePanelsInsteadOf(TypeOfPanels.NEW_GAME);
        }
        private void LoadGame()
        {
            ClosePanelsInsteadOf(TypeOfPanels.LOAD_GAME);
        }
        private void Options()
        {
            ClosePanelsInsteadOf(TypeOfPanels.OPTIONS);
        }
        private void Ranking()
        {
            ClosePanelsInsteadOf(TypeOfPanels.RANKING);
        }
        private void CloseGame()
        {
            UnityEngine.Application.Quit();
        }
    }
}