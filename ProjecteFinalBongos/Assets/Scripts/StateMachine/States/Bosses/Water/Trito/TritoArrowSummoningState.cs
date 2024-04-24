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
            Vector2 posicionAleatoria = Random.insideUnitCircle.normalized;
            GameObject arrow = m_Pool.GetElement();
            arrow.transform.position = m_Target.transform.position + 20 * new Vector3(posicionAleatoria.x, posicionAleatoria.y, 0);
            arrow.SetActive(true);
            arrow.GetComponent<TritoArrow>().enabled = true;
            arrow.GetComponent<TritoArrow>().Init(m_Target.position - arrow.transform.position, m_Target);
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
