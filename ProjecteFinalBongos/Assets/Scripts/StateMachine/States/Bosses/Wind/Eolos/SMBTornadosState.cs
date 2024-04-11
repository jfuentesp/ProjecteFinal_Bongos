using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBTornadosState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;

    float m_CurrentDuration = 0;
    [Header("Summoning duration")]
    [SerializeField]
    private float m_SummoningDuration;
    public Action<GameObject> onTornadoSpawned;

    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        //Initialize(); //Initializes parameters on a given bullet
    }
    public override void InitState()
    {
        base.InitState();
        m_CurrentDuration = 0;
        m_Boss.SetBusy(true);
        m_Rigidbody.velocity = Vector3.zero;
    }
    // Update is called once per frame
    void Update()
    {
        m_CurrentDuration += Time.deltaTime;
        if (m_CurrentDuration >= m_SummoningDuration)
        {
            onTornadoSpawned.Invoke(gameObject);
        }
    }
}
