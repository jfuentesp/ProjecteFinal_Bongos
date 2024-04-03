using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBehaviour : MonoBehaviour
{
    private bool m_IsColliding;
    public bool isColliding => m_IsColliding;

    private void Awake()
    {
        m_IsColliding = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
