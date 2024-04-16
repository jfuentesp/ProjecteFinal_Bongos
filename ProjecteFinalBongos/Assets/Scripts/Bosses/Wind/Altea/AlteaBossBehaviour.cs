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

    [SerializeField]
    private LayerMask m_HuevosLayerMask;

    [Header("Pool of projectiles/splats")]
    [SerializeField]
    private Pool m_Pool;

    private new void Awake()
    {
        base.Awake();
        m_SalaPadre = GetComponentInParent<SalaBoss>();
        GetComponent<SMBRunAwayState>().onStoppedRunningAway = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBRangedAttack>();
        };
        GetComponent<SMBRangedAttack>().onAttackStopped = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBRunAwayState>();
        };
        GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBRunAwayState>();
        };
        m_Pool = GameObject.Find("PoolSplash").GetComponent<Pool>();
        GetComponent<SMBIdleState>().OnPlayerEnter += EmpezarCorutina;
        m_StateMachine.ChangeState<SMBIdleState>();
    }
    private void Start()
    {
    }
    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        OnPlayerInSala.Invoke();
    }
    private void EmpezarCorutina(GameObject obj)
    {
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
    protected override void VidaCero()
    {
        base.VidaCero();
        m_IsAlive = false;
        Destroy(gameObject);
    }
    protected override void Update()
    {
        base.Update();
    }
}
