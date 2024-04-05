using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBLightningSummonState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private FiniteStateMachine m_StateMachine;
    private Animator m_Animator;
    private BossBehaviour m_Boss;

    [Header("Amount of lightnings")]
    [SerializeField]
    private int m_NumberOfLightnings;

    [Header("Delay time between lightnings")]
    [SerializeField]
    private float m_DelayTime;

    [Header("Summoning duration")]
    [SerializeField]
    private float m_SummoningDuration;

    [Header("Summoning area radius")]
    [SerializeField]
    private int m_SummoningArea;

    [Header("Pool of projectiles/splats")]
    [SerializeField]
    private Pool m_Pool;

    [Header("Projectile/Attack Scriptable object")]
    [SerializeField]
    private ProjectileInfo m_ScriptableInfo;

    private string m_AttackName;
    private float m_AttackDamage;
    private float m_AttackRate;
    private float m_AttackSpeed;
    private Sprite m_AttackSprite;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        //Initialize(); //Initializes parameters on a given bullet
    }

    bool m_Summoning;

    float m_CurrentDuration = 0;
    void Update()
    {
        m_CurrentDuration += Time.deltaTime;
        if (m_CurrentDuration >= m_SummoningDuration)
            m_Summoning = false;
    }

    public override void InitState()
    {
        base.InitState();
        m_Summoning = true;
        m_Boss.SetBusy(true);
        StartCoroutine(SpawnLightnings());
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    private void Initialize()
    {
        m_AttackName = m_ScriptableInfo.AttackName;
        m_AttackDamage = m_ScriptableInfo.AttackDamage;
        m_AttackRate = m_ScriptableInfo.AttackRate;
        m_AttackSpeed = m_ScriptableInfo.AttackSpeed;
        m_AttackSprite = m_ScriptableInfo.AttackSprite;
    }

    private IEnumerator SpawnLightnings()
    {
        while(m_Summoning)
        {
            float randX = Random.Range(-m_SummoningArea, m_SummoningArea);
            float randY = Random.Range(-m_SummoningArea, m_SummoningArea);
            GameObject lightning = m_Pool.GetElement();
            lightning.transform.position = new Vector3(transform.position.x + randX, transform.position.y + randY, 0);
            lightning.SetActive(true);
            yield return new WaitForSeconds(m_DelayTime);
        }
        m_Boss.SetBusy(false);
    }
}
