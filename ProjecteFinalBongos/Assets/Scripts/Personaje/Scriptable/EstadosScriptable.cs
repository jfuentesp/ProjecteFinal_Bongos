using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Estado", menuName = "Estados/Estados - Scriptable")]
public class EstadosScriptable : ScriptableObject
{
    [SerializeField]
    private string m_EstadoId;
    [SerializeField]
    private string m_EstadoName;
    [SerializeField]
    private string m_EstadoDescripcion;
    [SerializeField]
    private Sprite m_EstadoSprite;
    [SerializeField]
    private EstadosAlterados m_Estado;
    
    public string Id => m_EstadoId;
    public string Name => m_EstadoName;
    public string Descripcion => m_EstadoDescripcion;
    public Sprite Sprite => m_EstadoSprite;
    public EstadosAlterados Estado => m_Estado;
}
