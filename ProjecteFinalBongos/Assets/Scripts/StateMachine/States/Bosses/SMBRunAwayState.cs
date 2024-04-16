using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBRunAwayState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;

    [Header("Chase speed")]
    [SerializeField]
    private float m_RunSpeed;

    private Transform m_Target;

    [Header("Walking duration")]
    [SerializeField]
    private float m_minimumWalkingDuration;
    [SerializeField]
    private float m_maximumWalkingDuration;
    private float m_WalkingDuration;

    private float m_TimeCaminar;
    private float m_TimeChangeDirection;

    [Header("Chase animation")]
    [SerializeField]
    private string m_ChaseAnimationName;

    [Header("Layers para evitar")]
    [SerializeField]
    private LayerMask m_LayerMask;

    public delegate void OnStopRunning(GameObject obj);
    public OnStopRunning onStoppedRunningAway;

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
        m_WalkingDuration = Random.Range(m_minimumWalkingDuration, m_maximumWalkingDuration);
        m_TimeChangeDirection = 0;
        m_TimeCaminar = 0;
        
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    private void Update()
    {
        m_TimeCaminar += Time.deltaTime;
        //To face the target
        if (m_Target != null)
            transform.up = m_Target.position - transform.position;
    }
    private void AVerKittea()
    {
        Vector3 direccion = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direccion, 7, m_LayerMask);
        if (Vector2.Distance(m_Target.position, transform.position + direccion * 7) < 5)
        {
            AVerKittea();
        }
        else
        {
            if (hit.collider != null)
            {
                AVerKittea();
            }
            else
            {
                m_Rigidbody.velocity = direccion.normalized * m_RunSpeed;
            }
        }
    }
    private void FixedUpdate()
    {
        m_TimeChangeDirection += Time.fixedDeltaTime;
        if (m_TimeChangeDirection > 1)
        {
            AVerKittea();
            m_TimeChangeDirection = 0;
        }
        if(m_TimeCaminar > m_WalkingDuration)
        {
            onStoppedRunningAway.Invoke(gameObject);
        }
    }
}
