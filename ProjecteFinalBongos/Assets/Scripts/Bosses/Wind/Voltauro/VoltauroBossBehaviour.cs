using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FiniteStateMachine))]
[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBParriedState))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class VoltauroBossBehaviour : BossBehaviour
{
    private FiniteStateMachine m_StateMachine;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;

    private void Awake()
    {
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
