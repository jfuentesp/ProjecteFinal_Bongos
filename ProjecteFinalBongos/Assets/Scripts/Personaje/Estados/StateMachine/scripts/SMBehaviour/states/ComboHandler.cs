using m17;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboHandler : MonoBehaviour
{
    private bool m_ComboAvailable = false;
    public bool ComboAvailable => m_ComboAvailable;

    public event Action OnEndAction;

    private void Awake()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        m_ComboAvailable = false;
    }

    private void OnDisable()
    {
        m_ComboAvailable = false;
    }

    public void InitComboWindow()
    {
        m_ComboAvailable = true;
    }

    public void EndComboWindow()
    {
        m_ComboAvailable = false;
    }

    public void EndAction()
    {
        OnEndAction?.Invoke();
    }
}
