using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SMBFlyingState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;

    private Transform m_Target;

    [Header("Flying Animation")]
    [SerializeField]
    private string m_FlyingAnimationName;

    [Header("Harpies Spawn Timer")]
    [Tooltip("Summons a harpy every elapsed seconds we set on this propierty")]
    [SerializeField]
    private float m_SpawnTime;

    [Header("Number of harpies to spawn")]
    [SerializeField]
    private int m_MinimumHarpiesToSpawn;
    [SerializeField]
    private int m_MaximumHarpiesToSpawn;

    private int m_NumberOfHarpiesToSpawn;
    private int m_DeadHarpies;

    [Header("Minion prefab")]
    [SerializeField]
    private GameObject m_Prefab;

    [Header("Flying speed")]
    private float m_FlyingSpeed;

    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Animator = GetComponent<Animator>();
        m_Boss = GetComponent<BossBehaviour>();

        m_Target = m_Boss.Target;
    }

    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true);
        StartCoroutine(SpawnWhileFlyingCoroutine());
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    private void Update()
    {
        
    }

    private IEnumerator SpawnWhileFlyingCoroutine()
    {
        while(m_NumberOfHarpiesToSpawn > 0)
        {
            GameObject harpy = Instantiate(m_Prefab);
            harpy.transform.position = transform.position;
            yield return new WaitForSeconds(m_SpawnTime);
        }
    }

}
