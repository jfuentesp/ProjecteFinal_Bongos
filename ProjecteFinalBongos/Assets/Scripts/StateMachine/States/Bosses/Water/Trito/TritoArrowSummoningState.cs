using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class TritoArrowSummoningState : SMState
{
    public Action onArrowSummoned;
    [SerializeField]
    private float m_DurationOfSummoning;
    private float m_TimeSummoning;
    private Transform m_Target;
    private BossBehaviour m_Boss;

    protected override void Awake()
    {
        base.Awake();
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
        m_TimeSummoning = 0;
        if (m_Target != null)
        {
            Vector2 posicionAleatoria = Random.insideUnitCircle;
            Vector2 posicionPlayer = m_Target.position - transform.position;
            float angulo = Mathf.Atan2(posicionAleatoria.y, posicionAleatoria.x);
            angulo = Mathf.Rad2Deg * angulo - 90;

            LevelManager.Instance.GUIBossManager.SpawnFlecha(angulo);
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_TimeSummoning += Time.deltaTime;
        if(m_TimeSummoning >= m_DurationOfSummoning)
        {
            onArrowSummoned?.Invoke();
        }
    }
}
