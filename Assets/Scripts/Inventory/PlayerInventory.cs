using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
public class PlayerInventory : MonoBehaviour
{
    private List<ItemInstance> items = new();
    private ItemInstance equipped;

    public event Action<ItemDefinition, int> OnItemChanged;
    public event Action<ItemInstance> OnEquippedChanged;

    public ItemInstance Equipped => equipped;

    public bool TryAddItem(ItemDefinition def) 
    {
        if (def == null) return false;        

        int current = GetCount(def);
        if (def.maxStack > 0 && current >= def.maxStack) return false;

        var inst = new ItemInstance
        {
            def = def,
            stack = 1,
            durability = 0,
            instanceGuid = System.Guid.NewGuid().ToString()
        };

        if (inst.def.consimeOnPickup) 
        {
            return inst.TryUse(new UseContext
            {
                user = gameObject,
                target = gameObject
            });            
        }

        items.Add(inst);
        OnItemChanged?.Invoke(def, GetCount(def));

        // auto-equip if nothing equipped
        if (equipped == null) Equip(inst);

        return true;
    }
    public bool TryRemoveItem(ItemInstance inst) 
    {
        if (inst == null) return false;
        if (!items.Remove(inst)) return false;

        OnItemChanged?.Invoke(inst.def, GetCount(inst.def));

        if (equipped == inst) Equip(null);
        return true;
    }
    public void Equip(ItemInstance inst) 
    {
        equipped = inst;
        OnEquippedChanged?.Invoke(inst);
    }
    public int GetCount(ItemDefinition def)
    {
        int total = 0;
        foreach (var it in items)
            if (it != null && it.def == def)
                total += Mathf.Max(1, it.stack); // or it.stack if you use stacks per instance
        return total;
    }
}
