using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour
{
    [SerializeField]
    private Consumable m_Scriptable;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Presiono espacio");
            m_Scriptable.OnUse(gameObject);
        }
    }
}
