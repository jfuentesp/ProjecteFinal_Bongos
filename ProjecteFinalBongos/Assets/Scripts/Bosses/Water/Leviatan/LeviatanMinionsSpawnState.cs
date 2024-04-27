using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LeviatanMinionsSpawnState : SMState
{
    [SerializeField] private GameObject m_piranha;
    [SerializeField] private GameObject m_anguila;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;
    private LeviatanBossBehaviour a;
    public Action<GameObject> OnSpawnFinished;
    private Transform m_Target;
    private SalaBoss m_SalaPadre;
    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        a= GetComponent<LeviatanBossBehaviour>();
        m_SalaPadre = GetComponentInParent<SalaBoss>();
        m_Boss.OnPlayerInSala += GetTarget;
    }

    private void GetTarget()
    {
        m_Target = m_Boss.Target;
    }

    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true);
        m_SpawnCoroutine = StartCoroutine(Spawn());
    }
    public override void ExitState()
    {
        base.ExitState();
        StopCoroutine(m_SpawnCoroutine);
    }
    private Coroutine m_SpawnCoroutine;

    private IEnumerator Spawn() {
        int rng = Random.Range(0,2);
        if (rng == 0)
        {
            float pirañaNumber = Random.Range(6, 16);
            for (int p = 0; p < pirañaNumber; p++) {
                Vector2 posicionPiranha = m_SalaPadre.GetPosicionAleatoriaEnSala();
                GameObject piranha = Instantiate(m_piranha, transform.parent);
                piranha.transform.position = posicionPiranha;
                piranha.GetComponent<PiranaBehaviour>().Init(m_Target);
            }

        }
        else {
            float anguilaNumber = Random.Range(4, 6);
            for (int p = 0; p < anguilaNumber; p++)
            {
                Vector2 posicionAnguila = m_SalaPadre.GetPosicionAleatoriaEnSala();
                GameObject anguila = Instantiate(m_anguila, transform.parent);
                anguila.transform.position = posicionAnguila;
                anguila.GetComponent<AnguilaBehaviour>().Init(m_Target);
            }
        }
        yield return new WaitForSeconds(0.5f);
        OnSpawnFinished?.Invoke(gameObject);
    }
    
}
