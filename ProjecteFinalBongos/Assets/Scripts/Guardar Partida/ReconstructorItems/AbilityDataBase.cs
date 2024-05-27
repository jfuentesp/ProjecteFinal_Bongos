using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityEquipable", menuName = "Scriptables/DataBases/AbilityDataBase")]
public class AbilityDataBase : ScriptableObject
{
    [SerializeField]
    public List<Ability> abilities = new List<Ability>();

    public Ability GetItemByID(string id) => abilities.FirstOrDefault<Ability>(ability => ability.id == id);
}
