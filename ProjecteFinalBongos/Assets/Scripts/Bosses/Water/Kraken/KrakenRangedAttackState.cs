using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenRangedAttackState : SMState
{
    public bool trapped;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;
    [SerializeField] private GameObject m_Tentacle;
    [Header("Pool of projectiles/splats")]
    [SerializeField]
    private Pool m_Pool;

    private Transform m_Target;

    public Action<GameObject> onAttackStopped;

    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        m_Pool = LevelManager.Instance._BulletPool;
        m_Boss.OnPlayerInSala += GetTarget;
        trapped = false;
    }

    private void GetTarget()
    {
        m_Target = m_Boss.Target;
    }

    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true);
        StartCoroutine(Disparar());
        m_Rigidbody.velocity = Vector3.zero;
    }

    private IEnumerator Disparar()
    {
        m_Tentacle.SetActive(true);
        yield return new WaitForSeconds(2f);
        if (trapped)
        {
            Debug.Log("Hola");
            m_Tentacle.SetActive(false);
            GameObject lightning = m_Pool.GetElement();
            lightning.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            lightning.SetActive(true);
            lightning.GetComponent<TintaBullet>().enabled = true;
            lightning.GetComponent<TintaBullet>().Init(m_Target.position - transform.position);
            yield return new WaitForSeconds(1f);
            trapped = false;
            onAttackStopped.Invoke(gameObject);
        }
        else {
            trapped = false;
            onAttackStopped.Invoke(gameObject);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        StopCoroutine(Disparar());
    }
    // Update is called once per frame
    void Update()
    {

    }
}
