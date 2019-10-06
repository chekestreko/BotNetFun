using System.Collections.Generic;

namespace BotNetFun.Data
{
    using BotNetFun.Loot.Enums;
    using BotNetFun.Loot.MetaItem;
    using BotNetFun.Enemy;

    internal static class Collections
    {
        static Collections()
        {

        }

        public static Dictionary<string, Item> Items { get; private set; } = new Dictionary<string, Item>();
        public static Dictionary<string, BaseEnemy> Enemies { get; private set; } = new Dictionary<string, BaseEnemy>();

        private static void AddToItems(Item item)
            => Items.Add(item.Name, item);

        private static void AddToEnemies(BaseEnemy enemy)
           => Enemies.Add(enemy.Name, enemy);


        internal static string ParseItemInfo(Item item)
        {
            switch(item.Type)
            {
                case ItemType.Helmet:
                    return "Helmet";
                case ItemType.Chestplate:
                    return "Chestplate";
                case ItemType.Gauntlet:
                    return "Gauntlet";
                case ItemType.Pants:
                    return "Pants";
                case ItemType.Boots:
                    return "Boots";
                case ItemType.Primary:
                    return "Primary Item";
                case ItemType.Secondary:
                    return "Secondary Item";
                case ItemType.Charm:
                    return "Charm";
                default:
                    throw new System.InvalidOperationException("Item has unknown type (?)");
            }
        }
    }
}
