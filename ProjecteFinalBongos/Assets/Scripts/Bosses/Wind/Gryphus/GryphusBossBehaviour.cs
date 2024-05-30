using NavMeshPlus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentOverride2d))]
[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBParriedState))]
[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(SMBSingleAttackState))]
[RequireComponent(typeof(SMBDoubleAttackState))]
[RequireComponent(typeof(SMBTripleAttackState))]
public class GryphusBossBehaviour : BossBehaviour
{
    [SerializeField] private GameObject m_HealParticles;
    private Coroutine m_HealCoroutine;
    private Coroutine m_PlayerDetectionCoroutine;
    private enum Phase { ONE, TWO }
    private Phase m_CurrentPhase;

    private new void Awake()
    {
        base.Awake();
        GetComponent<SMBParriedState>().OnRecomposited = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBSingleAttackState>().OnStopDetectingPlayer = (GameObject obj) =>
        {
            
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBSingleAttackState>().OnAttackParried = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
        GetComponent<SMBSingleAttackState>().OnAttackStopped = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBDoubleAttackState>().OnAttackParried = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
        GetComponent<SMBDoubleAttackState>().OnAttackStopped = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBTripleAttackState>().OnAttackParried = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
        GetComponent<SMBTripleAttackState>().OnAttackStopped = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        transform.GetChild(0).GetComponent<BossAttackDamage>().OnAttackParried = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
        transform.GetChild(0).GetComponent<BossAttackDamage>().OnAttackHealed = (GameObject obj) =>
        {
            if(m_CurrentPhase == Phase.ONE)
            {
                m_HealthController.Heal(transform.GetChild(0).GetComponent<BossAttackDamage>().Damage * 0.75f);
                ParticulitasCura();
            }
        };
        GetComponent<SMBIdleState>().OnPlayerEnter += EmpezarCorutina;
        m_CurrentPhase = Phase.ONE;
        m_HealthController.onHurt += CheckPhase;
    }

    private void CheckPhase()
    {
        if (m_IsAlive)
        {
            if (m_CurrentPhase == Phase.ONE && m_HealthController.HP <= m_HealthController.HPMAX / 2)
            {
                print("Cambio de fase");
                m_CurrentPhase = Phase.TWO;
                transform.GetChild(0).GetComponent<BossAttackDamage>().SetDamage(transform.GetChild(0).GetComponent<BossAttackDamage>().Damage * 2);
            }
        }
    }

    private void ParticulitasCura()
    {
        if(m_HealCoroutine != null)
        {
            StopCoroutine(m_HealCoroutine);
            m_HealCoroutine = StartCoroutine(CuraParticulas());
        }
        else
        {
            m_HealCoroutine = StartCoroutine(CuraParticulas());
        }
    }

    private IEnumerator CuraParticulas()
    {
        m_HealParticles.SetActive(true);
        yield return new WaitForSeconds(1);
        m_HealParticles.SetActive(false);
    }

    private void Start()
    {
        m_StateMachine.ChangeState<SMBIdleState>();
    }
    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        OnPlayerInSala.Invoke();
    }
    private void EmpezarCorutina(GameObject obj)
    {
        m_PlayerDetectionCoroutine = StartCoroutine(PlayerDetectionCoroutine());
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    private void SetAttack()
    {
        float rng = Random.value;

        if (rng > 0 && rng < 0.5)
        {
            m_StateMachine.ChangeState<SMBSingleAttackState>();
        }
        else if (rng >= 0.5 && rng < 0.8)
        {
            m_StateMachine.ChangeState<SMBDoubleAttackState>();
        }
        else if (rng >= 0.8)
        {
            m_StateMachine.ChangeState<SMBTripleAttackState>();
        }
    }

    private void SetPhase(Phase phaseToSet)
    {
        m_CurrentPhase = phaseToSet;
    }

    private IEnumerator PlayerDetectionCoroutine()
    {
        while (m_IsAlive)
        {

            if (m_PlayerAttackDetectionAreaType == CollisionType.CIRCLE)
            {
                RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position + m_Pivote, m_AreaRadius, transform.position, m_AreaRadius, m_LayersToCheck);
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("Player") && !m_IsBusy)
                {
                    m_IsPlayerDetected = true;
                    SetAttack();
                }
                else
                {
                    m_IsPlayerDetected = false;
                }
            }
            else
            {
                RaycastHit2D hitInfo = Physics2D.BoxCast(transform.position + m_Pivote, m_BoxArea, transform.rotation.z, transform.position);
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("Player") && !m_IsBusy)
                {
                    m_IsPlayerDetected = true;
                    SetAttack();
                }
                else
                {
                    m_IsPlayerDetected = false;
                }
            }
            yield return new WaitForSeconds(m_CheckingPlayerTimelapse);
        }
    }
    private void MatarBoss()
    {
        if (m_GoldPrefab)
        {
            GameObject dinero = Instantiate(m_GoldPrefab, transform.parent);
            dinero.transform.position = transform.position;
        }
        if (m_AbilityPointPrefab)
        {
            GameObject abilityPoint = Instantiate(m_AbilityPointPrefab, transform.parent);
            abilityPoint.transform.position = transform.position;
        }
        Destroy(gameObject);
    }
    protected override void VidaCero()
    {
        base.VidaCero();
        if (m_IsAlive)
        {
            m_IsAlive = false;
            m_BloodController.PlayDeathBlood();
            StopAllCoroutines();
            m_HealParticles.SetActive(false);
            GetComponentInParent<SalaBoss>().OnPlayerIn -= Init;
            m_StateMachine.ChangeState<DeathState>();
            OnBossDeath?.Invoke();
            if (m_BossFinalSala)
                m_BossMuertoEvent.Raise();
        }
    }
}
