using NavMeshPlus.Extensions;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentOverride2d))]
[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(KrakenRangedAttackState))]
[RequireComponent(typeof(KrakenSpinState))]
[RequireComponent(typeof(KrakenParalizingAttack))]
[RequireComponent(typeof(KrakenSetSubMergeState))]
[RequireComponent(typeof(KrakenMergeState))]
[RequireComponent(typeof(SubMergeState))]
[RequireComponent(typeof(HealthController))]
public class KrakenBossBehaviour : BossBehaviour
{
    private Coroutine m_PlayerDetectionCoroutine;
   [SerializeField] private GameObject m_tentacle;
   [SerializeField] private int paralizingTenacleCount = 0;

    private new void Awake()
    {
        base.Awake();
        m_SalaPadre = GetComponentInParent<SalaBoss>();
        GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<KrakenParalizingAttack>().onAttackStopped = (GameObject obj) =>
        {
            StartCoroutine(WaitPTentacleCoroutine());
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<KrakenRangedAttackState>().onAttackStopped = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SubMergeState>();
        };
        GetComponent<KrakenSpinState>().onAttackStopped = (GameObject obj) =>
        {
          
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<KrakenMergeState>().OnMergeFinish = (GameObject obj) =>
        {
          
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBParalized>().OnStopParalized = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBBossStunState>().OnStopStun = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };

        m_StateMachine.ChangeState<SMBIdleState>();
        GetComponent<SMBIdleState>().OnPlayerEnter += EmpezarCorutina;
    }
    private void EmpezarCorutina(GameObject obj)
    {
        m_PlayerDetectionCoroutine = StartCoroutine(PlayerDetectionCoroutine());
        StartCoroutine(SpawnTentacles());
        
    }
    private IEnumerator WaitPTentacleCoroutine()
    {
        while (paralizingTenacleCount > 0) { 
            yield return new WaitForSeconds(1f);
            paralizingTenacleCount--;
        }
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
                   SetAttack();                
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

    private IEnumerator SpawnTentacles() {
        while (m_IsAlive) {
            yield return new WaitForSeconds(1f);
            GameObject tentacle = Instantiate(m_tentacle, transform.parent);
            tentacle.transform.position = m_SalaPadre.GetPosicionAleatoriaEnSala();
        }
    }
    private void SetAttack()
    {
        float rng = Random.value;
        switch (rng)
        {
            case < 0.3f:
                m_StateMachine.ChangeState<KrakenRangedAttackState>();
                break;
            case > 0.8f:
                    m_StateMachine.ChangeState<KrakenSpinState>();
                
                break;

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
        StopAllCoroutines();
        m_IsAlive = false;
        OnBossDeath?.Invoke();
        Destroy(gameObject);
    }
}
