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
    [SerializeField]
    protected Transform m_Target;
    public Transform Target => m_Target;

    protected FiniteStateMachine m_StateMachine;
    protected Rigidbody2D m_Rigidbody;
    protected Animator m_Animator;

    protected bool m_IsBusy;
    protected bool m_IsAlive;
    protected bool m_IsPlayerDetected;
    public bool IsPlayerDetected => m_IsPlayerDetected;

    protected enum CollisionType { CIRCLE, BOX }

    [Header("Attack detection area settings (CircleCast collider)")]
    [Tooltip("The range within the enemy will trigger main attacks")]
    [SerializeField]
    protected CollisionType m_PlayerAttackDetectionAreaType;
    [Header("If the area is checking as a rectangle collider")]
    [SerializeField]
    protected float m_AreaLength;
    [SerializeField]
    protected float m_AreaWideness;
    protected Vector2 m_BoxArea; 
    [Header("If the area is checking as a circle collider")]
    [SerializeField]
    protected float m_AreaRadius;

    [Header("Player check timelapse")]
    [SerializeField]
    protected float m_CheckingPlayerTimelapse;

    [Header("Layers to detect")]
    [SerializeField]
    protected LayerMask m_LayersToCheck;

    //en el meu estat--
    public delegate void OnPlayerEnter(GameObject obj);
    private OnPlayerEnter onPlayerEnter;

    protected void Awake()
    {
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        if(m_PlayerAttackDetectionAreaType == CollisionType.BOX)
            m_BoxArea = new Vector2(m_AreaWideness, m_AreaLength);
        m_IsBusy = false;
        m_IsAlive = true;
        m_IsPlayerDetected = false;
        /* GetComponent<SMBPatrol>().OnPlayerEnter = (GameObject obj) =>
         {
             m_StateMachine.ChangeState<SMBAttack>();
         };*/
    }

    public void SetBusy(bool status)
    {
        m_IsBusy = status;
    }
}
