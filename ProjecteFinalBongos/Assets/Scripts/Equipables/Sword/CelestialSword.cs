using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Equipable/Sword/CelestialSword")]
public class CelestialSword : Sword, IVampire
{
    private float lifeStealed;
    public float lifeMin;
    public float lifeMax;
    public void Regenerate(GameObject boss, GameObject player)
    {
        if (!boss.TryGetComponent(out HealthController bosshealth)) {
            return;
        }
        lifeStealed = Random.Range(lifeMin, lifeMax);
        bosshealth.Damage(lifeStealed);
        player.GetComponent<HealthController>().Heal(lifeStealed);
          
    }
}
