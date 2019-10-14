namespace BotNetFun.Loot.MetaItem
{
    using BotNetFun.Loot.Enums;

    [System.Serializable]
    public sealed class ItemBonus
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ItemBonusContext Context { get; private set; }
        public ItemBonus(string name, string description, ItemBonusContext cont)
        {
            Name = name;
            Description = description;
            Context = cont;
        }

        public static ItemBonus NoBonus => new ItemBonus("None", "N/A", ItemBonusContext.None);
    }
}
