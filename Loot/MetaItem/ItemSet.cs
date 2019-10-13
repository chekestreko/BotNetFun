namespace BotNetFun.Loot.MetaItem
{
    using BotNetFun.Loot.Enums;

    public sealed class ItemSet
    {
        public string Name { get; private set; }
        public byte SetId { get; private set; }
        public string Description { get; private set; }
        public ItemSetContext Context { get; private set; }
        public ItemSet(string name, byte setid, string description, ItemSetContext cont)
        {
            Name = name;
            SetId = setid;
            Description = description;
            Context = cont;
        }
    }
}
