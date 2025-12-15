using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Item
{
    [CreateAssetMenu(menuName = "Items/Modules/AmmoModule")]
    public class AmmoModule : ItemModule
    {
        public AmmoType type;
        public int amount;

        public override bool CanUse(UseContext ctx, ItemInstance item)
        {
            GameServices.Player.Weapons.AddAmmo(type, amount);
            return true;
        }
        public override void Use(UseContext ctx, ItemInstance item)
        {
            base.Use(ctx, item);
        }
    }
}