using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    [SerializeField]
    private Transform m_Target;

    [SerializeField]
    private Pool m_BulletPool;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Splash"))
        {
            Splash splash = collision.GetComponents<Splash>().FirstOrDefault(splash => splash.isActiveAndEnabled);
            if (splash.SplashEffectState == ObstacleStateEnum.ELECTRIFIED)
            {
                ShootLightning();
            }
        }
    }

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnDestroy()
    {
        if(m_EffectCoroutine != null)
            StopCoroutine(m_EffectCoroutine);
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
        while(m_ShootingTime > 0)
        {
            GameObject bullet = m_BulletPool.GetElement();
            Vector2 direction = (m_Target.position - transform.position).normalized;
            Vector2 offset = new Vector2(transform.localScale.x + bullet.transform.localScale.x, transform.localScale.y + bullet.transform.localScale.y) + direction;
            bullet.transform.position = new Vector2(transform.position.x, transform.position.y) - offset;
            bullet.SetActive(true);
            bullet.GetComponent<SinusBullet>().enabled = true;
            bullet.GetComponent<SinusBullet>().Init((direction));
            m_ShootingTime -= m_LightningShootingDelay;
            yield return new WaitForSeconds(m_LightningShootingDelay);
        }
    }

}
