using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloodController : MonoBehaviour
{
    [SerializeField] private Color[] m_BloodColorList;
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
            
    }

    public void PlayBlood()
    {
        if (gameObject != null)
        {
            gameObject.SetActive(true);
            m_SpriteRenderer.material.SetColor("_Color", m_BloodColorList[Random.Range(0, m_BloodColorList.Length)]);
            m_Animator.Play(m_AnimationsList[Random.Range(0, m_AnimationsList.Length)].name);
        }
    }

    public void PlayDeathBlood()
    {
        if(gameObject != null)
        {
            gameObject.SetActive(true);
            m_SpriteRenderer.material.SetColor("_Color", m_BloodColorList[Random.Range(0, m_BloodColorList.Length)]);
            m_Animator.Play("Muerto");
        }
    }

    private void EndAnimation()
    {
        gameObject.SetActive(false);
    }
}
