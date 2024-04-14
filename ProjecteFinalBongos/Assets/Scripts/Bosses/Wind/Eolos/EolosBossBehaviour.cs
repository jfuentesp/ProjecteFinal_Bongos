using System;
using System.Collections;
using System.Collections.Generic;
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
        m_CurrentPhase = Phase.TWO;
        GetComponent<SMBChaosState>().empezarContador += EmpezarCorrutina;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_StateMachine.ChangeState<SMBChaosState>();
        GetComponent<SMBBulletsAroundState>().onBulletsSpawned = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaosState>();
        };
        GetComponent<SMBTornadosState>().onTornadoSpawned = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaosState>();
        };
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



    // Update is called once per frame
    void Update()
    {

    }
}
