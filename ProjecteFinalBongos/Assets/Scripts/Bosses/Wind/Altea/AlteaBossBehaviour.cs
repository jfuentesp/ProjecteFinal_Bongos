using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlteaBossBehaviour : BossBehaviour
{
    private Coroutine m_PlayerDetectionCoroutine;
    private Coroutine m_SpawnEggCoroutine;
    private int m_NumberOfAttacksBeforeCharge;
    private SalaBoss m_SalaPadre;
    private new void Awake()
    {
        base.Awake();
        m_PlayerDetectionCoroutine = StartCoroutine(PlayerDetectionCoroutine());
        m_SpawnEggCoroutine = StartCoroutine(SpawnEggsCoroutine());
        m_SalaPadre = GetComponentInParent<SalaBoss>();
    }

   

    void Start()
    {
        m_StateMachine.ChangeState<SMBChaseState>();
        m_NumberOfAttacksBeforeCharge = Random.Range(5, 7);
        GetComponent<SMBChargeState>().OnChargeMissed = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBParriedState>();
        };
        GetComponent<SMBParriedState>().OnRecomposited = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBGroundHitState>();
        };
    }
    private IEnumerator PlayerDetectionCoroutine()
    {
        while (m_IsAlive)
        {

            if (m_PlayerAttackDetectionAreaType == CollisionType.CIRCLE)
            {
                RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position, m_AreaRadius, transform.position, m_AreaRadius, m_LayersToCheck);

                if (hitInfo.collider.CompareTag("Player") && !m_IsBusy)
                {
                    m_IsPlayerDetected = true;
                    
                }
                else
                {
                    m_IsPlayerDetected = false;
                }
            }
            else
            {
                RaycastHit2D hitInfo = Physics2D.BoxCast(transform.position, m_BoxArea, transform.rotation.z, transform.position);
                if (hitInfo.collider.CompareTag("Player") && !m_IsBusy)
                {
                    m_IsPlayerDetected = true;
                    
                }
                else
                {
                    m_IsPlayerDetected = false;
                }
            }
            yield return new WaitForSeconds(m_CheckingPlayerTimelapse);
        }
    }
    private IEnumerator SpawnEggsCoroutine()
    {
        while (m_IsAlive)
        {
            m_SalaPadre.GetPosicionAleatoriaEnSala();
            yield return new WaitForSeconds(2);
        }
    }
}
