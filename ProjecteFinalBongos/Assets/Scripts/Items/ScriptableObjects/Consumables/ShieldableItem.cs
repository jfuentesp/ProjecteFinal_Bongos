using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield Item", menuName = "Scriptables/Items/Shield Item")]
public class ShieldableItem : Consumable
{
    [SerializeField]
    private float m_ShieldDuration;
    public override void OnUse(GameObject usedBy)
    {
        usedBy.TryGetComponent<IShieldable>(out IShieldable shieldable);
        if (shieldable != null)
            shieldable.Shield(usedBy, m_ShieldDuration);
    }
}
