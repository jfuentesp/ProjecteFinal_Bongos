using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HurtsSounds : MonoBehaviour
{
    private AudioSource m_AudioSource;
    [SerializeField] private AudioClip[] m_AudioClipsList;
    [SerializeField] private AudioClip m_AudioDeath;

    private void Awake()
    {
        m_AudioSource= GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInParent<HealthController>().onHurt += Hurted;
        GetComponentInParent<HealthController>().onDeath += Dead;
    }

    private void Dead()
    {
        m_AudioSource.clip = m_AudioDeath;
        m_AudioSource.Play();
    }

    private void Hurted()
    {
        m_AudioSource.clip = m_AudioClipsList[Random.Range(0, m_AudioClipsList.Length)];
        m_AudioSource.Play();
    }
    private void OnDestroy()
    {
        if (GetComponentInParent<HealthController>() != null)
        {
            GetComponentInParent<HealthController>().onHurt -= Hurted;
            GetComponentInParent<HealthController>().onDeath -= Dead;
        }
    }
}
