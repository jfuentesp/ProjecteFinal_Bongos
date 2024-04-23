using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Equipable/Sword/WaterSword")]
public class WaterSword : StateSword {
    private void OnEnable()
    {
        Estado = EstadosAlterados.Mullat;
    }
}
