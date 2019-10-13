using System.Collections.Generic;

namespace BotNetFun.Data
{
    using BotNetFun.Loot.Enums;
    using BotNetFun.Loot.MetaItem;
    using BotNetFun.MetaEnemy;

    public static class Collections
    {
        static Collections()
        {

        }

        public static Dictionary<string, Item> Items { get; private set; } = new Dictionary<string, Item>();
        public static Dictionary<string, Enemy> Enemies { get; private set; } = new Dictionary<string, Enemy>();
        public static Dictionary<string, ItemSet> ItemSets { get; private set; } = new Dictionary<string, ItemSet>();

        private static void AddToItems(Item item)
            => Items.Add(item.Name, item);

        private static void AddToEnemies(Enemy enemy)
            => Enemies.Add(enemy.Name, enemy);

        private static void AddToItemSets(ItemSet itemSet)
            => ItemSets.Add(itemSet.Name, itemSet);

        public static string ParseItemInfo(Item item)
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
