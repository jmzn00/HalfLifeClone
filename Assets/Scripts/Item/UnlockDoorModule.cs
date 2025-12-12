using UnityEngine;

[CreateAssetMenu(menuName = "Items/Modules/UnlockDoor")]
public class UnlockDoorModule : ItemModule
{
    public string keyId;

    public override bool CanUse(UseContext ctx, ItemInstance item) 
    {
        if(ctx.target == null)
            return false;
        if (ctx.target.TryGetComponent(out KeyPad keypad))
        {
            return keypad.TryUnlock(keyId);          
        }
        return false;
    }
    public override void Use(UseContext ctx, ItemInstance item)
    {
        base.Use(ctx, item);
    }
}
