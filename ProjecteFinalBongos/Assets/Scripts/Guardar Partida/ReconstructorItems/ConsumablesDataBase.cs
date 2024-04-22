using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;


[CreateAssetMenu(fileName = "DatabaseConsumables", menuName = "Scriptables/DataBases/ConsumableDataBase")]
public class ConsumablesDataBase : ScriptableObject
{
    [SerializeField]
    List<Consumable> consumables = new List<Consumable>();

    public Consumable GetItemByID(string id) => consumables.FirstOrDefault<Consumable>(consumbable => consumbable.id == id);
}

