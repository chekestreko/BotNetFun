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
    }
}
