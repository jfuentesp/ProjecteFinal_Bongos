using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Healing Item", menuName = "Scriptables/Items/Healing")]
public class DamageableItem : Consumable
{
    public override void OnUse(GameObject usedBy)
    {
        if (!usedBy.TryGetComponent<IHealable>(out IHealable healthController))
            return;

        healthController.Heal(m_HealingAmount);
    }
}
