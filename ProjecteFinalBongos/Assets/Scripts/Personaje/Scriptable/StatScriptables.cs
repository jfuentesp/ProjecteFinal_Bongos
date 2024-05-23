using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Stat", menuName = "Estadisticas/Stat - Scriptable")]
public class StatScriptables : ScriptableObject
{
    [SerializeField]
    private string m_StatId;
    [SerializeField]
    private string m_StatName;
    [SerializeField]
    private string m_StatDescripcion;
    [SerializeField]
    private Sprite m_StatSprite;
    [SerializeField]
    private StatType m_StatType;

    public string Id => m_StatId;
    public string Name => m_StatName;
    public string Descripcion => m_StatDescripcion;
    public Sprite Sprite => m_StatSprite;
    public StatType TipoStat => m_StatType;
}
