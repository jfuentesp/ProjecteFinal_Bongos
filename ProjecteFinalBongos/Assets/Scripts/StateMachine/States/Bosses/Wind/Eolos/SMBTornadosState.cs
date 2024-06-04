using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class SMBTornadosState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;

    float m_CurrentDuration = 0;
    [Header("Summoning duration")]
    [SerializeField]
    private float m_SummoningDuration;
    public Action<GameObject> onTornadoSpawned;

    [SerializeField]
    private GameObject m_TornadoPrefab;

    private Transform m_Target;
    [Header("Circlecast Tornado Variables")]
    [SerializeField]
    private LayerMask m_TornadoLayerMask;
    [SerializeField]
    private float m_RangoCircleCast;
    [SerializeField] private string m_AnimationName;
    private Coroutine m_Corrutina;

    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        m_Boss.OnPlayerInSala += GetTarget;
    }

    private void GetTarget()
    {
        m_Target = m_Boss.Target;
    }
    public override void InitState()
    {
        base.InitState();
        m_CurrentDuration = 0;
        m_Boss.SetBusy(true);
        m_Rigidbody.velocity = Vector3.zero;
        m_Animator.Play(m_AnimationName);
        m_Corrutina = StartCoroutine(SpawnCoroutine());
        
    }
    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();
    }

    private IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(m_SummoningDuration);
        SpawnTornado();
        onTornadoSpawned?.Invoke(gameObject);
    }

    private void SpawnTornado()
    {
        Vector2 posicionTornado = Random.insideUnitCircle.normalized * Random.Range(6f, 11f);
        Vector2 posicionFinal = (Vector2) m_Target.transform.position + posicionTornado;
        GameObject tornado = Instantiate(m_TornadoPrefab);
        tornado.transform.position = posicionFinal;
        tornado.GetComponent<TornadoBehaviour>().Init(m_Target);    
    }
}
