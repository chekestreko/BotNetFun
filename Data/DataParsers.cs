namespace BotNetFun.Data
{
    using BotNetFun.Loot.Enums;
    using BotNetFun.Loot.MetaItem;
    using BotNetFun.MetaEnemy;

    public static class DataParsers
    {
        public static string ParseItemInfo(Item item) => item.Type switch
        {
            ItemType.Helmet => "Helmet",
            ItemType.Chestplate => "Chestplate",
            ItemType.Gauntlet => "Gauntlet",
            ItemType.Pants => "Pants",
            ItemType.Boots => "Boots",
            ItemType.Primary => "Primary Item",
            ItemType.Secondary => "Secondary Item",
            ItemType.Charm => "Charm",
            _ => throw new System.InvalidOperationException("Item has unknown type (?)"),
        };
    }
}
