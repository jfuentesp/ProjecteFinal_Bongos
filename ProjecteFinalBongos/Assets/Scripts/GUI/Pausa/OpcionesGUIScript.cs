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
    // Start is called before the first frame update
    void Start()
    {
        SettearIdiomasDropDown();

    }

    private void SettearIdiomasDropDown()
    {
        m_DropDown.options.Clear();
        foreach (IdiomaEnum idioma in Enum.GetValues(typeof(IdiomaEnum)))
        {
            TMP_Dropdown.OptionData opcion = new TMP_Dropdown.OptionData();
            opcion.text = TranslateEnumLanguage(idioma);
            m_DropDown.options.Add(opcion);
        }
    }

    // Update is called once per frame
    void Update()
    {

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
