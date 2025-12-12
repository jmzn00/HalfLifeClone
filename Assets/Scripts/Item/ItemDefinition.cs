using System.Globalization;
using UnityEngine;
public enum ItemCategory { Key, Consumable }
[System.Serializable]
public class ItemInstance 
{
    public ItemDefinition def;
    public int stack = 1;
    public int durability;
    public string instanceGuid;

    public bool TryUse(UseContext ctx)
    {
        if (def == null || def.modules == null)
            return false;

        // First pass: validation
        foreach (var m in def.modules)
        {
            if (m != null && !m.CanUse(ctx, this))
                return false;
        }

        // Second pass: execution
        foreach (var m in def.modules)
        {
            m?.Use(ctx, this);
        }

        return true;
    }
}
public abstract class ItemModule : ScriptableObject 
{
    public virtual bool CanUse(UseContext ctx, ItemInstance item) => true;
    public virtual void Use(UseContext ctx, ItemInstance item) { }
}
public struct UseContext 
{
    public GameObject user;
    public GameObject target;
}
[CreateAssetMenu(menuName = "Items/ItemDefinition")]
public class ItemDefinition : ScriptableObject
{
    public string displayName;
    public GameObject mesh;
    public Sprite icon;
    public ItemCategory category;
    public int maxStack = 1;
    public bool consimeOnPickup = false;

    public ItemModule[] modules;    
}
