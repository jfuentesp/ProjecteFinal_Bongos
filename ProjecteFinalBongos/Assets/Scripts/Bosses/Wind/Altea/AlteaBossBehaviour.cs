using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBRunAwayState))]
[RequireComponent(typeof(SMBRangedAttack))]
public class AlteaBossBehaviour : BossBehaviour
{
    private Coroutine m_SpawnEggCoroutine;
    private SalaBoss m_SalaPadre;

    [SerializeField]
    private LayerMask m_HuevosLayerMask;

    [Header("Pool of projectiles/splats")]
    [SerializeField]
    private Pool m_Pool;

    private new void Awake()
    {
        base.Awake();
        m_SalaPadre = GetComponentInParent<SalaBoss>();
    }

    void Start()
    {
        GetComponent<SMBRunAwayState>().onStoppedRunningAway = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBRangedAttack>();
        };
        GetComponent<SMBRangedAttack>().onAttackStopped = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBRunAwayState>();
        };
        m_StateMachine.ChangeState<SMBRunAwayState>();
        m_SpawnEggCoroutine = StartCoroutine(SpawnEggsCoroutine());
    }
   
    private IEnumerator SpawnEggsCoroutine()
    {
        while (m_IsAlive)
        {
            yield return new WaitForSeconds(7);
            PonerHuevo();
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
            egg.GetComponent<EggAltea>().Init(m_Target);
        }
    }
}
