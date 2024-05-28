using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniDaryaBossBehaviour : BossBehaviour
{
    private Coroutine m_PlayerDetectionCoroutine;
    private enum Phase { ONE, TWO }
    private Phase m_CurrentPhase;
    [Header("Segunda fase")]
    [SerializeField] private float m_TimeBetwwenAttacksSecondPhase;
    [SerializeField] private int vecesAntesDeQuePeteEscudo;
    [SerializeField] private BubbleProtectionScript m_Bubble;
    [SerializeField] private Material m_materialPared;
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

        for (int i = 0; i < hijos; i++)
        {
            if (transform.parent.GetChild(i).childCount > 0)
            {
                int hijosDeLosHijos = transform.parent.GetChild(i).childCount;
                for (int j = 0; j < hijosDeLosHijos; j++)
                {
                    if (transform.parent.GetChild(i).GetChild(j).gameObject.CompareTag("MechanicObstacle"))
                    {
                        transform.parent.GetChild(i).GetChild(j).gameObject.AddComponent<BossAttackDamage>();
                        transform.parent.GetChild(i).GetChild(j).gameObject.GetComponent<BossAttackDamage>().SetEstado(EstadosAlterados.Normal);
                        transform.parent.GetChild(i).GetChild(j).gameObject.GetComponent<BossAttackDamage>().SetDamage(20);
                        if (transform.parent.GetChild(i).GetChild(j).gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer sprite))
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

                if (cuantoQuedaEscudo == 0)
                {
                    m_CurrentPhase = Phase.TWO;
                    StartCoroutine(SetAttack());
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

            m_StateMachine.ChangeState<DaryaAreaAttackState>();
            atacando = false;
        }
    }
}
