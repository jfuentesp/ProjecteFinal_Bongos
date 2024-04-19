using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Equipable/Sword/PoisonSword")]
public class PoisonSword : StateSword {
    private void OnEnable()
    {
        Estado = EstadosAlterados.Enverinat;
    }
    
}
