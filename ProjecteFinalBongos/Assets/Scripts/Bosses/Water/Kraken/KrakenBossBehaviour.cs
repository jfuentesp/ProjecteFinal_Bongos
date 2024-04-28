using NavMeshPlus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentOverride2d))]
[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(SMBRunAwayState))]
[RequireComponent(typeof(HealthController))]
public class KrakenBossBehaviour : BossBehaviour
{
    [SerializeField] private int m_RangoHuirPerseguir;
    private Coroutine m_PlayerDetectionCoroutine;
    [SerializeField] private GameObject m_tentacle;
    private new void Awake()
    {
        base.Awake();
        m_SalaPadre = GetComponentInParent<SalaBoss>();
        GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
        {
            PerseguirHuir();
        };
        GetComponent<SMBRunAwayState>().onStoppedRunningAway = (GameObject obj) =>
        {
            PerseguirHuir();
        };

        m_StateMachine.ChangeState<SMBIdleState>();
        GetComponent<SMBIdleState>().OnPlayerEnter += EmpezarCorutina;
    }
    private void EmpezarCorutina(GameObject obj)
    {
        m_PlayerDetectionCoroutine = StartCoroutine(PlayerDetectionCoroutine());
        StartCoroutine(SpawnTentacles());
        
    }

    private IEnumerator PlayerDetectionCoroutine()
    {
        while (m_IsAlive)
        {

            if (m_PlayerAttackDetectionAreaType == CollisionType.CIRCLE)
            {
                RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position, m_AreaRadius, transform.position, m_AreaRadius, m_LayersToCheck);
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("Player") && !m_IsBusy)
                {
                    m_IsPlayerDetected = true;
                    PerseguirHuir();
                }
                else
                {
                    m_IsPlayerDetected = false;
                }
            }
            else
            {
                RaycastHit2D hitInfo = Physics2D.BoxCast(transform.position, m_BoxArea, transform.rotation.z, transform.position);
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("Player") && !m_IsBusy)
                {
                    m_IsPlayerDetected = true;
                    PerseguirHuir();
                }
                else
                {
                    m_IsPlayerDetected = false;
                }
            }
            yield return new WaitForSeconds(m_CheckingPlayerTimelapse);
        }
    }

    private IEnumerator SpawnTentacles() {
        while (m_IsAlive) {
            yield return new WaitForSeconds(4f);
            GameObject tentacle = Instantiate(m_tentacle, transform.parent);
            tentacle.transform.position = m_SalaPadre.GetPosicionAleatoriaEnSala();
        }
    }
    private void PerseguirHuir()
    {

        if (Vector2.Distance(transform.position, m_Target.position) > m_RangoHuirPerseguir)
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        }
        else
        { 
            m_StateMachine.ChangeState<SMBRunAwayState>();
        }

    }
    
    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        OnPlayerInSala?.Invoke();
    }
    protected override void VidaCero()
    {
        base.VidaCero();
        m_IsAlive = false;
        OnBossDeath?.Invoke();
        Destroy(gameObject);
    }
}
