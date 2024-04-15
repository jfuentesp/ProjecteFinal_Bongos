using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    private ObstacleStateEnum m_CurrentObstacleState;
    private Rigidbody2D m_Rigidbody;

    [Header("Splash damage on contact")]
    [SerializeField]
    private float m_SplashDamage;
    [Header("Splash slow time")]
    [SerializeField]
    private float m_SplashSlowTime;

    [Header("Obstacle State Effect Settings")]
    [SerializeField]
    private float m_LightningShootingTime;
    [SerializeField]
    private float m_LightningShootingDelay;

    private float m_ShootingTime;
    private Transform m_Target;

    [SerializeField]
    private Pool m_BulletPool;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Splash"))
        {
            Splash splash = collision.GetComponent<Splash>();
            Debug.Log("Soy el obstaculo y choco contra el Splash");
            if (splash.SplashEffectState == ObstacleStateEnum.ELECTRIFIED)
            {
                Debug.Log("Entro en shoot");
                ShootLightning();
            }
        }
    }

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Init(Transform target)
    {
        m_Target = target;
    }

    public void ShootLightning()
    {
        m_ShootingTime = m_LightningShootingTime;
        m_EffectCoroutine = StartCoroutine(ShootLightningCoroutine());
    }

    private Coroutine m_EffectCoroutine;
    private IEnumerator ShootLightningCoroutine()
    {
        while(m_LightningShootingTime > 0)
        {
            GameObject bullet = m_BulletPool.GetElement();
            bullet.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            bullet.SetActive(true);
            bullet.GetComponent<SinusBullet>().enabled = true;
            bullet.GetComponent<SinusBullet>().Init((m_Target.position - transform.position).normalized);
            m_ShootingTime -= m_LightningShootingDelay;
            yield return new WaitForSeconds(m_LightningShootingDelay);
        }
    }

}
