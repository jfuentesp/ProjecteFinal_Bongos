using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DatabaseEquipable", menuName = "Scriptables/DataBases/EquipableDataBase")]
public class EquipableDataBase : ScriptableObject
{
    [SerializeField]
    public List<Equipable> equipables = new List<Equipable>();

    public Equipable GetItemByID(string id) => equipables.FirstOrDefault<Equipable>(equipable => equipable.id == id);
}
