using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AgullaChargeState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;
    private NavMeshAgent m_NavMeshAgent;

    [Header("AnimationName")]
    [SerializeField] private string m_AnimationName;

    [Header("Charge speed")]
    [SerializeField]
    private float m_ChargeSpeed;

    [Header("Time Before Charge")]
    [SerializeField] private float m_TimeBeforeCharge;

    [Header("GameObject Shader")]
    [SerializeField] private GameObject m_Shader;
    [SerializeField] private GameObject m_Objetivo;

    [Header("Animation Two Directions")]
    [SerializeField] protected bool m_TwoDirections;

    [Header("Colores del Shader")]
    [SerializeField] private Color[] m_ColoresList;

    private bool derecha;

    private bool m_IsAiming;
    private bool m_IsCharging;

    private Vector2 m_PositionToCharge;

    private Transform m_Target;

    public Action<GameObject> OnChargeMissed;
    public Action<GameObject> OnChargePlayer;


    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_Boss.OnPlayerInSala += GetTarget;
        m_Boss.OnBossDeath += HideTelegraph;
    }
    private void HideTelegraph()
    {
        m_Shader.SetActive(false);
        m_Objetivo.SetActive(false);
    }

    private void GetTarget()
    {
        m_Target = m_Boss.Target;
    }

    public override void InitState()
    {
        base.InitState();
        m_NavMeshAgent.isStopped = false;
        m_IsAiming = false;
        m_IsCharging = false;
        m_Boss.SetBusy(true);
        StartCoroutine(ChargeCoroutine());
        m_NavMeshAgent.speed = m_ChargeSpeed;
        m_NavMeshAgent.ResetPath();
    }

    public override void ExitState()
    {
        base.ExitState();
        m_NavMeshAgent.isStopped = true;
        StopAllCoroutines();
    }

    Vector3 destino;
    private IEnumerator ChargeCoroutine()
    {
        m_IsAiming = true;
        m_Animator.Play(m_AnimationName);
        m_Objetivo.SetActive(true);
        m_Shader.SetActive(true);
        m_Shader.GetComponent<SpriteRenderer>().material.SetColor("_Color", m_ColoresList[Random.Range(0, m_ColoresList.Length)]);
        yield return new WaitForSeconds(m_TimeBeforeCharge);
        m_Shader.SetActive(false);
        m_Objetivo.SetActive(false);
        m_IsAiming = false;
        m_IsCharging = true;
        m_NavMeshAgent.isStopped = false;
        destino = m_Target.position;
        m_NavMeshAgent.SetDestination(destino);
    }

    private void Update()
    {
        if (m_TwoDirections)
        {
            if (m_IsAiming)
            {
                m_Rigidbody.velocity = Vector3.zero;
               
                float distancia = Vector2.Distance(m_Shader.transform.position, m_Target.position);

                m_Shader.GetComponent<SpriteRenderer>().size = new Vector2(m_Shader.GetComponent<SpriteRenderer>().size.x, distancia / transform.localScale.y);
                
                if (m_Target.position.x - transform.position.x < 0)
                {
                    derecha = false;
                    Vector2 direction = m_Target.position - m_Shader.transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    m_Shader.transform.rotation = Quaternion.Euler(new Vector3(0, 180, angle - 90));
                    Vector2 posicion = direction.normalized * distancia / transform.localScale.y;
                    m_Objetivo.transform.localPosition = new Vector2(-posicion.x, posicion.y);
                }
                else
                {
                    derecha = true;
                    Vector2 direction = m_Target.position - m_Shader.transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    m_Shader.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
                    m_Objetivo.transform.localPosition = direction.normalized * distancia / transform.localScale.y;
                }
            }
            if (m_IsCharging)
            {
                if (Vector2.Distance(destino, transform.position) < 0.2f)
                {
                    print("falle");
                    OnChargeMissed?.Invoke(gameObject);
                }
            }
            if (derecha)
                transform.localEulerAngles = Vector3.zero;
            else
                transform.localEulerAngles = new Vector3(0, 180, 0);
        }
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enabled)
        {
            if (m_IsCharging)
            {
                print("Choque");
                m_IsCharging = false;
                m_NavMeshAgent.velocity = Vector3.zero;
                if (collision.gameObject.CompareTag("MechanicObstacle"))
                {
                    OnChargeMissed?.Invoke(gameObject);
                }
                if (collision.gameObject.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                    OnChargePlayer?.Invoke(gameObject);
                }
            }
        }
    }
}
