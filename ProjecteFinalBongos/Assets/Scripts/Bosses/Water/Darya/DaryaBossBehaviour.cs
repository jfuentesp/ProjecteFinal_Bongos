using NavMeshPlus.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentOverride2d))]
[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(HealthController))]
[RequireComponent(typeof(DaryaWavesState))]
[RequireComponent(typeof(DaryaTornadosState))]
[RequireComponent(typeof(DaryaAreaAttackState))]

public class DaryaBossBehaviour : BossBehaviour
{

    private Coroutine m_PlayerDetectionCoroutine;
    private enum Phase { ONE, TWO }
    private Phase m_CurrentPhase;
    [Header("Segunda fase")]
    [SerializeField] private float m_TimeBetwwenAttacksSecondPhase;
    [SerializeField] private int vecesAntesDeQuePeteEscudo;
    [SerializeField] private BubbleProtectionScript m_Bubble;
    [SerializeField] private Material m_materialPared;
    [SerializeField] private float m_PorcentajePerdidaVidaSegundaFase;
    private int cuantoQuedaEscudo;
    private bool atacando;

    private new void Awake()
    {
        base.Awake();
        m_CurrentPhase = Phase.ONE;
        m_SalaPadre = GetComponentInParent<SalaBoss>();
        GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<DaryaWavesState>();
        };
        GetComponent<DaryaTornadosState>().onTornadoSpawned = (GameObject obj) =>
        {
            StartCoroutine(SetAttack());
        };
        GetComponent<DaryaAreaAttackState>().onAttackSpawned = (GameObject obj) =>
        {
            StartCoroutine(SetAttack());
        };
        atacando = false;
    }
    private void Start()
    {
        m_StateMachine.ChangeState<SMBIdleState>();
        cuantoQuedaEscudo = vecesAntesDeQuePeteEscudo;
        m_Bubble.Init(cuantoQuedaEscudo);
        ParedesDarya();
    }

    private void ParedesDarya()
    {
        int hijos = transform.parent.childCount;

        for(int i = 0; i < hijos; i++)
        {
            if (transform.parent.GetChild(i).childCount > 0)
            {
                int hijosDeLosHijos = transform.parent.GetChild(i).childCount;
                for(int j = 0; j < hijosDeLosHijos; j++)
                {
                    if (transform.parent.GetChild(i).GetChild(j).gameObject.CompareTag("MechanicObstacle"))
                    {
                        transform.parent.GetChild(i).GetChild(j).gameObject.AddComponent<BossAttackDamage>();
                        transform.parent.GetChild(i).GetChild(j).gameObject.GetComponent<BossAttackDamage>().SetEstado(EstadosAlterados.Normal);
                        transform.parent.GetChild(i).GetChild(j).gameObject.GetComponent<BossAttackDamage>().SetDamage(20);
                        if(transform.parent.GetChild(i).GetChild(j).gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer sprite))
                            sprite.material = m_materialPared;
                    }
                }
            }
            else
            {
                transform.parent.GetChild(i).gameObject.AddComponent<BossAttackDamage>();
                transform.parent.GetChild(i).gameObject.GetComponent<BossAttackDamage>().SetEstado(EstadosAlterados.Normal);
                transform.parent.GetChild(i).gameObject.GetComponent<BossAttackDamage>().SetDamage(20);
                if (transform.parent.GetChild(i).gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer sprite))
                    sprite.material = m_materialPared;
            }
        }
    }

    protected new void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_CurrentPhase == Phase.ONE)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("AllHitBox") && collision.CompareTag("DaryaWave"))
            {
                cuantoQuedaEscudo--;
                m_Bubble.SetVida(cuantoQuedaEscudo);

                if(cuantoQuedaEscudo == 0)
                {
                    m_CurrentPhase = Phase.TWO;
                    StartCoroutine(SetAttack());
                    StartCoroutine(EmpezarFallecer());
                }
            }
        }
        else if (m_CurrentPhase == Phase.TWO)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHitBox"))
            {
                if (collision.TryGetComponent<AttackDamage>(out AttackDamage damage))
                {
                    recibirDaño(damage.Damage);
                }
            }
        }
    }

    private IEnumerator EmpezarFallecer()
    {
        while (m_IsAlive)
        {
            yield return new WaitForSeconds(1);
            m_HealthController.Damage(m_HealthController.HPMAX / m_PorcentajePerdidaVidaSegundaFase / 100);
        }
    }

    protected override void VidaCero()
    {
        base.VidaCero();
        if (m_CurrentPhase == Phase.TWO)
        {
            StopAllCoroutines();
            GetComponentInParent<SalaBoss>().OnPlayerIn -= Init;
            m_StateMachine.ChangeState<DeathState>();
            m_IsAlive = false;
            OnBossDeath?.Invoke();
            if (m_BossFinalSala)
                m_BossMuertoEvent.Raise();
        }
    }
    private void MatarBoss()
    {
        Destroy(gameObject);
    }

    private IEnumerator SetAttack()
    {
        if (!atacando)
        {
            atacando = true;
            yield return new WaitForSeconds(m_TimeBetwwenAttacksSecondPhase);

            int rand = Random.Range(0, 2);

            if (rand == 0)
            {
                m_StateMachine.ChangeState<DaryaTornadosState>();
            }
            else if (rand == 1)
            {
                m_StateMachine.ChangeState<DaryaAreaAttackState>();
            }
            atacando = false;
        }
    }
}
