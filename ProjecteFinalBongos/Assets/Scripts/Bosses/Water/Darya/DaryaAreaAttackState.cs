using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class DaryaAreaAttackState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;
    private Transform m_Target;

    [Header("Summoning duration")]
    [SerializeField]
    private float m_SummoningDuration;
    public Action<GameObject> onAttackSpawned;
    [SerializeField] private GameObject m_AreaAttackPrefab;

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
        StartCoroutine(SpawnAttack());
        print("AttackState");
    }

    private IEnumerator SpawnAttack()
    {
        GameObject areaAttack = Instantiate(m_AreaAttackPrefab, transform.parent);
        areaAttack.transform. position = m_Target.transform.position;
        areaAttack.transform.localScale = new Vector3(Random.Range(0.5f, 6), Random.Range(0.5f, 6), 1);
        yield return new WaitForSeconds(m_SummoningDuration);
        onAttackSpawned?.Invoke(gameObject);
    }
}
