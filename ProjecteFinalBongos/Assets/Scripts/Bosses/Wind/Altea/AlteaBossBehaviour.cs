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

    [Header("Time between Eggs")]
    [SerializeField] private float m_TimeBetweenEggs;

    [Header("Maximum Snakes")]
    [SerializeField] private int m_MaximumSnakes;
    [SerializeField] private int m_CurrentSnakes;

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
        GetComponent<SMBBossStunState>().OnStopStun = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBRunAwayState>();
        };
        GetComponent<SMBParalized>().OnStopParalized = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBRunAwayState>();
        };
        m_Pool = LevelManager.Instance._SplashPool;
        GetComponent<SMBIdleState>().OnPlayerEnter += EmpezarCorutina;
        m_StateMachine.ChangeState<SMBIdleState>();
    }
    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        m_CurrentSnakes = 0;
        OnPlayerInSala?.Invoke();
    }
    private void EmpezarCorutina(GameObject obj)
    {
        m_SpawnEggCoroutine??= StartCoroutine(SpawnEggsCoroutine());
    }

    private IEnumerator SpawnEggsCoroutine()
    {
        while (m_IsAlive)
        {
            yield return new WaitForSeconds(m_TimeBetweenEggs);
            PonerHuevo();
        }
    }

    private void PonerHuevo()
    {
        if(m_CurrentSnakes < m_MaximumSnakes)
        {
            Vector2 posicionHuevo = m_SalaPadre.GetPosicionAleatoriaEnSala();
            RaycastHit2D hit = Physics2D.CircleCast(posicionHuevo, 1, posicionHuevo, 1, m_HuevosLayerMask);
            if (hit.collider != null || Vector2.Distance(transform.position, posicionHuevo) > 10)
            {
                PonerHuevo();
            }
            else
            {
                GameObject egg = m_Pool.GetElement();
                egg.transform.position = new Vector2(posicionHuevo.x, posicionHuevo.y);
                egg.SetActive(true);
                egg.GetComponent<EggAltea>().enabled = true;
                egg.GetComponent<EggAltea>().Init(m_Target, transform.parent, transform);
                m_CurrentSnakes++;
            }
        }
    }

    public void SnakeMuerta()
    {
        m_CurrentSnakes--;
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
        m_BloodController.PlayDeathBlood();
        StopAllCoroutines();
        GetComponentInParent<SalaBoss>().OnPlayerIn -= Init;
        m_StateMachine.ChangeState<DeathState>();
        m_IsAlive = false;
        OnBossDeath?.Invoke();
        if (m_BossFinalSala)
            m_BossMuertoEvent.Raise();
    }
    protected override void Update()
    {
        base.Update();
    }
}
