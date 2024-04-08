using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectrifiedObstacle : MonoBehaviour
{
    private bool m_IsElectrified;
    public bool IsElectrified => m_IsElectrified;

    private Animator m_Animator;

    private void Awake()
    {
        m_IsElectrified = false;
        m_Animator = GetComponent<Animator>();
    }

}
