using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DracMVaporSplash : Splash
{
    [SerializeField]
    private float m_Damage;
    public float Damage => m_Damage;
    private BossAttackDamage m_AttackDamage;
    private Animator m_Animator;
    [SerializeField] private string Animation;
    private CircleCollider2D m_CircleCollider;
    private void Awake()
    {
        m_CircleCollider = GetComponent<CircleCollider2D>();
        m_Animator = GetComponent<Animator>();
        m_AttackDamage = GetComponent<BossAttackDamage>();
    }
    public override void Init()
    {
        base.Init();
        m_Animator.Play(Animation);
        m_AttackDamage.SetDamage(m_Damage);
        StartCoroutine(DamageRoutine()) ;
        gameObject.tag = "Untagged";
    }
    public void ChangeLayer(string Layer) { 
        gameObject.layer = LayerMask.NameToLayer(Layer);
    }


    private IEnumerator DamageRoutine() {
        while (true) { 
            m_CircleCollider.enabled = false;
            yield return new WaitForSeconds(0.2f);
            m_CircleCollider.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnDisable()
    {
        print("adsadasdadadadadadadadadadadadadadadadadadadadad");
        StopCoroutine(DamageRoutine());
    }
}
