using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeArmor : Armor, ISpike
{
    private float spikeDamage;
    public float spikeDamageMin;
    public float spikeDamageMax;
    public void Spike(GameObject boss)
    {
        if (!boss.TryGetComponent(out HealthController bosshealth))
            return;
        spikeDamage = Random.Range(spikeDamageMin, spikeDamageMax);
        bosshealth.Damage(spikeDamage);
    }
}
