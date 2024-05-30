using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private float m_TotalVolume;
    public float TotalVolume => m_TotalVolume;
    [SerializeField] private float m_MusicVolume;
    public float MusicVolume => m_MusicVolume;
    [SerializeField] private float m_EffectVolume;
    public float EffectVolume => m_EffectVolume;

    public Action<float> OnTotalVolumeChanged;
    public Action<float> OnMusicVolumeChanged;
    public Action<float> OnEffectVolumeCHanged;

    public void SetTotalVolume(float _TotalVolume)
    {
        m_TotalVolume = _TotalVolume;
        OnTotalVolumeChanged?.Invoke(m_TotalVolume);
    }
    public void SetMusicVolume(float _MusicVolume)
    {
        m_MusicVolume = _MusicVolume;
        OnMusicVolumeChanged?.Invoke(m_MusicVolume);
    }
    public void SetEffectVolume(float _EffectVolume)
    {
        m_EffectVolume = _EffectVolume;
        OnEffectVolumeCHanged?.Invoke(m_EffectVolume);
    }
}
