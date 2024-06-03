using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomySoundScript : MonoBehaviour
{
    private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_AbilitySound;
    [SerializeField] private AudioClip m_GoldSound;

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInParent<GoldController>().onGainGold += Gold;
        GetComponentInParent<AbilityPointsController>().onAbilityPointGained += Points;
    }

    private void Points()
    {
        m_AudioSource.clip = m_AbilitySound;
        m_AudioSource.Play();
    }

    private void Gold()
    {
        m_AudioSource.clip = m_GoldSound;
        m_AudioSource.Play();
    }
    private void OnDestroy()
    {
        if (GetComponentInParent<GoldController>() != null)
        {
            GetComponentInParent<GoldController>().onGainGold -= Gold;
        }
        if (GetComponentInParent<AbilityPointsController>() != null)
        {
            GetComponentInParent<AbilityPointsController>().onAbilityPointGained -= Points;
        }
    }
}
