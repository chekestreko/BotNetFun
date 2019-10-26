namespace BotNetFun.Loot.MetaItem
{
    [System.Serializable]
    public class ItemData
    {
        public int Damage { get; set; } = 0;
        public int Defense { get; set; } = 0;
        public int CritChance { get; set; } = 0;
        public int CritDamage { get; set; } = 0;
        public double ItemValue { get; set; } = 0;

        public static ItemData IncompleteItemData { get; } = new ItemData
        {
            Damage = 0,
            Defense = 0,
            CritChance = 0,
            CritDamage = 0,
            ItemValue = 0
        };
    }
}
