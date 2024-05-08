using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProximityItemBehaviour : Interactuable
{
    [SerializeField] private Consumable m_Consumible;
    [SerializeField] private Equipable m_Equipable;

    public override void Interact(InputAction.CallbackContext context)
    {
        if (inRange) {
           if (m_Consumible != null)
            {
                PJSMB.Instance.Inventory.BackPack.AddConsumable(m_Consumible);
                PJSMB.Instance.Inventory.RefreshInventoryGUI();
                Destroy(gameObject);
            }   
            else if (m_Equipable != null)
            {
                PJSMB.Instance.Inventory.BackPack.AddEquipable(m_Equipable);
                PJSMB.Instance.Inventory.RefreshInventoryGUI();
                Destroy(gameObject);
            }
        }
        
    }
    public void SetEquipable(Equipable equipable)
    {
        m_Equipable = equipable;
    }
    public void SetConsumable(Consumable consumable) 
    { 
        m_Consumible = consumable;
    }
    private void OnDestroy()
    {
        StopCoroutine(check());
        PJSMB.Instance.Input.FindActionMap("PlayerActions").FindAction("Interact").performed -= Interact;
    }
}
