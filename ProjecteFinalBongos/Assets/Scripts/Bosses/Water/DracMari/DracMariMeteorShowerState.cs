using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class DracMariMeteorShowerState : SMState
{
    [SerializeField] private GameObject m_meteor;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;

    public Action<GameObject> OnShowerFinished;

    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
    }

    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true);
        m_MeteorCoroutine = StartCoroutine(MeteorShower());
    }
    public override void ExitState()
    {
        base.ExitState();
        StopCoroutine(m_MeteorCoroutine);   
    }
    private Coroutine m_MeteorCoroutine;

    private IEnumerator MeteorShower() {
        int meteorNumber = Random.Range(5, 9);
        while (meteorNumber > 0) {
            float delayTime = Random.Range(0.2f, 0.6f);
            GameObject meteor = Instantiate(m_meteor);
            meteor.transform.position = m_Boss.SalaPadre.GetPosicionAleatoriaEnSala();
            meteor.GetComponent<Animator>().speed += Random.Range(-0.5f, 0.6f);
            meteorNumber--;
            yield return new WaitForSeconds(delayTime);

        }
        print("finished");
        OnShowerFinished(gameObject);
    }
}
