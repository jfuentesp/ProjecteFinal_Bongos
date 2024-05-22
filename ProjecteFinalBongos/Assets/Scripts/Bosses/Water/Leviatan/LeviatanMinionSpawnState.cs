using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LeviatanMinionsSpawnState : SMState
{
    [SerializeField] private GameObject m_piranha;
    [SerializeField] private GameObject m_anguila;
    [SerializeField] private LayerMask m_LayerCannotInteractInSpawn;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;
    public Action<GameObject> OnSpawnFinished;
    private Transform m_Target;
    private SalaBoss m_SalaPadre;
    private Pool m_Pool;

    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        m_SalaPadre = GetComponentInParent<SalaBoss>();
        m_Pool = LevelManager.Instance._SplashPool;
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

    private IEnumerator Spawn()
    {
        int rng = Random.Range(0, 2);
        if (rng == 0)
        {
            float pirañaNumber = Random.Range(4, 8);
            for (int p = 0; p < pirañaNumber; p++)
            {
                SpawnBicho(0);
            }
        }
        else
        {
            float anguilaNumber = Random.Range(2, 4);
            for (int p = 0; p < anguilaNumber; p++)
            {
                SpawnBicho(1);
            }
        }
        yield return new WaitForSeconds(5f);
        OnSpawnFinished?.Invoke(gameObject);
    }

    private void SpawnBicho(int numBicho)
    {
        Vector2 posicionBicho = m_SalaPadre.GetPosicionAleatoriaEnSala();
        RaycastHit2D hit = Physics2D.CircleCast(posicionBicho, 1, posicionBicho, 1, m_LayerCannotInteractInSpawn);
        if (hit.collider != null)
        {
            SpawnBicho(numBicho);
        }
        else
        {
            if (numBicho == 0)
            {
                //print("Posicion AAAAAAA " + posicionBicho);
                GameObject bicho = m_Pool.GetElement();
                bicho.transform.position = new Vector2(posicionBicho.x, posicionBicho.y);
                bicho.SetActive(true);
                bicho.GetComponent<LeviatanEggs>().enabled = true;
                bicho.GetComponent<LeviatanEggs>().Init(m_Target, transform.parent, 0);
            }
            else
            {
                //print("Posicion AAAAAAA " + posicionBicho);
                GameObject bicho = m_Pool.GetElement();
                bicho.transform.position = new Vector2(posicionBicho.x, posicionBicho.y);
                bicho.SetActive(true);
                bicho.GetComponent<LeviatanEggs>().enabled = true;
                bicho.GetComponent<LeviatanEggs>().Init(m_Target, transform.parent, 1);
            }
        }
    }
}