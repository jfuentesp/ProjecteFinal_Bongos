using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundHit : MonoBehaviour
{
    private bool m_Growing;

    [SerializeField]
    float m_GrowingDuration;
    float m_CurrentDuration;
    // Start is called before the first frame update
    void Start()
    {
        m_Growing = false;
        GetComponent<SMBParriedState>().OnRecomposited += StartGrowing;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(0).localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    private void StartGrowing(GameObject obj)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        m_Growing = true;
        m_CurrentDuration = 0;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (m_Growing)
        {
            m_CurrentDuration += Time.deltaTime;
            if (m_CurrentDuration >= m_GrowingDuration)
            {
                StopGrowing();
            }
            transform.GetChild(0).localScale += new Vector3(0.5f, 0.5f, 0.5f) * Time.deltaTime;
        }
    }

    private void StopGrowing()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(0).localScale = new Vector3(0.1f, 0.1f, 0.1f);
        m_Growing = false;
    }
}
