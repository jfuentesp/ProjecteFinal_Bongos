using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMergeState : SMState
{

    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;
    private Transform m_Target;

    public Action<GameObject> onAttackStopped;

    private new void Awake()
    {
        base.Awake();
        transform.up = Vector3.up;
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        m_Boss.OnPlayerInSala += GetTarget;
    }

    private void GetTarget()
    {
        m_Target = m_Boss.Target;
    }

    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true);
        m_Rigidbody.velocity = Vector3.zero;
        StartCoroutine(Teletransporte()); 
    }

    private IEnumerator Teletransporte() {
        print("Submerge");
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(4f);
        transform.position = m_Boss.SalaPadre.GetPosicionAleatoriaEnSala();
        print("Merge");
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        onAttackStopped.Invoke(gameObject);
    }  

    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
