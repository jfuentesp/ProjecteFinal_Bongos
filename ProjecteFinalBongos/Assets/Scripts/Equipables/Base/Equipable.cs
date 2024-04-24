using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipable : ScriptableObject, IEquipable
{
    [SerializeField]
    protected string m_Id = "";
    public string id { get => m_Id; set => m_Id = value; }
    [SerializeField]
    protected string m_ItemName;
    public string itemName { get => m_ItemName; set => m_ItemName = value; }
    [SerializeField]
    protected string m_Description = "";
    public string description { get => m_Description; set => m_Description = value; }
    [SerializeField]
    protected Sprite m_Sprite = null;
    public Sprite Sprite { get => m_Sprite; set => m_Sprite = value; }
    [SerializeField]
    protected int m_ShopPrice;
    public int shopPrice { get => m_ShopPrice; set => m_ShopPrice = value; }
    [SerializeField]
    protected EstadosAlterados m_Estado = EstadosAlterados.Normal;
    public EstadosAlterados Estado { get => m_Estado; set => m_Estado = value; }

    public abstract void OnEquip(GameObject equipedBy);
    public abstract void OnWithdraw(GameObject equipedBy);
}
