using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgullaDeCoralFlipState : SMBBasicAttackState
{
    public Action OnAttackParried;
    public Action OnAttackHitted;
    public Action OnAttackMissed;

    [Header("Attack Animation")]
    [SerializeField]
    private string m_AgullaCoralFlipAttackAnimationName;


    protected override void Awake()
    {
        base.Awake();
    }

    public override void InitState()
    {
        base.InitState();
        m_Animator.Play(m_AgullaCoralFlipAttackAnimationName);
        m_Boss.SetBusy(true);
    }

    public override void ExitState()
    {
        base.ExitState();
       
    }
    private void AcabarAtaque()
    {
        OnAttackMissed?.Invoke();
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
            print("eyeyeyeyeyeyeyeyey");
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (collision.gameObject.GetComponent<SMBPlayerParryState>().parry)
            {
                print("Parried");
                OnAttackParried?.Invoke();
            }
            else
            {
                print("No Parried");
                OnAttackHitted?.Invoke();
            }
        }
    }
}
