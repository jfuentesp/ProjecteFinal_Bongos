using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class KrakenRangedAttackState : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;
    [Header("Pool of projectiles/splats")]
    [SerializeField]
    private Pool m_Pool;
    [SerializeField]
    private string m_ShootAnimation;
    [SerializeField]
    private Transform m_ShootPoint;
    private Transform m_Target;
    [SerializeField]
    private string m_WaitAnimation;
    [SerializeField]
    private float minWaitTime;
    [SerializeField]
    private float maxWaitTime;
    private bool derecha;
    public bool m_TwoDirections;
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
    }

    private void GetTarget()
    {
        m_Target = m_Boss.Target;
    }

    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true); 
        m_Rigidbody.velocity = Vector3.zero;
        StartCoroutine(AttackAnimationRoutine());
    }
    private IEnumerator AttackAnimationRoutine()
    {
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        if (m_TwoDirections)
        {
            m_Animator.Play(m_WaitAnimation);
            if (m_Target != null)
            {
                if (m_Target.position.x - transform.position.x < 0)
                {
                    derecha = false;
                }
                else
                {
                    derecha = true;
                }
            }
        }
        yield return new WaitForSeconds(waitTime);

        if (m_TwoDirections)
        {
            m_Animator.Play(m_ShootAnimation);
            if (m_Target != null)
            {
                if (m_Target.position.x - transform.position.x < 0)
                {
                    derecha = false;
                }
                else
                {
                    derecha = true;
                }
            }
        }
    }
    private void Shoot() {
        GameObject lightning = m_Pool.GetElement();
        lightning.transform.position = new Vector3(m_ShootPoint.position.x, m_ShootPoint.position.y, 0);
        lightning.SetActive(true);
        lightning.GetComponent<TintaBullet>().enabled = true;
        lightning.GetComponent<TintaBullet>().Init(m_Target.position - transform.position);
    }

    private void End() {
        onAttackStopped.Invoke(gameObject);
    }

    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();

    }
    // Update is called once per frame
    void Update()
    {
        if (derecha)
            transform.localEulerAngles = Vector3.zero;
        else
            transform.localEulerAngles = new Vector3(0, 180, 0);

    }
}
