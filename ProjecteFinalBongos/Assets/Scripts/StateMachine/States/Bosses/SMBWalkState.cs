using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class SMBWalkState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;

    [Header("Walking speed")]
    [SerializeField]
    private float m_WalkSpeed;

    [Header("Walking time towards a random direction")]
    [SerializeField]
    private float m_WalkTime;

    [Header("Random amount of Time to walk towards a random direction")]
    [Tooltip("If selected, it will randomly select a number between 0 and Walk Time value")]
    [SerializeField]
    private bool m_IsRandomTimeToWalk;

    [Header("Walk animation")]
    [SerializeField]
    private string m_WalkAnimationName;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
    }

    public override void InitState()
    {
        base.InitState();
        m_WalkCoroutine = StartCoroutine(WalkCoroutine());
    }

    public override void ExitState()
    {
        base.ExitState();
        if(m_WalkCoroutine != null)
            StopCoroutine(m_WalkCoroutine);
    }

    private Coroutine m_WalkCoroutine;
    private IEnumerator WalkCoroutine()
    {
        while(true)
        {
            m_Rigidbody.velocity = Vector3.zero;
            int randomX = Random.Range(-1, 2);
            int randomY = Random.Range(-1, 2);
            Vector2 direction = new Vector2(randomX, randomY);

            if (m_IsRandomTimeToWalk) 
            {
                float randomTime = Random.Range(0f, m_WalkTime);
                m_Rigidbody.velocity = direction * m_WalkSpeed;
                yield return new WaitForSeconds(randomTime);
            }
            else
            {
                m_Rigidbody.velocity = direction * m_WalkSpeed;
                yield return new WaitForSeconds(m_WalkTime);
            }
        }
    }
}
