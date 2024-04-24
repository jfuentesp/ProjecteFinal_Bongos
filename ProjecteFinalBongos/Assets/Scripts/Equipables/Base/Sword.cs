using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Sword", menuName = "Equipable/Sword")]
public class Sword : Equipable
{
    public List<string> propiedades;
    public float attack;
    public float speed;
    public float speedAttack;
    public float maxRatio;
    public float minRatio;
    public float minPosibilty;
    public float maxPosibilty;
    private float lifeStealed;
    public float lifeMin;
    public float lifeMax;
    public void Regenerate(GameObject boss, GameObject player)
    {
        if (!boss.TryGetComponent(out HealthController bosshealth))
        {
            return;
        }
        lifeStealed = Random.Range(lifeMin, lifeMax);
        bosshealth.Damage(lifeStealed);
        player.GetComponent<HealthController>().Heal(lifeStealed);

    }
    public void ChangeState(GameObject go)
    {
        if (!go.TryGetComponent(out BossEstadosController bec))
        { return; }
        float random = Random.Range(minRatio, maxRatio);
        if (random >= minPosibilty && random <= maxPosibilty)
        {
            bec.AlternarEstado(Estado);
        }

    }
}
