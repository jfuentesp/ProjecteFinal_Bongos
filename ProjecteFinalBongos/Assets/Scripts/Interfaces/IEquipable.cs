using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable
{
    public string id { get; set; }
    public string itemName { get; set; }
    public string description { get; set; }
    public Sprite Sprite { get; set; }
    public int shopPrice { get; set; }
    public EstadosAlterados Estado { get; set; }

    public void OnEquip(GameObject usedBy);
}
