using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldController : MonoBehaviour
{
    private const int DINEROINICIAL = 0;

    [SerializeField]
    private int m_DINERO = DINEROINICIAL;
    public int DINERO => m_DINERO;

    public void AddDinero(int _Dinero)
    {
        m_DINERO += _Dinero;
    }

    public void RemoveDinero(int _Dinero)
    {
        m_DINERO -= _Dinero;
    }
}
