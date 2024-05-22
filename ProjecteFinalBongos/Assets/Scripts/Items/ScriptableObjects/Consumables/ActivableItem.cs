using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

[CreateAssetMenu(fileName = "Area Active Item", menuName = "Scriptables/Items/AreaActive")]
public class ActivableItem : Consumable
{
    [SerializeField]
    private LayerMask m_Layermask;
    [SerializeField]
    private float m_DamageAmount;
    [SerializeField]
    private float m_AreaRadius;
    [SerializeField]
    private float m_Duration;
    [SerializeField]
    private ActivableEnum m_ActivableTypeEnum;

    public override void OnUse(GameObject usedBy)
    {
        RaycastHit2D[] enemiesHit = Physics2D.CircleCastAll(usedBy.transform.position, m_AreaRadius, usedBy.transform.position, m_AreaRadius, m_Layermask);
        if (enemiesHit == null)
            return;
        switch (m_ActivableTypeEnum)
        {
            case ActivableEnum.DAMAGE:
                foreach (RaycastHit2D hitDamage in enemiesHit)
                {
                    hitDamage.collider.TryGetComponent(out HealthController healthcontroller);
                    if(healthcontroller != null)
                        healthcontroller.Damage(m_DamageAmount);
                }
                break;
            case ActivableEnum.POISON:
                foreach(RaycastHit2D hitPoison in enemiesHit)
                {
                    hitPoison.collider.TryGetComponent(out BossEstadosController bossEstados);
                    if (bossEstados != null)
                        bossEstados.AlternarEstado(EstadosAlterados.Enverinat);
                }
                break;
            case ActivableEnum.SLOW:
                foreach (RaycastHit2D hitSlow in enemiesHit)
                {
                    hitSlow.collider.TryGetComponent(out BossEstadosController bossEstados);
                    if (bossEstados != null)
                        bossEstados.AlternarEstado(EstadosAlterados.Mullat);
                }
                break;
        }
    }
}
