using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectOrMusicScript : MonoBehaviour
{
    [SerializeField]
    private SoundTypeEnum m_SoundType;
    private AudioSource m_AudioSource;
    private bool m_Listener;

    // Start is called before the first frame update
    void Start()
    {
        print(GameManager.Instance.SoundManager == null);
        if(GameManager.Instance != null)
        {
            switch (m_SoundType)
            {
                case SoundTypeEnum.EFFECT:
                    m_Listener = false;
                    m_AudioSource = GetComponent<AudioSource>();
                    GameManager.Instance.SoundManager.OnEffectVolumeCHanged += SetVolume;
                    SetVolume(GameManager.Instance.SoundManager.EffectVolume);
                    break;
                case SoundTypeEnum.MUSIC:
                    m_Listener = false;
                    m_AudioSource = GetComponent<AudioSource>();
                    GameManager.Instance.SoundManager.OnMusicVolumeChanged += SetVolume;
                    SetVolume(GameManager.Instance.SoundManager.MusicVolume);
                    break;
                case SoundTypeEnum.GLOBAL:
                    m_Listener = true;
                    GameManager.Instance.SoundManager.OnTotalVolumeChanged += SetVolume;
                    SetVolume(GameManager.Instance.SoundManager.TotalVolume);
                    break;
            }
        }
    }

    private void SetVolume(float _Volume)
    {
        if (m_Listener)
            AudioListener.volume = _Volume;
        else
            m_AudioSource.volume = _Volume;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            switch (m_SoundType)
            {
                case SoundTypeEnum.EFFECT:
                    GameManager.Instance.SoundManager.OnEffectVolumeCHanged -= SetVolume;
                    break;
                case SoundTypeEnum.MUSIC:
                    GameManager.Instance.SoundManager.OnMusicVolumeChanged -= SetVolume;
                    break;
                case SoundTypeEnum.GLOBAL:
                    GameManager.Instance.SoundManager.OnTotalVolumeChanged -= SetVolume;
                    break;
            }
        }
    }
}
