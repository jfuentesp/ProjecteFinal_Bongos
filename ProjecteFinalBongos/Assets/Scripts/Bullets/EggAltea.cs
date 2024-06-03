using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EggAltea : Splash
{
    [SerializeField]
    private GameObject m_SerpientePrefab;

    private Transform m_Target;
    private Transform m_TransformSala;
    private Transform m_TransformBoss;
    private bool m_Nacido;
    private Animator m_Animator;
    [SerializeField] private AnimationClip[] m_AnimationsClips;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void Init(Transform _Target, Transform parent, Transform AlteaTransform)
    {
        m_TransformSala = parent;
        m_TransformBoss = AlteaTransform;
        if (m_TransformBoss.TryGetComponent<AlteaBossBehaviour>(out AlteaBossBehaviour altea))
            altea.OnBossDeath += BossMuerto;
        m_Size = new Vector2(m_SizeWideness, m_SizeLength);
        transform.localScale = m_Size;
        m_Target = _Target;
        m_Nacido = false;
        StartCoroutine(BornSNake());
    }

    private void BossMuerto()
    {
        StopAllCoroutines();
        m_Animator.Play("EggAlteaBorn");
        if (m_TransformBoss.TryGetComponent<AlteaBossBehaviour>(out AlteaBossBehaviour altea))
            altea.SnakeMuerta();
    }

    protected virtual IEnumerator BornSNake()
    {
        PlayRandomAnimation();
        yield return new WaitForSeconds(m_TimeUntilDestroyed);
        m_Nacido = true;
        m_Animator.Play("EggAlteaBorn");
    }

    private void PlayRandomAnimation()
    {
        m_Animator.Play(m_AnimationsClips[Random.Range(0, m_AnimationsClips.Length)].name);
    }

    private void OnDisable()
    {
        if(m_TransformBoss != null)
        {
            if (m_TransformBoss.TryGetComponent<AlteaBossBehaviour>(out AlteaBossBehaviour altea))
                altea.OnBossDeath -= BossMuerto;
        }
        

        DisableBullet();
    }

    private void OnSnakeBorned()
    {
        if (m_Nacido)
        {
            GameObject Serpiente = Instantiate(m_SerpientePrefab, m_TransformSala);
            Serpiente.transform.position = transform.position;
            Serpiente.GetComponent<BossBehaviour>().BossFinalSalaSpawn(m_Target);
            Serpiente.GetComponent<EnemySnake>().SetAlteaTransform(m_TransformBoss);
        }
    }
    private void EndEgg()
    {
        this.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled)
            return;
        if(collision.gameObject.layer == LayerMask.NameToLayer("PlayerHitBox"))
        {
            StopAllCoroutines();
            m_Animator.Play("EggAlteaBorn");
            if (m_TransformBoss.TryGetComponent<AlteaBossBehaviour>(out AlteaBossBehaviour altea))
                altea.SnakeMuerta();
        }
    }
}
