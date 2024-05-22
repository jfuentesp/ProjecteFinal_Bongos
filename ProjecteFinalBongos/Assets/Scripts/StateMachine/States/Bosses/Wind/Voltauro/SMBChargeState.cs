using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class SMBChargeState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;
    private NavMeshAgent m_NavMeshAgent;
    [Header("Layers para el telegraph")]
    [SerializeField] private LayerMask m_LayersTelegraph;

    [Header("Time Before Charge")]
    [SerializeField] private float m_TimeBeforeCharge;

    [Header("GameObject Shader")]
    [SerializeField] private GameObject m_Shader;

    [Header("Charge speed")]
    [SerializeField]
    private float m_ChargeSpeed;

    [Header("Charge force to apply on impact")]
    [SerializeField]
    private float m_ChargeForce;

    [Header("Animation Name")]
    [SerializeField] private string m_StartChargeAnimationName;
    [SerializeField] private string m_ChargeAnimationName;
    [SerializeField] private string m_EndChargeAnimationName;

    [Header("Animation Two Directions")]
    [SerializeField] protected bool m_TwoDirections;
    [Header("Colores del Shader")]
    [SerializeField] private Color[] m_ColoresList;

    private bool m_IsAiming;
    private bool m_IsCharging;

    private Transform m_Target;

    public Action<GameObject> OnChargeMissed;
    public Action<GameObject> OnChargeParried;
    public Action<GameObject> OnChargePlayer;

    private bool derecha;


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
    }

    private void GetTarget()
    {
        m_Target = m_Boss.Target;
    }

    public override void InitState()
    {
        base.InitState();
        m_IsAiming = false;
        m_IsCharging = false;
        m_Boss.SetBusy(true);
        StartCoroutine(ChargeCoroutine());
        m_NavMeshAgent.ResetPath();
        m_NavMeshAgent.acceleration = m_ChargeSpeed;
        m_NavMeshAgent.speed = m_ChargeSpeed;
    }

    public override void ExitState()
    {
        base.ExitState();
        m_NavMeshAgent.isStopped = true;
        StopAllCoroutines();
    }

    private IEnumerator ChargeCoroutine()
    {
        m_IsAiming = true;
        m_Animator.Play(m_StartChargeAnimationName);
        m_Shader.SetActive(true);
        m_Shader.GetComponent<SpriteRenderer>().material.SetColor("_Color", m_ColoresList[Random.Range(0, m_ColoresList.Length)]);
        yield return new WaitForSeconds(m_TimeBeforeCharge);
        m_Shader.SetActive(false);
        m_IsAiming = false;
        m_IsCharging = true;
        m_Animator.Play(m_ChargeAnimationName);
    }

    Vector3 m_Direction;
    private void Update()
    {
        if (m_TwoDirections)
        {
            if (m_IsAiming)
            {
                if(m_Target != null)
                {
                    RaycastHit2D hit;
                    hit = Physics2D.Raycast(m_Shader.transform.position, m_Target.position - m_Shader.transform.position, Mathf.Infinity, m_LayersTelegraph);

                    float distancia = Vector2.Distance(m_Shader.transform.position, hit.point);

                    m_Shader.GetComponent<SpriteRenderer>().size = new Vector2(m_Shader.GetComponent<SpriteRenderer>().size.x, distancia / transform.localScale.y);

                    //m_Shader.transform.localScale = new Vector3(m_Shader.transform.localScale.x, distancia, m_Shader.transform.localScale.z);

                    if (m_Target.position.x - transform.position.x < 0)
                    {
                        derecha = false;
                        Vector2 direction = m_Target.position - m_Shader.transform.position;
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        m_Shader.transform.rotation = Quaternion.Euler(new Vector3(0, 180, angle - 90));
                    }
                    else
                    {
                        derecha = true;
                        Vector2 direction = m_Target.position - m_Shader.transform.position;
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        m_Shader.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
                    }
                    m_Direction = (m_Target.transform.position - transform.position).normalized;
                }
            }
            if (derecha)
                transform.localEulerAngles = Vector3.zero;
            else
                transform.localEulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void FixedUpdate()
    {
        if (m_IsCharging)
        {
            m_NavMeshAgent.isStopped = false;
            m_NavMeshAgent.velocity = m_Direction * m_ChargeSpeed;
        }
        //m_Rigidbody.velocity = m_Direction * m_ChargeSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enabled)
        {
            if (m_IsCharging)
            {
                m_IsCharging = false;
                m_NavMeshAgent.velocity = Vector3.zero;
                if (collision.gameObject.CompareTag("MechanicObstacle"))
                {
                    OnChargeMissed?.Invoke(gameObject);
                }
                if (collision.gameObject.layer == LayerMask.NameToLayer("BossHurtBox"))
                {
                    OnChargeMissed?.Invoke(gameObject);
                }
                if (collision.gameObject.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                    if (collision.gameObject.TryGetComponent<SMBPlayerParryState>(out SMBPlayerParryState parry))
                    {
                        if (parry.parry)
                        {
                            OnChargeParried?.Invoke(gameObject);
                        }
                        else
                        {
                            OnChargePlayer?.Invoke(gameObject);
                            if (collision.gameObject.GetComponent<PJSMB>())
                                collision.gameObject.GetComponent<PJSMB>().GetDamage(GetComponent<BossAttackDamage>().Damage, GetComponent<BossAttackDamage>().EstadoAlterado, GetComponent<BossAttackDamage>().StateTime);
                            Rigidbody2D target;
                            collision.gameObject.TryGetComponent<Rigidbody2D>(out target);
                            if (target != null)
                                target.AddForce((transform.position + collision.transform.position).normalized * m_ChargeSpeed, ForceMode2D.Impulse);
                        }
                    }
                }
            }
        }
    }
}
