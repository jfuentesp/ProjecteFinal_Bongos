using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBBulletsAroundState))]
[RequireComponent(typeof(SMBTornadosState))]
[RequireComponent(typeof(SMBChaosState))]
public class MiniEolosBehaviour : BossBehaviour
{
    [SerializeField]
    private float m_TiempoMinimo;
    [SerializeField]
    private float m_TiempoMaximo;
    private float m_tiempoEntreAtaque;

    private new void Awake()
    {
        base.Awake();
        m_SalaPadre = GetComponentInParent<SalaBoss>();
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
        m_StateMachine.ChangeState<SMBIdleState>();
    }

    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        OnPlayerInSala.Invoke();
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
    protected override void VidaCero()
    {
        base.VidaCero();
            m_IsAlive = false;
            OnBossDeath?.Invoke();
            m_BossMuertoEvent.Raise();
            Destroy(gameObject);
  
    }

}
