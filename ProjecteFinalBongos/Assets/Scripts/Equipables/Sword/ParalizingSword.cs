using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Equipable/Sword/ParalizingSword")]
public class ParalizingSword : StateSword
{
    private void OnEnable()
    {
        Estado = EstadosAlterados.Paralitzat;
    }
}
