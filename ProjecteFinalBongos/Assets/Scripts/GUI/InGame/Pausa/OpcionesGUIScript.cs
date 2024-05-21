using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpcionesGUIScript : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown m_IdiomasDropDown;
    [SerializeField] private Toggle m_FullScreenToggle;
    [SerializeField] private TMP_Dropdown m_ResolucionesDropDown;

    Resolution[] resoluciones;
    List<IdiomaEnum> m_IdiomaList = new();
    // Start is called before the first frame update
    void Start()
    {
        SettearIdiomasDropDown();
        SetDropDownValue();
        m_IdiomasDropDown.onValueChanged.AddListener(LanguageChanged);
        m_FullScreenToggle.onValueChanged.AddListener(CheckFullScreen);
        m_ResolucionesDropDown.onValueChanged.AddListener(CambiarResolucion);
        CheckResolutions();
    }

    private void CheckResolutions()
    {
        resoluciones = Screen.resolutions;
        m_ResolucionesDropDown.ClearOptions();
        List<string> opciones = new();
        int resolucionActual = 0;

        for(int i = 0; i < resoluciones.Length; i++)
        {
            string opcion = resoluciones[i].width + "x" + resoluciones[i].height;
            opciones.Add(opcion);

            if(Screen.fullScreen && resoluciones[i].width == Screen.currentResolution.width && resoluciones[i].height == Screen.currentResolution.height)
            {
                resolucionActual = i;
            }
        }

        m_ResolucionesDropDown.AddOptions(opciones);
        m_ResolucionesDropDown.value = resolucionActual;
        m_ResolucionesDropDown.RefreshShownValue();
    }

    private void CambiarResolucion(int indiceResolucion)
    {
        Resolution resolucion = resoluciones[indiceResolucion];
        Screen.SetResolution(resolucion.width, resolucion.height, Screen.fullScreen);
    }

    private void CheckFullScreen(bool fullScreen)
    {
        if (fullScreen)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }
    }

    private void LanguageChanged(int idioma)
    {
        GameManager.Instance.ChangeLanguage(m_IdiomaList[idioma]);
    }

    private void SettearIdiomasDropDown()
    {
        m_IdiomasDropDown.options.Clear();
        foreach (IdiomaEnum idioma in Enum.GetValues(typeof(IdiomaEnum)))
        {
            TMP_Dropdown.OptionData opcion = new TMP_Dropdown.OptionData();
            opcion.text = TranslateEnumLanguage(idioma);
            m_IdiomaList.Add(idioma);
            m_IdiomasDropDown.options.Add(opcion);
        }
    }

    private void SetDropDownValue()
    {
        for(int i = 0; i < m_IdiomaList.Count; i++)
        {
            if (m_IdiomaList[i] == GameManager.Instance.IdiomaJuego)
            {
                m_IdiomasDropDown.value = i;
                break;
            }
        }
    }

    private string TranslateEnumLanguage(IdiomaEnum idioma)
    {
        switch (idioma)
        {
            case IdiomaEnum.ES:
                return "Español";
            case IdiomaEnum.EN:
                return "English";
            case IdiomaEnum.IT:
                return "Italiano";
            case IdiomaEnum.CH:
                return "陈航";
            default:
                return null;

        }
    }
}
