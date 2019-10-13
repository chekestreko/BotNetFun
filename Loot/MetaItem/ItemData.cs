namespace BotNetFun.Loot.MetaItem
{
    public struct ItemData
    {
        public int Damage { get; set; } 
        public int Defense { get; set; } 
        public int CritChance { get; set; } 
        public int CritDamage { get; set; } 
        public double ItemValue { get; set; }

        public void Default()
        {
            Damage = 0;
            Defense = 0;
            CritChance = 0;
            CritDamage = 0;
        }
    }
}
