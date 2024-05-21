using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProximityItemBehaviour : Interactuable
{
    [SerializeField] private Consumable m_Consumible;
    [SerializeField] private Equipable m_Equipable;

    protected override void Start()
    {
        base.Start();
        if (m_Consumible != null)
        {
            m_SpriteRenderer.sprite = m_Consumible.Sprite;
        }
        else if (m_Equipable != null)
        {
            m_SpriteRenderer.sprite = m_Equipable.Sprite;
        }
    }
    
    protected override void Interact(InputAction.CallbackContext context)
    {
        if (inRange) {
           if (m_Consumible != null)
            {
                LevelManager.Instance.InventoryController.BackPack.AddConsumable(m_Consumible);
                LevelManager.Instance.InventoryController.RefreshInventoryGUI();
                Destroy(gameObject);
            }   
            else if (m_Equipable != null)
            {
                LevelManager.Instance.InventoryController.BackPack.AddEquipable(m_Equipable);
                LevelManager.Instance.InventoryController.RefreshInventoryGUI();
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
    public void SetSprite(Sprite sprite)
    {
        if(m_SpriteRenderer != null)
        {
            m_SpriteRenderer.sprite = sprite;
        }
    }
}
