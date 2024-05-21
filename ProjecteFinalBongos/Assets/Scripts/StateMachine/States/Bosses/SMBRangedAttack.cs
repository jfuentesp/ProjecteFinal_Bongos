using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SMBRangedAttack : SMState
{
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;
    [SerializeField] private float m_TiempoAutoAtaque;

    [Header("Pool of projectiles/splats")]
    [SerializeField]
    private Pool m_Pool;

    [SerializeField] private bool m_TwoDirections;
    [SerializeField] private string m_RangedAttackAnimationName;
    [SerializeField]
    private string m_WaitAnimation;
    [SerializeField]
    private float minWaitTime;
    [SerializeField]
    private float maxWaitTime;
    private bool derecha;

    private Transform m_Target;

    public delegate void OnStopAttacking(GameObject obj);
    public OnStopAttacking onAttackStopped;

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

        if (m_RangedAttackAnimationName == String.Empty)
            print("ay");
        else
            AttackAnimation();
    }

    private void AttackAnimation()
    {
        if (m_TwoDirections)
        {
            m_Animator.Play(m_RangedAttackAnimationName);

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
            m_Animator.Play(m_RangedAttackAnimationName);
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
    private void DispararAtaque()
    {
        GameObject lightning = m_Pool.GetElement();
        lightning.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        lightning.SetActive(true);
        lightning.GetComponent<AlteaBullet>().enabled = true;
        lightning.GetComponent<AlteaBullet>().Init(m_Target.position - transform.position);
    }

    private void EndAttack()
    {
        onAttackStopped?.Invoke(gameObject);
    }

    private IEnumerator Disparar()
    {
        yield return new WaitForSeconds(m_TiempoAutoAtaque);
        GameObject lightning = m_Pool.GetElement();
        lightning.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        lightning.SetActive(true);
        lightning.GetComponent<AlteaBullet>().enabled = true;
        lightning.GetComponent<AlteaBullet>().Init(m_Target.position - transform.position);
    }

    public override void ExitState()
    {
        base.ExitState();
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
