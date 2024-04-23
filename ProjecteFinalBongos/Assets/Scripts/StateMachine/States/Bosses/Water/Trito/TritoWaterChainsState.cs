using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TritoWaterChainsState : SMState
{
    public Action onChainSummoned;
    [SerializeField]
    private float m_DurationOfSummoning;
    private float m_TimeSummoning;
    private Transform m_Target;
    private BossBehaviour m_Boss;

    private Pool m_Pool;

    protected override void Awake()
    {
        base.Awake();
        m_Boss = GetComponent<BossBehaviour>();
        m_Pool = LevelManager.Instance._BulletPool;
        m_Boss.OnPlayerInSala += GetTarget;
    }

    private void GetTarget()
    {
        m_Target = m_Boss.Target;
    }
    public override void InitState()
    {
        base.InitState();
        m_TimeSummoning = 0;
        m_Boss.SetBusy(true);
        if (m_Target != null)
        {
            GameObject arrow = m_Pool.GetElement();
            arrow.transform.position = transform.position;
            arrow.SetActive(true);
            arrow.GetComponent<TritoChains>().enabled = true;
            arrow.GetComponent<TritoChains>().Init(m_Target.position - transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_TimeSummoning += Time.deltaTime;
        if (m_TimeSummoning >= m_DurationOfSummoning)
        {
            onChainSummoned?.Invoke();
        }
    }
}
