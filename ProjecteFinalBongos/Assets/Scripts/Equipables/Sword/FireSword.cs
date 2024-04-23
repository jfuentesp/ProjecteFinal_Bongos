using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Equipable/Sword/FireSword")]
public class FireSword : StateSword {
    private void OnEnable()
    {
        Estado = EstadosAlterados.Cremat;
    }
}
