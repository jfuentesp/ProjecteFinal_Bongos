using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Placeable Item", menuName = "Scriptables/Items/Placeable Item")]
public class PlaceableItem : Consumable
{
    [SerializeField]
    private GameObject m_PrefabToInstantiateAndPlace;
    [SerializeField]
    private PlaceableEnum m_PlaceableType;
    public override void OnUse(GameObject usedBy)
    {
        GameObject objectToPlace = Instantiate(m_PrefabToInstantiateAndPlace);
        if (!objectToPlace.TryGetComponent<PlaceableController>(out PlaceableController placeable))
            return;        
        objectToPlace.transform.position = usedBy.transform.position;
        placeable.Initialize(m_PlaceableType, this);
    }
}
