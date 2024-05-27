using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgullaDeCoralFlipState : SMBBasicAttackState
{
    public Action<GameObject> OnAttackParried;
    public Action<GameObject> OnAttackHitted;
    public Action<GameObject> OnAttackMissed;
    private bool m_Golpeado;

    [Header("Attack Animation")]
    [SerializeField]
    private string m_AgullaCoralFlipAttackAnimationName;

    private Coroutine m_AtaqueFlipCoroutine;

   
    protected override void Awake()
    {
        base.Awake();
    }

    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true);
        m_Golpeado = false;
        m_Animator.Play(m_AgullaCoralFlipAttackAnimationName);
    }

    public override void ExitState()
    {
        base.ExitState();
        m_Boss.SetBusy(false);
        m_AttackHitbox.gameObject.SetActive(false);
    }
    private void AcabarAtaque()
    {
        m_AttackHitbox.gameObject.SetActive(false);
        if(m_Golpeado)
        {
            OnAttackHitted?.Invoke(gameObject);
        }
        else
        {
            OnAttackMissed?.Invoke(gameObject);
        }
    }
    private void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!enabled)
            return;
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!enabled)
            return;
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!collision.gameObject.GetComponent<SMBPlayerParryState>().parry)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                print("No Parried");
                m_Golpeado = true;
            }
        }
    }
}
