using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProximityItemBehaviour : Interactuable
{
    [SerializeField] private Consumable m_Consumible;
    [SerializeField] private Equipable m_Equipable;
    private SpriteRenderer m_SpriteRenderer;
    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        if (m_Consumible != null)
        {
            m_SpriteRenderer.sprite = m_Consumible.Sprite;
        }
        else if (m_Equipable != null)
        {
           m_SpriteRenderer.sprite = m_Equipable.Sprite;
        }
    }
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
