using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StoreGUIController : MonoBehaviour
{
    [Header("Store GUI GameObject references")]
    [SerializeField]
    private GameObject m_GUIPanel;
    [Header("Store GUI Components")]
    [SerializeField]
    private InventoryController m_PlayerInventory;
    [Header("Arrays that represent each of Piccolo stores")]
    [SerializeField]
    private Consumable[] m_PiccoloStoreConsumables = new Consumable[10];
    [SerializeField]
    private Equipable[] m_PiccoloStoreEquipables = new Equipable[10];
    [Header("Lists including all the possible objects that piccolo can show in its store")]
    [SerializeField]
    private List<Consumable> m_ConsumableList = new List<Consumable>();
    [SerializeField]
    private List<Equipable> m_EquipableList = new List<Equipable>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadStore()
    {

    }

    private void LoadPlayerInventory()
    {

    }

    
}
