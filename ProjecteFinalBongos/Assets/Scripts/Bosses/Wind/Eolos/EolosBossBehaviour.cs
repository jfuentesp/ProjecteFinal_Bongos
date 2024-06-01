using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBChaosState))]
[RequireComponent(typeof(SMBBulletsAroundState))]
[RequireComponent(typeof(SMBTornadosState))]
[RequireComponent(typeof(SMBLightningSummonState))]
public class EolosBossBehaviour : BossBehaviour
{
    [SerializeField]
    private float m_TiempoMinimo;
    [SerializeField]
    private float m_TiempoMaximo;
    private float m_tiempoEntreAtaque;

    private enum Phase { ONE, TWO, THREE }
    private Phase m_CurrentPhase;

    private new void Awake()
    {
        base.Awake();
        m_SalaPadre = GetComponentInParent<SalaBoss>();
        m_CurrentPhase = Phase.ONE;
        GetComponent<SMBChaosState>().empezarContador += EmpezarCorrutina;
        GetComponent<SMBBulletsAroundState>().onBulletsSpawned = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaosState>();
        };
        GetComponent<SMBTornadosState>().onTornadoSpawned = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaosState>();
        };
        GetComponent<SMBLightningSummonState>().OnEndSummoning = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaosState>();
        };
        GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaosState>();
        };
        GetComponent<SMBBossStunState>().OnStopStun = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaosState>();
        };
        GetComponent<SMBParalized>().OnStopParalized = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaosState>();
        };
    }

    private void Start()
    {
        m_StateMachine.ChangeState<SMBIdleState>();
    }

    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        OnPlayerInSala?.Invoke();
    }
    private void EmpezarCorrutina()
    {
        StartCoroutine(EsperarAtaque());
    }

    private IEnumerator EsperarAtaque()
    {
        m_tiempoEntreAtaque = Random.Range(m_TiempoMinimo, m_TiempoMaximo);
        yield return new WaitForSeconds(m_tiempoEntreAtaque);
        int numerin = Random.Range(0, 2);
        if(m_CurrentPhase == Phase.ONE)
        {
            switch (numerin)
            {
                case 0:
                    m_StateMachine.ChangeState<SMBBulletsAroundState>();
                    break;
                case 1:
                    m_StateMachine.ChangeState<SMBTornadosState>();
                    break;
            }
        }
        else if(m_CurrentPhase == Phase.TWO)
        {
            switch (numerin)
            {
                case 0:
                    m_StateMachine.ChangeState<SMBLightningSummonState>();
                    break;
                case 1:
                    m_StateMachine.ChangeState<SMBTornadosState>();
                    break;
            }
        }
        else
        {

        }
        
    }

    protected override void VidaCero()
    {
        base.VidaCero();
        print("Cambio de fase");
        /*if (m_CurrentPhase == Phase.THREE)
        {
            m_IsAlive = false;
            StopAllCoroutines();
            OnBossDeath?.Invoke();
            if (m_BossFinalSala)
                m_BossMuertoEvent.Raise();
            Destroy(gameObject);
        }*/
        if (m_CurrentPhase == Phase.TWO)
        {
            /*GetComponent<BoxCollider2D>().enabled = false;
            m_HealthController.Revive();
            m_CurrentPhase = Phase.THREE;
            StartCoroutine(DescontarVida());
            return;*/
            m_BloodController.PlayDeathBlood();
            m_IsAlive = false;
            StopAllCoroutines();
            OnBossDeath?.Invoke();
            if (m_BossFinalSala)
                m_BossMuertoEvent.Raise();
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
        else if (m_CurrentPhase == Phase.ONE)
        {
            m_HealthController.Revive();
            m_CurrentPhase = Phase.TWO;
            return;
        }
    }

    private IEnumerator DescontarVida()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            m_HealthController.Damage(m_HealthController.HPMAX / 50);
        }
    }
}
