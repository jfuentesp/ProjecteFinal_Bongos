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
        private int m_NumeroDeLista;
        [SerializeField] private FloatEvent m_EventoNuevaIdPartida;

        private void Awake()
        {
            m_ActionButton = GetComponent<Button>();    
        }

        public void Init(TipoDeBotonCargarBorrarNuevaPartidaEnum _ButtonType, string namePlayer, MundoEnum _Mundo, int _NumeroDeLista)
        {
            m_ActionButton.onClick.RemoveAllListeners();
            m_NumeroDeLista = _NumeroDeLista;
            m_Name = namePlayer;
            switch (_Mundo)
            {
                case MundoEnum.MUNDO_UNO:
                    m_Mundo = "Mundo1";
                    break;
                case MundoEnum.MUNDO_DOS:
                    m_Mundo = "Mundo2";
                    break;
            }

            switch (_ButtonType)
            {
                case TipoDeBotonCargarBorrarNuevaPartidaEnum.NUEVA:
                    m_ActionButton.onClick.AddListener(NewWorld);
                    break;
                case TipoDeBotonCargarBorrarNuevaPartidaEnum.CARGAR:
                    m_ActionButton.onClick.AddListener(LoadWorld);
                    break;
                case TipoDeBotonCargarBorrarNuevaPartidaEnum.BORRAR:
                    m_ActionButton.onClick.AddListener(DeleteWorld);
                    break;
            }
        }

        private void NewWorld()
        {
            m_EventoNuevaIdPartida.Raise(m_NumeroDeLista);
        }

        private void LoadWorld()
        {
            GameManager.Instance.LoadGameOfPlayer(m_Name, m_Mundo);
        }

        private void DeleteWorld()
        {
            GameManager.Instance.DeletePlayerGame(m_Name , m_NumeroDeLista);
        }
    }
}