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
    private int m_HarpiesToDie;

    [Header("Minion prefab")]
    [SerializeField]
    private GameObject m_Prefab;

    [Header("Flying speed")]
    [SerializeField]
    private float m_FlyingSpeed;

    [Header("Change direction time")]
    [SerializeField]
    private float m_DirectionTime;

    [Header("LayerMask for RayCasting on random walk")]
    [SerializeField]
    private LayerMask m_LayerMask;

    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Animator = GetComponent<Animator>();
        m_Boss = GetComponent<BossBehaviour>();
        m_Boss.OnPlayerInSala += SetTarget;
    }

    private void SetTarget()
    {
        m_Target = m_Boss.Target;
    }

    public override void InitState()
    {
        base.InitState();
        m_Animator.Play(m_FlyingAnimationName);
        m_Boss.SetBusy(true);
        m_NumberOfHarpiesToSpawn = Random.Range(m_MinimumHarpiesToSpawn, m_MaximumHarpiesToSpawn);
        m_HarpiesToDie = m_NumberOfHarpiesToSpawn; //Puede que esto sea -1...
        StartCoroutine(SpawnWhileFlyingCoroutine());
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public void OnHarpyDeath()
    {
        m_HarpiesToDie--;
        //Debug.Log("Harpias muertas: " + m_HarpiesToDie);
        if (m_HarpiesToDie <= 0)
        {
            m_StateMachine.ChangeState<SMBLandingState>();
        }
    }

    private float m_TimeChangeDirection = 0;
    private void FixedUpdate()
    {
        m_TimeChangeDirection += Time.fixedDeltaTime;
        if (m_TimeChangeDirection > m_DirectionTime)
        {
            RandomWalk();
            m_TimeChangeDirection = 0;
        }
    }

    private void RandomWalk()
    {
        Vector3 direccion = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direccion, 7, m_LayerMask);
        if (hit.collider != null)
        {
            RandomWalk();
        }
        else
        {
            m_Rigidbody.velocity = direccion.normalized * m_FlyingSpeed;
        }
    }


    private IEnumerator SpawnWhileFlyingCoroutine()
    {
        while(m_NumberOfHarpiesToSpawn > 0)
        {
            m_NumberOfHarpiesToSpawn--;
            GameObject obj = Instantiate(m_Prefab, transform.parent);
            HarpyBehaviour harpy = obj.GetComponent<HarpyBehaviour>();
            Vector3 spawnPos = new Vector3(Random.Range(transform.position.x - 5, transform.position.x + 5), transform.position.y + 7, 0);
            obj.transform.position = spawnPos;
            harpy.Init(m_Target);
            yield return new WaitForSeconds(m_SpawnTime);
        }
    }

}
