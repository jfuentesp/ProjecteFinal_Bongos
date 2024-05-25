using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TritoWaterChainsState : SMState
{
    public Action onChainSummoned;
    private float m_TimeSummoning;
    private Transform m_Target;
    private BossBehaviour m_Boss;
    [SerializeField] private Vector3 m_PositionToSpawn;
    [SerializeField] private string m_AnimationName;
    [SerializeField] private bool m_TwoDirections;
    private Animator m_Animator;
    private bool derecha;
    private Pool m_Pool;

    protected override void Awake()
    {
        base.Awake();
        m_Boss = GetComponent<BossBehaviour>();
        m_Pool = LevelManager.Instance._BulletPool;
        m_Boss.OnPlayerInSala += GetTarget;
        m_Animator = GetComponent<Animator>();
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
        Dispara();
    }

    private void Dispara()
    {
        if (m_TwoDirections)
        {
            m_Animator.Play(m_AnimationName);
            if (m_Target.position.x - transform.position.x < 0)
            {
                derecha = false;
            }
            else
            {
                derecha = true;
            }
        }
    }

    private void Shoot()
    {
        if (m_Target != null)
        {
            GameObject arrow = m_Pool.GetElement();
            if(derecha)
                arrow.transform.position = transform.position + m_PositionToSpawn;
            else
                arrow.transform.position = transform.position - m_PositionToSpawn;
            arrow.SetActive(true);
            arrow.GetComponent<TritoChains>().enabled = true;
            arrow.GetComponent<TritoChains>().Init(m_Target.position - arrow.transform.position);
        }
    }

    private void EndShootAttack()
    {
        onChainSummoned?.Invoke();
    }
    private void Update()
    {
        if (derecha)
            transform.localEulerAngles = Vector3.zero;
        else
            transform.localEulerAngles = new Vector3(0, 180, 0);

    }

}
