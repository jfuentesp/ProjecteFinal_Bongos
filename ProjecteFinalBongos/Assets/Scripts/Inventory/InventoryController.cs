using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_InventorySlotPrefab;

    [SerializeField]
    private List<Consumable> m_Consumables = new List<Consumable>();
    //Otra lista para los equipables

    [Header("Consumable Grid settings")]
    [SerializeField]
    private GridLayout m_ConsumableGrid;
    [SerializeField]
    private int m_ConsumableGridColumns;
    [SerializeField]
    private int m_ConsumableGridRows;


    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void BuildConsumableGUI()
    {

    }

}
