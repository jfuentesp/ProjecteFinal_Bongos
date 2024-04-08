using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealable
{
    public void Heal(float healAmount);
}

public interface IDamageable
{
    public void Damage(float damageAmount);
}
