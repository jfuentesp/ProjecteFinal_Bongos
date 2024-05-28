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

    private Animator m_Animator;

    public override void OnUse(GameObject usedBy)
    {
        RaycastHit2D[] enemiesHit = Physics2D.CircleCastAll(usedBy.transform.position, m_AreaRadius, usedBy.transform.position, m_AreaRadius, m_Layermask);
        for(int i = 0; i < enemiesHit.Length; i++)
        {
            enemiesHit[i].collider.TryGetComponent<HealthController>(out HealthController healthcontroller);
            if (healthcontroller != null)
                healthcontroller.Damage(m_DamageAmount);
        }
    }
}
