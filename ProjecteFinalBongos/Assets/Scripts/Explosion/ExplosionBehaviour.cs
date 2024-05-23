using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    [SerializeField]
    private ExplosionType m_ExplosionType;
    [SerializeField]
    private float m_ExplosionDamage;
    [SerializeField]
    private float m_ExplosionForce;
    [SerializeField]
    private float m_PoisonDuration;
    [SerializeField]
    private float m_SlowDuration;
    [SerializeField]
    private LayerMask m_LayersAffected;

    public void InitializeExplosion(ExplosionType explosionType, float explosionDamage = 5f, float explosionForce = 10f, float poisonDuration = 5f, float slowDuration = 5f)
    {
        m_ExplosionType = explosionType;
        m_ExplosionDamage = explosionDamage;
        m_ExplosionForce = explosionForce;
        m_PoisonDuration = poisonDuration;
        m_SlowDuration = slowDuration;
    }

    private void OnEnable()
    {
        OnExplode();
    }

    public void OnExplode()
    {
        switch(m_ExplosionType) 
        {
            case ExplosionType.EXPLOSION:
                RaycastHit2D [] explosionhits = Physics2D.CircleCastAll(transform.position, transform.localScale.y, transform.position, transform.localScale.y, m_LayersAffected);
                foreach(RaycastHit2D hit in explosionhits )
                {
                    hit.collider.gameObject.TryGetComponent(out HealthController targetHealth);
                    hit.collider.gameObject.TryGetComponent(out Rigidbody2D targetRigidbody);
                    if(targetRigidbody != null)
                        targetRigidbody.AddForce((targetRigidbody.transform.position - transform.position).normalized * m_ExplosionForce, ForceMode2D.Impulse);
                    if(targetHealth != null)
                        targetHealth.Damage(m_ExplosionDamage);
                    if(hit.collider.CompareTag("Player"))
                    {
                        hit.collider.gameObject.TryGetComponent(out PlayerEstadosController playerEstadosController);
                        if (playerEstadosController != null)
                            playerEstadosController.AlternarEstado(EstadosAlterados.Atordit, 0.5f);
                    }
                    if(hit.collider.CompareTag("Enemy"))
                    {
                        hit.collider.gameObject.TryGetComponent(out BossEstadosController bossEstadosController);
                        if (bossEstadosController != null)
                            bossEstadosController.AlternarEstado(EstadosAlterados.Atordit);
                    }
                }
                break;
            case ExplosionType.POISON:
                RaycastHit2D[] poisonhits = Physics2D.CircleCastAll(transform.position, transform.localScale.y, transform.position, transform.localScale.y, m_LayersAffected);
                foreach(RaycastHit2D hit in  poisonhits)
                {
                    if(hit.collider.CompareTag("Player"))
                    {
                        hit.collider.gameObject.TryGetComponent(out PlayerEstadosController playerEstados);
                        if(playerEstados != null)
                            playerEstados.AlternarEstado(EstadosAlterados.Enverinat, m_PoisonDuration);
                    }
                    if(hit.collider.CompareTag("Enemy"))
                    {
                        hit.collider.gameObject.TryGetComponent(out BossEstadosController bossEstados);
                        if (bossEstados != null)
                            bossEstados.AlternarEstado(EstadosAlterados.Enverinat);
                    }
                }
                break;
            case ExplosionType.SLOW:
                RaycastHit2D[] slowhits = Physics2D.CircleCastAll(transform.position, transform.localScale.y, transform.position, transform.localScale.y, m_LayersAffected);
                foreach (RaycastHit2D hit in slowhits)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        hit.collider.gameObject.TryGetComponent(out PlayerEstadosController playerEstados);
                        if (playerEstados != null)
                            playerEstados.AlternarEstado(EstadosAlterados.Mullat, m_SlowDuration);
                    }
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        hit.collider.gameObject.TryGetComponent(out BossEstadosController bossEstados);
                        if (bossEstados != null)
                            bossEstados.AlternarEstado(EstadosAlterados.Mullat);
                    }
                }
                break;
        }
    }
}
