using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackDamage : MonoBehaviour
{
    [SerializeField]
    private float m_Damage;
    [SerializeField]
    private PlayerStatsController m_StatsController;
    [SerializeField]
    private GameObject m_player;
    public float Damage => m_Damage;

    private void Start()
    {
        m_StatsController = GetComponentInParent<PlayerStatsController>();
    }
    public void ChangeAttack(float damage)
    {
        m_Damage = damage;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("BossHurtBox"))
        {
            if (m_StatsController.Sword != null) {
                foreach (EquipablePropertiesEnum propiedad in m_StatsController.Sword.propiedades)
                {
                    switch (propiedad)
                    {
                        case EquipablePropertiesEnum.ESTADO:
                            m_StatsController.Sword.ChangeState(collision.gameObject);
                            break;
                        case EquipablePropertiesEnum.VAMPIRO:
                            m_StatsController.Sword.Regenerate(collision.gameObject, m_player);
                            break;
                    }
                }
            }    
     
        }

    }
}
