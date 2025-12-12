using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Item
{
    [CreateAssetMenu(menuName = "Items/Modules/HealModule")]
    public class HealModule : ItemModule
    {
        public float healAmount = 15f;
        public override bool CanUse(UseContext ctx, ItemInstance item)
        {
            return GameServices.Player.Health.TryHeal(healAmount);
        }
        public override void Use(UseContext ctx, ItemInstance item)
        {
            base.Use(ctx, item);
        }
    }
}