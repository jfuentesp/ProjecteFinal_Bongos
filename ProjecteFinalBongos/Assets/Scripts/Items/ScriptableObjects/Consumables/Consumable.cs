using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : ScriptableObject, IConsumable
{
    [Header("Item attributes")]
    [SerializeField]
    protected string m_ItemId; 
    public string id { get => m_ItemId; set => m_ItemId = value; }
    [SerializeField]
    protected string m_ItemName;
    public string itemName { get => m_ItemName; set => m_ItemName = value; }
    [SerializeField]
    protected string m_ItemDescription;
    public string description { get => m_ItemDescription; set => m_ItemDescription = value; }
    [SerializeField]
    protected Sprite m_ItemSprite;
    public Sprite Sprite { get => m_ItemSprite; set => m_ItemSprite = value; }
    [SerializeField]
    protected int m_ShopPrice;
    public int shopPrice { get => m_ShopPrice; set => m_ShopPrice = value; }

    public abstract void OnUse(GameObject usedBy);
}
