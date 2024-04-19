using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiccoloChadScript : MonoBehaviour
{
    [SerializeField] private List<LevelManager.ObjetosDisponibles> m_ObjetosDisponibles = new();

    public void Init(List<LevelManager.ObjetosDisponibles> _ObjetosDisponibles)
    {
        m_ObjetosDisponibles = _ObjetosDisponibles;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
