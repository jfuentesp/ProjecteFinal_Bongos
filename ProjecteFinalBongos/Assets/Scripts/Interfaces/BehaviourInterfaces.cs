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
    public IEnumerator AttackBuff(float statAmount, float duration);
    public IEnumerator DefenseBuff(float statAmount, float duration);
    public IEnumerator SpeedBuff(float statAmount, float duration);
    public void BuffStat(StatType statToBuff, float statAmount, float duration);
}

public interface IDebuffable
{
    public IEnumerator AttackDebuff(float statAmount, float duration);
    public IEnumerator DefenseDebuff(float statAmount, float duration);
    public IEnumerator SpeedDebuff(float statAmount, float duration);
    public void DebuffStat(StatType statToDebuff, float statAmount, float duration);
}

public interface IPlaceable
{
    public void Place(GameObject objectToPlace);
}

public interface IShieldable
{
    public void Shield(GameObject shieldObject, float duration);
}
