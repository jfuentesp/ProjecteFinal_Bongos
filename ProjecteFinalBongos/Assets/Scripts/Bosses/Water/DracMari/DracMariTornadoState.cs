using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class DracMariTornadoState : SMState
{
    [SerializeField] private GameObject m_tornado;
    private GameObject tornado;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private BossBehaviour m_Boss;
    private Pool m_Pool;
    public Action<GameObject> OnTornadoFinished;
    [SerializeField] private string m_BeginExplosionAnimation;
    [SerializeField] private string m_MidExplosionAnimation;
    [SerializeField] private string m_WaitAnimation;
    [SerializeField]
    private float minWaitTime;
    [SerializeField]
    private float maxWaitTime;
    private new void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Boss = GetComponent<BossBehaviour>();
        m_Pool = LevelManager.Instance._BulletPool;
    }

    public override void InitState()
    {
        base.InitState();
        m_Boss.SetBusy(true);
        StartCoroutine(Wait());
    }
    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();
        if(tornado != null)
            Destroy(tornado.gameObject);
    }

    private IEnumerator Wait()
    {
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
  
            m_Animator.Play(m_WaitAnimation);

        yield return new WaitForSeconds(waitTime);
        BeginExplosion();


    

    }

    private void BeginExplosion() {
        m_Animator.Play(m_BeginExplosionAnimation);
        tornado = Instantiate(m_tornado, transform.parent);
        tornado.transform.position = transform.position;
    
    }
    private void MidExplosion() {
        m_Animator.Play(m_MidExplosionAnimation);
    }
    private void FinishExplosion() {
        Destroy(tornado.gameObject);
        for (int x = -1; x < 2; x++)
        {
            for (int j = -1; j < 2; j++)
            {
                GameObject bullet = m_Pool.GetElement();
                bullet.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                bullet.SetActive(true);
                bullet.GetComponent<DracMBullet>().enabled = true;
                bullet.GetComponent<DracMBullet>().Init(new Vector2(x, j).normalized);
            }
        }
        OnTornadoFinished(gameObject);
    }
}
