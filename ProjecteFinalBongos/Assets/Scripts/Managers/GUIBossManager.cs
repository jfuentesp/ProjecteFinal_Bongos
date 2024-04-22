using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIBossManager : MonoBehaviour
{
    [Header("Variables Boss Triton")]
    [SerializeField] private GameObject m_TritonPanel;
    [SerializeField] private GameObject m_FlechaTriton;


    // Start is called before the first frame update
    void Start()
    {
        m_TritonPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnFlecha(float degrees)
    {
        m_FlechaTriton.transform.localEulerAngles = new Vector3(0, 0, degrees);
        StartCoroutine(ActivarFlecha());
    }

    private IEnumerator ActivarFlecha()
    {
        m_TritonPanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        m_TritonPanel.SetActive(false);
        m_FlechaTriton.transform.localEulerAngles = new Vector3(0, 0, 0);
    }
}
