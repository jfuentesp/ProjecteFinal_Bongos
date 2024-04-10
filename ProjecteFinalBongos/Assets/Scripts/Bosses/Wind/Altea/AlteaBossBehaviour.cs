using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlteaBossBehaviour : BossBehaviour
{
    private Coroutine m_PlayerDetectionCoroutine;
    private Coroutine m_SpawnEggCoroutine;
    private int m_NumberOfAttacksBeforeCharge;
    private SalaBoss m_SalaPadre;

    [SerializeField]
    private LayerMask m_HuevosLayerMask;

    [Header("Pool of projectiles/splats")]
    [SerializeField]
    private Pool m_Pool;

    private new void Awake()
    {
        base.Awake();
        m_PlayerDetectionCoroutine = StartCoroutine(PlayerDetectionCoroutine());
        m_SalaPadre = GetComponentInParent<SalaBoss>();
    }



    void Start()
    {
        m_StateMachine.ChangeState<SMBChaseState>();
        m_SpawnEggCoroutine = StartCoroutine(SpawnEggsCoroutine());
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
            PonerHuevo();
            yield return new WaitForSeconds(2);
        }
    }

    private void PonerHuevo()
    {
        Vector2 posicionHuevo = m_SalaPadre.GetPosicionAleatoriaEnSala();
        RaycastHit2D hit = Physics2D.CircleCast(posicionHuevo, 1, posicionHuevo, 1, m_HuevosLayerMask);
        if (hit.collider != null)
        {
            PonerHuevo();
        }
        else
        {
            GameObject egg = m_Pool.GetElement();
            egg.transform.position = new Vector2(posicionHuevo.x, posicionHuevo.y);
            egg.SetActive(true);
            egg.GetComponent<EggAltea>().enabled = true;
            egg.GetComponent<EggAltea>().Init();
        }
    }
}
