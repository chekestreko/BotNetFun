namespace BotNetFun.MainGame
{
    using BotNetFun.Loot.MetaItem;
    using BotNetFun.MainGame.Enums;

    [System.Serializable]
    public sealed class Player
    { 
        public double MaxHealth { get; set; }
        public double Health { get; set; }
        public double DodgeChance { get; set; }
        public double Defense { get; set; }
        public double Damage { get; set; }
        public double CritChance { get; set; }
        public double CritDamage { get; set; }
        public byte Level { get; set; }
        public double XP { get; set; }
        public double Gold { get; set; }
        public bool InBattle { get; set; }
        public Location CurrentLocation { get; set; }
        public PlayerClass Class { get; set; }

        public Item Helmet { get; set; }
        public Item Chestplate { get; set; }
        public Item Gauntlets { get; set; }
        public Item Pants { get; set; }
        public Item Boots { get; set; }
        public Item PrimaryItem { get; set; }
        public Item SecondaryItem { get; set; }
        public Item Charm { get; set; }

        public Item I1 { get; set; }
        public Item I2 { get; set; }
        public Item I3 { get; set; }
        public Item I4 { get; set; }
        public Item I5 { get; set; }
        public Item I6 { get; set; }
        public Item I7 { get; set; }
        public Item I8 { get; set; }
        public Item I9 { get; set; }
        public Item I10 { get; set; }
        public Item I11 { get; set; }
        public Item I12 { get; set; }
        public Item I13 { get; set; }
        public Item I14 { get; set; }
        public Item I15 { get; set; }
        public Item I16 { get; set; }
        public Item I17 { get; set; }
        public Item I18 { get; set; }
        public Item I19 { get; set; }
        public Item I20 { get; set; }
    }
}
