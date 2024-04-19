using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        //Lo mismo que arriba
    }

    private List<ConsumableSlot> m_ConsumableSlots = new List<ConsumableSlot>();
    private List<EquipableSlot> m_EquipableSlots = new List<EquipableSlot>();
    public ReadOnlyCollection<ConsumableSlot> ConsumableSlots => new ReadOnlyCollection<ConsumableSlot>(m_ConsumableSlots);
    public ReadOnlyCollection<EquipableSlot> EquipableSlots => new ReadOnlyCollection<EquipableSlot>(m_EquipableSlots);

    public void AddConsumable(Consumable item)
    {
        ConsumableSlot itemSlot = GetConsumable(item);
        if (itemSlot == null)
            m_ConsumableSlots.Add(new ConsumableSlot(item));
        else
            itemSlot.Quantity++;
    }

    public void RemoveConsumable(Consumable item)
    {
        ConsumableSlot itemSlot = GetConsumable(item);
        if (itemSlot == null)
            return;

        itemSlot.Quantity--;
        if (itemSlot.Quantity <= 0)
            m_ConsumableSlots.Remove(itemSlot);
    }

    public ConsumableSlot GetConsumable(Consumable item)
    {
        return m_ConsumableSlots.FirstOrDefault(slot => slot.Consumable == item);
    }

    public void AddEquipable(/*Equipable item*/)
    {
        EquipableSlot itemSlot = GetEquipable(/*item*/);
        //if (itemSlot == null)
            //m_EquipableSlots.Add(new EquipableSlot(item));
        //else
            //itemSlot.Quantity++;
    }

    public void RemoveEquipable(/*Equipable item*/)
    {
        EquipableSlot itemSlot = GetEquipable(/*item*/);
        if (itemSlot == null)
            return;

        //itemSlot.Quantity--;
        //if (itemSlot.Quantity <= 0)
            //m_EquipableSlots.Remove(itemSlot);
    }

    public EquipableSlot GetEquipable(/*Equipable item*/)
    {
        //return m_EquipableSlots.FirstOrDefault(slot => slot.Equipable == item);
        return null;
    }
}
