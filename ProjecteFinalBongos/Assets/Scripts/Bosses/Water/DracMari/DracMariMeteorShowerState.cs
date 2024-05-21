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
    [SerializeField] private string m_MeteorExplosionAnimation;
    [SerializeField] private string m_WaitAnimation;
    [SerializeField]
    private float minWaitTime;
    [SerializeField]
    private float maxWaitTime;
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
        StartCoroutine(Wait());
    
    }
    public override void ExitState()
    {
        base.ExitState();
        StopCoroutine(Wait());
        StopCoroutine(MeteorShower());
    }
    private Coroutine m_MeteorCoroutine;
    private IEnumerator Wait()
    {
        float waitTime = Random.Range(minWaitTime, maxWaitTime);

        m_Animator.Play(m_WaitAnimation);
        yield return new WaitForSeconds(waitTime);
        m_Animator.Play(m_MeteorExplosionAnimation);
        StartCoroutine(MeteorShower());
    }


    private IEnumerator MeteorShower() {
        int meteorNumber = Random.Range(5, 9);
        while (meteorNumber > 0) {
            print("Entro");
            float delayTime = Random.Range(0.2f, 0.6f);
            GameObject meteor = Instantiate(m_meteor);
            meteor.transform.position = m_Boss.SalaPadre.GetPosicionAleatoriaEnSala();
            meteor.GetComponent<Animator>().speed += Random.Range(-0.5f, 0.6f);
            meteorNumber--;
            yield return new WaitForSeconds(delayTime);
            print("Disparos: "+meteorNumber);

        }
        OnShowerFinished?.Invoke(gameObject);
    }


    private void Update()
    {

    }
}
