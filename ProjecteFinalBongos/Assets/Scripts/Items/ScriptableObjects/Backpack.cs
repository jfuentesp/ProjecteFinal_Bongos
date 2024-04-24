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
    public ConsumableSlot[] ConsumableSlots => m_ConsumableSlots;
    public EquipableSlot[] EquipableSlots => m_EquipableSlots;

    public void AddConsumable(Consumable item)
    {
        ConsumableSlot itemSlot = GetConsumable(item);
        if (itemSlot == null)
        {
            int index = Array.FindIndex(m_ConsumableSlots, i => i == null);
            m_ConsumableSlots[index] = new ConsumableSlot(item);
            Debug.Log("Añadido objeto " + item.itemName + " || Item => " + m_ConsumableSlots[index].Consumable.itemName);
        }
        else
        {
            itemSlot.Quantity++;
            Debug.Log("Aumentada la cantidad en 1 al objeto " + item.itemName);
        }
    }

    public void RemoveConsumable(Consumable item)
    {
        ConsumableSlot itemSlot = GetConsumable(item);
        if (itemSlot == null)
            return;

        itemSlot.Quantity--;
        if (itemSlot.Quantity <= 0)
        {
            int index = Array.FindIndex(m_ConsumableSlots, i => i == itemSlot);
            m_ConsumableSlots[index] = null;
        }
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
            Debug.Log("Añadido equipable " + item.itemName + " || Item => " + m_EquipableSlots[index].Equipable.itemName);
        }
    }

    public void RemoveEquipable(Equipable item)
    {
        EquipableSlot itemSlot = GetEquipable(item);
        if (itemSlot == null)
            return;

        int index = Array.FindIndex(m_EquipableSlots, i => i == itemSlot);
        m_EquipableSlots[index] = null;
    }

    public EquipableSlot GetEquipable(Equipable item)
    {
        return Array.Find(m_EquipableSlots, slot => slot?.Equipable == item); //Importante el interrogante para que compruebe si no es null
    }
}
