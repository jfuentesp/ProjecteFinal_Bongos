using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SMBBulletsAroundState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;

    [Header("Summoning duration")]
    [SerializeField]
    private float m_SummoningDuration;

    [Header("Pool of projectiles/splats")]
    [SerializeField]
    private Pool m_Pool;

    private Coroutine SpawnBulletsCoroutine;

    public Action<GameObject> onBulletsSpawned;

    float m_CurrentDuration = 0;

    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        m_Pool = LevelManager.Instance._BulletPool;
        //Initialize(); //Initializes parameters on a given bullet
    }
    public override void InitState()
    {
        base.InitState();
        m_CurrentDuration = 0;
        m_Boss.SetBusy(true);
        m_Rigidbody.velocity = Vector3.zero;
        SpawnBulletsCoroutine = StartCoroutine(SpawnBullets());
    }

    private IEnumerator SpawnBullets()
    {
        yield return new WaitForSeconds(m_SummoningDuration / 2);
        for(float i = -1; i < 2; i += 1f)
        {
            for (float j = -1; j < 2; j += 1f)
            {
                /*GameObject lightning = m_Pool.GetElement();
                lightning.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                lightning.SetActive(true);
                lightning.GetComponent<SinusBullet>().enabled = true;
                lightning.GetComponent<SinusBullet>().Init(new Vector2(i, j));*/
                GameObject lightning = m_Pool.GetElement();
                lightning.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                lightning.SetActive(true);
                lightning.GetComponent<SinusBullet>().enabled = true;
                lightning.GetComponent<SinusBullet>().Init(new Vector2(i, j).normalized);
            }
        }
    }

    void Update()
    {
        m_CurrentDuration += Time.deltaTime;
        if (m_CurrentDuration >= m_SummoningDuration)
        {
            onBulletsSpawned.Invoke(gameObject);
        }
    }
    public override void ExitState()
    {
        base.ExitState();
    }
}
