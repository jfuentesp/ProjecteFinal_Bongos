using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;
using System;

public class DaryaWavesState : SMState
{
    [SerializeField] private GameObject m_ParedObstaclePrefab;
    [SerializeField] private GameObject m_OlaPrefab;
    [SerializeField] private float m_TimeSpawn;
    [SerializeField] private float m_DistanceFromPlayer;
    [SerializeField] private string m_AnimationName;
    private Coroutine m_WavingCoroutine;

    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;
    private Transform m_Target;

    private new void Awake()
    {
        base.Awake();
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
        m_WavingCoroutine = StartCoroutine(Waving());
    }

    private void Update()
    {
        
    }

    private IEnumerator Waving()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_TimeSpawn);
            m_Animator.Play(m_AnimationName);
           
        }
    }
    public void SpawnWave()
    {
        Vector2 direccion = (m_Target.position - transform.position).normalized;

        Vector2 posicionPared = new Vector2(m_Target.position.x, m_Target.position.y) + direccion * m_DistanceFromPlayer;
        GameObject obstaculo = Instantiate(m_ParedObstaclePrefab, transform.parent);
        obstaculo.transform.position = posicionPared;
        Vector2 posicionPlayer = transform.position - obstaculo.transform.position;
        float angulo = Mathf.Atan2(posicionPlayer.y, posicionPlayer.x);
        angulo = Mathf.Rad2Deg * angulo - 90;
        obstaculo.transform.localEulerAngles = new Vector3(0, 0, angulo);

        GameObject OlaPrefab = Instantiate(m_OlaPrefab, transform.parent);
        OlaPrefab.GetComponent<DaryaWaveScript>().Init(m_Target, transform);
    }

    public override void ExitState()
    {
        base.ExitState();
        StopCoroutine(m_WavingCoroutine);
    }

}
