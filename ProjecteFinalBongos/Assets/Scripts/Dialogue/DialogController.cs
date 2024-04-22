using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    private Animator m_Animator;
    private List<string> m_ListDialogs;
    Texts text;
    [SerializeField] private TextMeshProUGUI m_TextoPantalla;

    public void ActivarCartel(Texts textObject)
    {
        m_Animator.SetBool("Cartel", true);
        text = textObject;
    }

    public void ActivaTexto()
    {
        m_ListDialogs.Clear();
        foreach (string textoGuardar in text.arrayTexts)
        {
            m_ListDialogs.Add(textoGuardar);
        }
        m_ListDialogs.Shuffle();
        SiguienteFrase();
    }

    private void SiguienteFrase()
    {
        if(m_ListDialogs.Count == 0)
        {
            CierraCartel();
            return;
        }

        string fraseActual = m_ListDialogs[0];
        m_ListDialogs.RemoveAt(0);
        m_TextoPantalla.text = fraseActual;
    }

    private void CierraCartel()
    {
        m_Animator.SetBool("Cartel", false);
    }
}
