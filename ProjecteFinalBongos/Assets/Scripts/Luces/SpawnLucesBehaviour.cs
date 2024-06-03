using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLucesBehaviour : MonoBehaviour
{
    private GameObject m_ComponenteLuz;
    private Animator m_Animator;
    private AudioSource m_AudioSource;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_AudioSource = transform.GetChild(transform.childCount - 1).GetComponent<AudioSource>();
        m_ComponenteLuz = transform.GetChild(transform.childCount - 1).gameObject;
    }
    private void Start()
    {
        LevelManager.Instance.SetSpawnLuces(this);
    }

    public void SetPositionLight(Vector3 newPosition)
    {
        m_Animator.Play("LucesMundo1");
        m_ComponenteLuz.transform.position = newPosition;
    }

    public void PlayThunderSound()
    {
        m_AudioSource.Play();
    }
}
