using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgullaDeCoralFlipState : SMBBasicAttackState
{
    public Action<GameObject> OnAttackParried;
    public Action<GameObject> OnAttackHitted;
    public Action<GameObject> OnAttackMissed;

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
        m_AtaqueFlipCoroutine = StartCoroutine(FlipCoroutine());
    }

    private IEnumerator FlipCoroutine()
    {
        m_AttackHitbox.gameObject.SetActive(true);
        int cont = 0;
        bool pepe = false;
        while (!pepe)
        {
            cont++;
            transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + 36);
            if (cont == 11)
                pepe = true;
            yield return new WaitForSeconds(.1f);
        }
        AcabarAtaque();
    }

    public override void ExitState()
    {
        base.ExitState();
        m_AttackHitbox.gameObject.SetActive(false);
        if (m_AtaqueFlipCoroutine != null)
            StopCoroutine(m_AtaqueFlipCoroutine);
    }
    private void AcabarAtaque()
    {
        m_AttackHitbox.gameObject.SetActive(false);
        OnAttackMissed?.Invoke(gameObject);
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
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (collision.gameObject.GetComponent<SMBPlayerParryState>().parry)
            {
                print("Parried");
                OnAttackParried?.Invoke(gameObject);
            }
            else
            {
                print("No Parried");
                OnAttackHitted?.Invoke(gameObject);
            }
        }
    }
}
