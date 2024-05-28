using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Backpack", menuName = "Inventory/Backpack")]
public class Backpack : ScriptableObject
{
    public class ConsumableSlot
    {
        public Consumable Consumable;
        public int Quantity;

        public ConsumableSlot(Consumable item)
        {
            Consumable = item;
            Quantity = 1;
        }
    }

    public class EquipableSlot
    {
        public Equipable Equipable;

        public EquipableSlot(Equipable item)
        {
            Equipable = item;
        }
    }

    private ConsumableSlot[] m_ConsumableSlots = new ConsumableSlot[25];
    private EquipableSlot[] m_EquipableSlots = new EquipableSlot[25];
    private ConsumableSlot[] m_QuickConsumableSlots = new ConsumableSlot[3];
    public ConsumableSlot[] ConsumableSlots => m_ConsumableSlots;
    public EquipableSlot[] EquipableSlots => m_EquipableSlots;



    public void AddConsumable(Consumable item)
    {
        ConsumableSlot itemSlot = GetConsumable(item);
        if (itemSlot == null)
        {
            int index = Array.FindIndex(m_ConsumableSlots, i => i == null);
            m_ConsumableSlots[index] = new ConsumableSlot(item);
        }
        else
        {
            if(itemSlot.Quantity < 99)
                itemSlot.Quantity++;
        }
    }

    public void AddConsumableStack(Consumable item, int quantity)
    {
        ConsumableSlot itemSlot = GetConsumable(item);
        if(itemSlot == null)
        {
            Debug.Log("Entro en el segundo if");
            int index = Array.FindIndex(m_ConsumableSlots, i => i == null);
            m_ConsumableSlots[index] = new ConsumableSlot(item);
            if (quantity < 99)
                m_ConsumableSlots[index].Quantity = quantity;
            else
                m_ConsumableSlots[index].Quantity = 99;
        }
        else
        {
            if (itemSlot.Quantity + quantity < 99)
                itemSlot.Quantity += quantity;
            else
                itemSlot.Quantity = 99;
        }
    }

    public void AddConsumableLoadGame(Consumable item, int quantity, int slotId)
    {
        m_ConsumableSlots[slotId] = new ConsumableSlot(item);
        m_ConsumableSlots[slotId].Quantity = quantity;
    }

    public void RemoveConsumable(Consumable item)
    {
        ConsumableSlot itemSlot = GetConsumable(item);
        if (itemSlot == null)
            return;
        if(itemSlot.Quantity > 0)
            itemSlot.Quantity--;
        if (itemSlot.Quantity <= 0)
        {
            int index = Array.FindIndex(m_ConsumableSlots, i => i == itemSlot);
            m_ConsumableSlots[index] = null;
        }
    }

    public void RemoveConsumableStack(Consumable item, int quantity)
    {
        ConsumableSlot itemSlot = GetConsumable(item);
        if (itemSlot == null)
            return;
        if(itemSlot.Quantity - quantity > 0) { }
            itemSlot.Quantity -= quantity;
        if(itemSlot.Quantity <= 0)
        {
            int index = Array.FindIndex(m_ConsumableSlots, i => i == itemSlot);
            m_ConsumableSlots[index] = null;
        }
    }

    public void MoveConsumable(int indexSelected, int indexTarget)
    {
        ConsumableSlot temp = m_ConsumableSlots[indexSelected];
        m_ConsumableSlots[indexSelected] = m_ConsumableSlots[indexTarget];
        m_ConsumableSlots[indexTarget] = temp;
    }

    public ConsumableSlot GetConsumable(Consumable item)
    {
        return Array.Find(m_ConsumableSlots, slot => slot?.Consumable == item); //Importante el interrogante para que compruebe si no es null
    }

    public void AddEquipable(Equipable item)
    {
        EquipableSlot itemSlot = GetEquipable(item);
        if (itemSlot == null)
        {
            int index = Array.FindIndex(m_EquipableSlots, i => i == null);
            m_EquipableSlots[index] = new EquipableSlot(item);
            Debug.Log("Entro en el itemslot: " + index + " | " + m_EquipableSlots[index].Equipable.itemName);
        }
    }

    public void AddEquipableLoadGame(Equipable item, int slotId)
    {
        m_EquipableSlots[slotId] = new EquipableSlot(item);
    }

    public void RemoveEquipable(Equipable item)
    {
        EquipableSlot itemSlot = GetEquipable(item);
        if (itemSlot == null)
            return;

        int index = Array.FindIndex(m_EquipableSlots, i => i == itemSlot);
        m_EquipableSlots[index] = null;
    }

    public void MoveEquipable(int indexSelected, int indexTarget)
    {
        EquipableSlot temp = m_EquipableSlots[indexSelected];
        m_EquipableSlots[indexSelected] = m_EquipableSlots[indexTarget];
        m_EquipableSlots[indexTarget] = temp;
    }

    public EquipableSlot GetEquipable(Equipable item)
    {
        return Array.Find(m_EquipableSlots, slot => slot?.Equipable == item); //Importante el interrogante para que compruebe si no es null
    }

    public int GetQuantity(Consumable consumable)
    {
        ConsumableSlot slot = Array.Find(m_ConsumableSlots, slot => slot?.Consumable == consumable);
        if (slot == null) 
            return 0;
        return slot.Quantity;
    }
}
