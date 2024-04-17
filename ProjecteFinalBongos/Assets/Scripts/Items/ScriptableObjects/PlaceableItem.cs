using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableItem : Consumable
{
    [SerializeField]
    private GameObject m_PrefabToInstantiateAndPlace;
    public override void OnUse(GameObject usedBy)
    {
        GameObject objectToPlace = Instantiate(m_PrefabToInstantiateAndPlace);
        objectToPlace.transform.position = usedBy.transform.position;
    }
}
