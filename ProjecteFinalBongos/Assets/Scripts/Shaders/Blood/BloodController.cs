using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloodController : MonoBehaviour
{
    [SerializeField] private bool m_MulticolorBlood;
    [SerializeField] private Color m_BloodColor;
    [SerializeField] private AnimationClip[] m_AnimationsList;
    private SpriteRenderer m_SpriteRenderer;
    private Animator m_Animator;


    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (!m_MulticolorBlood)
            m_SpriteRenderer.material.SetColor("_Color", m_BloodColor);
    }

    public void PlayBlood()
    {
        gameObject.SetActive(true);
        m_Animator.Play(m_AnimationsList[Random.Range(0, m_AnimationsList.Length)].name);
    }

    private void EndAnimation()
    {
        gameObject.SetActive(false);
    }
}
