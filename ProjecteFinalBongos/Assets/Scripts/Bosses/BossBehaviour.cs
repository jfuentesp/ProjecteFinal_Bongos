using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

[RequireComponent(typeof(FiniteStateMachine))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class BossBehaviour : MonoBehaviour
{
    [Header("Boss parameters")]
    [SerializeField]
    protected string m_BossName;
    [SerializeField]
    protected string m_Description;
    [SerializeField]
    protected float m_MaxHP;
    [SerializeField]
    protected Sprite m_Sprite;

    protected FiniteStateMachine m_StateMachine;
    protected Rigidbody2D m_Rigidbody;
    protected Animator m_Animator;

    //en el meu estat--
    public delegate void OnPlayerEnter(GameObject obj);
    private OnPlayerEnter onPlayerEnter;

    protected void Awake()
    {
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        /* GetComponent<SMBPatrol>().OnPlayerEnter = (GameObject obj) =>
         {
             m_StateMachine.ChangeState<SMBAttack>();
         };*/
    }
}
