using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraBehaviour : MonoBehaviour
{
    private Transform m_Player;
    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameManager.Instance.PlayerInGame.transform;       
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Player != null)
            transform.position = new Vector3(m_Player.position.x, m_Player.position.y, -10);
    }
}
