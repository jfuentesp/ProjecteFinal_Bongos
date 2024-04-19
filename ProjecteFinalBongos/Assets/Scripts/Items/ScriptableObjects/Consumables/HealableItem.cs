using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Healing Item", menuName = "Scriptables/Items/Healing")]
public class HealableItem : Consumable
{
    [Header("Healing parameters")]
    [SerializeField]
    private float m_HealingAmount;

    public override void OnUse(GameObject usedBy)
    {
        if (!usedBy.TryGetComponent<IHealable>(out IHealable healthController))
            return;
        
        healthController.Heal(m_HealingAmount);
    }
}
