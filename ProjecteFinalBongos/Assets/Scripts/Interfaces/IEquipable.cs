using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable
{
    public string Id { get; set; }
    public string Description { get; set; }
    public Sprite Sprite { get; set; }

    public void ApplyStats();
    public void UnapplyStats();
}
