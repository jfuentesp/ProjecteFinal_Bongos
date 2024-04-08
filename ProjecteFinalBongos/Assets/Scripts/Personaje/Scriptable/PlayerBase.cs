using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Player/PlayerBase")]
public class PlayerBase : ScriptableObject
{
    public float m_BaseVelocity;
    public float m_BaseAttackTime;
    public float m_BaseStrongAttack;
    public float m_BaseWeakAttack;
    public float m_BaseDefense;
    public float m_BaseStrength;

}
