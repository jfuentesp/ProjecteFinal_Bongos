using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeviatanCrashWaveState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;
    [SerializeField] private GameObject m_Wave;
    public Action<GameObject> OnSpawnedWave;

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
        m_CrashWaveInvokeCoroutine = StartCoroutine(CrashWaveInvoke());
    }
    public override void ExitState()
    {
        base.ExitState();
        StopCoroutine(m_CrashWaveInvokeCoroutine);
    }
    private Coroutine m_CrashWaveInvokeCoroutine;
    private IEnumerator CrashWaveInvoke() {
        GameObject wave = Instantiate(m_Wave);
        wave.transform.position = transform.position;
        yield return new WaitForSeconds(1f);
        OnSpawnedWave?.Invoke(gameObject);
    }
}
