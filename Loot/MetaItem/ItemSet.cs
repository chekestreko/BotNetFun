namespace BotNetFun.Loot.MetaItem
{
    // TODO: literally everything
    internal sealed class ItemSet
    {
        public string Name { get; private set; }
        public int SetId { get; private set; }
        public string Description { get; private set; }
        public ItemSet(string name, int setid)
        {
            Name = name;
            SetId = setid;
        }
    }
}
