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
        m_TornadoCoroutine = StartCoroutine(TornadoExplosion());
    }
    public override void ExitState()
    {
        base.ExitState();
        StopCoroutine(m_TornadoCoroutine);
        Destroy(tornado.gameObject);
    }
    private Coroutine m_TornadoCoroutine;

    private IEnumerator TornadoExplosion()
    {
        tornado = Instantiate(m_tornado, transform.parent);
        tornado.transform.position = transform.position;
        yield return new WaitForSeconds(1f);
        m_Animator.Play("Explosion");
        yield return new WaitForSeconds(2f);
        m_Animator.Play("ExplosionCloser");
        yield return new WaitForSeconds(0.5f);
        m_Animator.Play("idleDracMari");
        Destroy(tornado.gameObject);
        for (int x = -1; x < 2; x++) {
            for (int j = -1; j < 2; j++)
            {
                GameObject bullet = m_Pool.GetElement();
                bullet.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                bullet.SetActive(true);
                bullet.GetComponent<DracMBullet>().enabled = true;
                bullet.GetComponent<DracMBullet>().Init(new Vector2(x,j).normalized);
            }
        }
        yield return new WaitForSeconds(0.5f);
        OnTornadoFinished(gameObject);
    }
}
