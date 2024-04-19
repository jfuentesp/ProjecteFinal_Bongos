using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConsumable
{
    public string id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public Sprite Sprite { get; set; }
    public int shopPrice { get; set; }

    public void OnUse(GameObject usedBy);
}
