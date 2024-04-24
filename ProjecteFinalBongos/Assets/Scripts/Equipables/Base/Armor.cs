using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Armor", menuName ="Equipable/Armor")]
public class Armor : Equipable
{
    public List<EquipablePropertiesEnum> propiedades;
    public float defense;
    public float speed;
    public float ElementalResistance;
    public float stateResistance;
    public float stateResistanceMin;
    public float stateResistanceMax;
    public float maxRatio;
    public float minRatio;
    public float minPosibilty;
    public float maxPosibilty;
    public float regenMin;
    public float regenMax;
    public float RegenTime; 
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

    public IEnumerator Regen(GameObject player) {
        while (true) {
            yield return new WaitForSeconds(RegenTime);
            float lifeRegen = Random.Range(regenMin, regenMax);
            player.GetComponent<HealthController>().Heal(lifeRegen);

        }
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

    public override void OnEquip(GameObject equipedBy)
    {
        equipedBy.TryGetComponent<PlayerStatsController>(out PlayerStatsController playerController);
        if (playerController != null)
        {
            playerController.EquipArmor(this);
            Debug.Log("Equipada la armadura " + this.itemName);
        }

        //Actualizo inventario
        //Hago cosas
    }

    public override void OnWithdraw(GameObject equipedBy)
    {
        equipedBy.TryGetComponent<PlayerStatsController>(out PlayerStatsController playerController);
        if (playerController != null)
        {
            playerController.UnequipArmor();
            Debug.Log("Retirada la armadura " + this.itemName);
        }
    }
}
