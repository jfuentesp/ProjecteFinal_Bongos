using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInfo : ScriptableObject
{
    [SerializeField]
    private string m_AttackName;
    [SerializeField]
    private float m_AttackDamage;
    [SerializeField]
    private float m_AttackRate;
    [SerializeField]
    private float m_AttackSpeed;
    [SerializeField]
    private Sprite m_AttackSprite;
    
    public string AttackName => m_AttackName;
    public float AttackDamage => m_AttackDamage;
    public float AttackRate => m_AttackRate;
    public float AttackSpeed => m_AttackSpeed;
    public Sprite AttackSprite => m_AttackSprite; 
}
