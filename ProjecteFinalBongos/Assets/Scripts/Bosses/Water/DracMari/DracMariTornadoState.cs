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
        GameObject bullet = m_Pool.GetElement();
        bullet.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        bullet.SetActive(true);
        bullet.GetComponent<DracMBullet>().enabled = true;
        bullet.GetComponent<DracMBullet>().Init(transform.up);
        GameObject bullet2 = m_Pool.GetElement();
        bullet2.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        bullet2.SetActive(true);
        bullet2.GetComponent<DracMBullet>().enabled = true;
        bullet2.GetComponent<DracMBullet>().Init(-transform.up);
        GameObject bullet3 = m_Pool.GetElement();
        bullet3.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        bullet3.SetActive(true);
        bullet3.GetComponent<DracMBullet>().enabled = true;
        bullet3.GetComponent<DracMBullet>().Init(transform.right);
        GameObject bullet4 = m_Pool.GetElement();
        bullet4.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        bullet4.SetActive(true);
        bullet4.GetComponent<DracMBullet>().enabled = true;
        bullet4.GetComponent<DracMBullet>().Init(-transform.right);
        GameObject bullet5 = m_Pool.GetElement();
        bullet5.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        bullet5.SetActive(true);
        bullet5.GetComponent<DracMBullet>().enabled = true;
        bullet5.GetComponent<DracMBullet>().Init(transform.up + transform.right);
        GameObject bullet6 = m_Pool.GetElement();
        bullet6.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        bullet6.SetActive(true);
        bullet6.GetComponent<DracMBullet>().enabled = true;
        bullet6.GetComponent<DracMBullet>().Init(transform.up - transform.right);
        GameObject bullet7 = m_Pool.GetElement();
        bullet7.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        bullet7.SetActive(true);
        bullet7.GetComponent<DracMBullet>().enabled = true;
        bullet7.GetComponent<DracMBullet>().Init(-transform.up - transform.right);
        GameObject bullet8 = m_Pool.GetElement();
        bullet8.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        bullet8.SetActive(true);
        bullet8.GetComponent<DracMBullet>().enabled = true;
        bullet8.GetComponent<DracMBullet>().Init(-transform.up + transform.right);
        yield return new WaitForSeconds(0.5f);
        OnTornadoFinished(gameObject);
    }
}
