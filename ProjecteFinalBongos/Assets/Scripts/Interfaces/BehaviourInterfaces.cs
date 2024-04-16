using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealable
{
    public void Heal(float healAmount);
    public void Regenerate(float regenerationAmount, float duration, float tickDelay);
}

public interface IDamageable
{
    public void Damage(float damageAmount);
}

public interface IThrowable
{
    public void Throw(GameObject objectToThrow, Vector2 direction, float speed);
}

public interface IBuffable
{
    public void AttackBuff(float statAmount, float duration);
    public void DefenseBuff(float statAmount, float duration);
    public void SpeedBuff(float statAmount, float duration);
}

public interface IDebuffable
{
    public void Debuff(List<GameObject> targets);
    public void Debuff(GameObject target);
    public void Debuff(float areaRadius, float duration);
}

public interface IPlaceable
{
    public void Place(GameObject objectToPlace);
}

public interface IPShieldable
{
    public void Shield(GameObject shieldObject, float duration);
}
