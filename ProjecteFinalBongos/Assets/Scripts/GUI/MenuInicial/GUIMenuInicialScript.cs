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

        [Header("Menu Inicial")]
        [SerializeField] private GameObject m_MenuInicialPanel;
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

        [Header("Menu Opciones")]
        [SerializeField] private GameObject m_MenuOptionsPanel;
        [SerializeField] private Button m_BackOptionsButton;

        [Header("Menu Ranking")]
        [SerializeField] private GameObject m_MenuRankingPanel;
        [SerializeField] private Button m_BackRankingButton;

        private List<GameObject> m_PanelsList;
        private int m_IdPartidaNueva;

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.OnPlayerDeleted += RefreshPlayersFromStartGame;
            if (m_OptionsButton) m_OptionsButton.onClick.AddListener(Options);
            if (m_RankingsButton) m_RankingsButton.onClick.AddListener(Ranking);
            if (m_ExitButton) m_ExitButton.onClick.AddListener(CloseGame);
            if (m_StartButton) m_StartButton.onClick.AddListener(SlotsGame);
            if (m_StartNewGameButton) m_StartNewGameButton.onClick.AddListener(GuardarJugador);
            if (m_BackSlotsButton) m_BackSlotsButton.onClick.AddListener(BackMainMenu);
            if (m_BackNewGameButton) m_BackNewGameButton.onClick.AddListener(BackMainMenu);
            if (m_BackOptionsButton) m_BackOptionsButton.onClick.AddListener(BackMainMenu);
            if (m_BackRankingButton) m_BackRankingButton.onClick.AddListener(BackMainMenu);
            m_NewNameInput.onValueChanged.AddListener(ValidateIpInputField);
            m_StartNewGameButton.interactable = false;

            m_PanelsList = new()
            {
                m_MenuInicialPanel,
                m_MenuNewGamePanel,
                m_MenuOptionsPanel,
                m_MenuRankingPanel,
                m_MenuSlotsGamesPanel 
            };

            ClosePanelsInsteadOf(TypeOfPanels.INICIAL);
        }
        private void OnDestroy()
        {
            if(GameManager.Instance != null)
                GameManager.Instance.OnPlayerDeleted -= RefreshPlayersFromStartGame;    
            if (m_OptionsButton) m_OptionsButton.onClick.RemoveAllListeners();
            if (m_RankingsButton) m_RankingsButton.onClick.RemoveAllListeners();
            if (m_ExitButton) m_ExitButton.onClick.RemoveAllListeners();
            if (m_StartButton) m_StartButton.onClick.RemoveAllListeners();
            if (m_StartNewGameButton) m_StartNewGameButton.onClick.RemoveAllListeners();
            if (m_BackSlotsButton) m_BackSlotsButton.onClick.RemoveAllListeners();
            if (m_BackNewGameButton) m_BackNewGameButton.onClick.RemoveAllListeners();
            if (m_BackOptionsButton) m_BackOptionsButton.onClick.RemoveAllListeners();
            if (m_BackRankingButton) m_BackRankingButton.onClick.RemoveAllListeners();
            m_NewNameInput.onValueChanged.RemoveAllListeners();
        }
        private void SlotsGame()
        {
            ClosePanelsInsteadOf(TypeOfPanels.START_GAME);
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
            if(text.ToLower() == "empty")
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
                GameManager.Instance.SavePlayersAndTheirWorld(m_NewNameInput.text, m_IdPartidaNueva);
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
                if(GameManager.Instance.PlayersAndTheirWorldsList[i].m_Name == "EMPTY")
                {
                    m_PartidasSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameManager.Instance.PlayersAndTheirWorldsList[i].m_Name;
                    m_PartidasSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = GameManager.Instance.PlayersAndTheirWorldsList[i].m_Mundo.ToString();
                    m_PartidasSlots[i].transform.GetChild(2).GetComponent<LoadDeleteGame>().Init(TipoDeBotonCargarBorrarNuevaPartidaEnum.NUEVA, GameManager.Instance.PlayersAndTheirWorldsList[i].m_Name, GameManager.Instance.PlayersAndTheirWorldsList[i].m_Mundo, i);
                    m_PartidasSlots[i].transform.GetChild(3).GetComponent<Button>().interactable = false;
                    m_PartidasSlots[i].transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "New Game";
                }
                else
                {
                    m_PartidasSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameManager.Instance.PlayersAndTheirWorldsList[i].m_Name;
                    m_PartidasSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = GameManager.Instance.PlayersAndTheirWorldsList[i].m_Mundo.ToString();
                    m_PartidasSlots[i].transform.GetChild(2).GetComponent<LoadDeleteGame>().Init(TipoDeBotonCargarBorrarNuevaPartidaEnum.CARGAR, GameManager.Instance.PlayersAndTheirWorldsList[i].m_Name, GameManager.Instance.PlayersAndTheirWorldsList[i].m_Mundo, i);
                    m_PartidasSlots[i].transform.GetChild(3).GetComponent<LoadDeleteGame>().Init(TipoDeBotonCargarBorrarNuevaPartidaEnum.BORRAR, GameManager.Instance.PlayersAndTheirWorldsList[i].m_Name, GameManager.Instance.PlayersAndTheirWorldsList[i].m_Mundo, i);
                    m_PartidasSlots[i].transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Load Game";
                }
            }
        }

        public void NewGameId(float id)
        {
            m_IdPartidaNueva = (int)id;
            NewGame();
        }

        private void NewGame()
        {
            ClosePanelsInsteadOf(TypeOfPanels.NEW_GAME);
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