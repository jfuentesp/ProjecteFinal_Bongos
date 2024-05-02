using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GUIScripts
{
    public class LoadDeleteGame : MonoBehaviour
    {
        private Button m_ActionButton;
        private string m_Name;
        private string m_Mundo;

        private void Awake()
        {
            m_ActionButton = GetComponent<Button>();    
        }

        public void Init(bool _Load, string namePlayer, GameManager.Mundo _Mundo)
        {
            m_Name = namePlayer;
            switch (_Mundo)
            {
                case GameManager.Mundo.MUNDO_UNO:
                    m_Mundo = "Mundo1";
                    break;
                case GameManager.Mundo.MUNDO_DOS:
                    m_Mundo = "Mundo2";
                    break;
            }

            if (_Load)
            {
                m_ActionButton.onClick.AddListener(DeleteWorld);
            }
            else
            {
                m_ActionButton.onClick.AddListener(LoadWorld);
            }
        }

        private void LoadWorld()
        {
            print($"Cargando {m_Mundo} del jugador: {m_Name}");
            GameManager.Instance.LoadGameOfPlayer(m_Name, m_Mundo);
        }

        private void DeleteWorld()
        {
            GameManager.Instance.DeletePlayerGame(m_Name);
        }
    }
}