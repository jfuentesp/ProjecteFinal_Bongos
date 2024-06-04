using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleProtectionScript : MonoBehaviour
{
    
    [SerializeField] private float m_ZoomPadre;
    [SerializeField] private float m_ZoomHijo;
    private Material m_MaterialPadre;
    private Material m_MaterialHijo;
    [SerializeField] private int m_VecesQueTePegan;
    private float multiplicador;
    private float m_Diferenciador;
    private int m_Final;
    private float m_Dissolve;
    [SerializeField] private AudioClip[] m_AudioClipList;
    [SerializeField] private AudioClip m_BrokeClip;
    private AudioSource m_AudioSource;

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void Init(int veces)
    {
        m_VecesQueTePegan = veces;
        m_Final = m_VecesQueTePegan;
        m_Diferenciador = 0.1f;
        multiplicador = m_Diferenciador / m_VecesQueTePegan;
        m_MaterialHijo = transform.GetChild(0).GetComponent<SpriteRenderer>().material;
        m_MaterialPadre = GetComponent<SpriteRenderer>().material;
        SetVida(veces);
    }

    public void SetVida(int vez)
    {
        m_Final = vez;
        if (m_Final == 0)
        {
            m_AudioSource.clip = m_BrokeClip;
            m_AudioSource.Play();
            StartCoroutine(BreakBubble());
        }
        else
        {
            if(m_Final != m_VecesQueTePegan)
            {
                m_AudioSource.clip = m_AudioClipList[UnityEngine.Random.Range(0, m_AudioClipList.Length)];
                m_AudioSource.Play();
            }
            m_Dissolve = m_Final * multiplicador;
            m_MaterialHijo.SetFloat("_Dissolve", m_Dissolve);
            m_MaterialHijo.SetFloat("_ZoomNoise", m_ZoomHijo);
            m_MaterialPadre.SetFloat("_Dissolve", m_Dissolve);
            m_MaterialPadre.SetFloat("_ZoomNoise", m_ZoomPadre);
        }
    }

    private IEnumerator BreakBubble()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
