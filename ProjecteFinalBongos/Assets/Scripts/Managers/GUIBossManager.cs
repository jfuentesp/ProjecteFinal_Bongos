using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIBossManager : MonoBehaviour
{
    [Header("Variables Boss Triton")]
    [SerializeField] private GameObject m_TritonPanel;
    [SerializeField] private GameObject m_FlechaTriton;
    [SerializeField] private GameObject m_KrakenPanel;
    [SerializeField] private GameObject m_TintaKraken;
    [SerializeField] private GameObject m_ButtonPadre;
    [SerializeField] private GameObject m_ButtonHijo;
    //[SerializeField] private Button

    private bool eoeoeoeoeoeoeoeoeoeoeoeoeoeoeoeoeoeo;

    // Start is called before the first frame update
    void Start()
    {
        m_TritonPanel.SetActive(false);
        eoeoeoeoeoeoeoeoeoeoeoeoeoeoeoeoeoeo = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (eoeoeoeoeoeoeoeoeoeoeoeoeoeoeoeoeoeo)
            {
                eoeoeoeoeoeoeoeoeoeoeoeoeoeoeoeoeoeo = false;
                LevelManager.Instance.InputSystemUIInputModule.xrTrackingOrigin = m_ButtonPadre.transform;
                LevelManager.Instance.EventSystem.firstSelectedGameObject = m_ButtonPadre;
            }
            else
            {
                eoeoeoeoeoeoeoeoeoeoeoeoeoeoeoeoeoeo = true;
                LevelManager.Instance.InputSystemUIInputModule.xrTrackingOrigin = m_ButtonHijo.transform;
                LevelManager.Instance.EventSystem.firstSelectedGameObject = m_ButtonHijo;
            }
        }
    }
    public void SpawnTinta() {
        StartCoroutine(TintaFadeOut());
    }
    public void SpawnFlecha(float degrees)
    {
        m_FlechaTriton.transform.localEulerAngles = new Vector3(0, 0, degrees);
        StartCoroutine(ActivarFlecha());
    }

    public void UploadFlecha(float degrees)
    {
        if(m_TritonPanel.activeSelf)
            m_FlechaTriton.transform.localEulerAngles = new Vector3(0, 0, degrees);
    }

    private IEnumerator ActivarFlecha()
    {
        m_TritonPanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        m_TritonPanel.SetActive(false);
        m_FlechaTriton.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    private IEnumerator TintaFadeOut() { 
        m_KrakenPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        while (m_TintaKraken.GetComponent<Image>().color.a > 0) {
            Color newColor = m_TintaKraken.GetComponent<Image>().color;
            m_TintaKraken.GetComponent<Image>().color = new Color(newColor.r, newColor.g, newColor.b, (newColor.a - 0.001f));
            yield return new WaitForSeconds(0.0075f);
        }
        m_KrakenPanel.SetActive(false);
        m_TintaKraken.GetComponent<Image>().color = new Color(m_TintaKraken.GetComponent<Image>().color.r, m_TintaKraken.GetComponent<Image>().color.g, m_TintaKraken.GetComponent<Image>().color.b,0.8f) ;
        Finalizar("TintaFadeOut");
    }

    private void Finalizar(string Rutina)
    {
        StopCoroutine(Rutina);
    }
}
