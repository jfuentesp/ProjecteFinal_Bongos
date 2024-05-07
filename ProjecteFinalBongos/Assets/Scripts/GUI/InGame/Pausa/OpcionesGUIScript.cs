using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class OpcionesGUIScript : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown m_DropDown;
    [SerializeField] private Font m_Font;
    List<IdiomaEnum> m_IdiomaList = new();
    // Start is called before the first frame update
    void Start()
    {
        SettearIdiomasDropDown();
        SetDropDownValue();
        m_DropDown.onValueChanged.AddListener(LanguageChanged);
    }

    private void LanguageChanged(int idioma)
    {
        GameManager.Instance.ChangeLanguage(m_IdiomaList[idioma]);
    }

    private void SettearIdiomasDropDown()
    {
        m_DropDown.options.Clear();
        foreach (IdiomaEnum idioma in Enum.GetValues(typeof(IdiomaEnum)))
        {
            TMP_Dropdown.OptionData opcion = new TMP_Dropdown.OptionData();
            opcion.text = TranslateEnumLanguage(idioma);
            m_IdiomaList.Add(idioma);
            m_DropDown.options.Add(opcion);
        }
    }

    private void SetDropDownValue()
    {
        for(int i = 0; i < m_IdiomaList.Count; i++)
        {
            if (m_IdiomaList[i] == GameManager.Instance.IdiomaJuego)
            {
                m_DropDown.value = i;
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
