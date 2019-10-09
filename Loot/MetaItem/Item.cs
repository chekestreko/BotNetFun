namespace BotNetFun.Loot.MetaItem
{
    using BotNetFun.Loot.Enums;

    internal sealed class Item
    {
        public string Name { get; private set; }
        public int Damage { get; private set; }
        public int Defense { get; private set; }
        public int CritChance { get; private set; }
        public int CritDamage { get; private set; }
        public double ItemValue { get; private set; }
        public Rarity Rarity { get; private set; }
        public ItemType Type { get; private set; }
        public ItemBonus Bonus { get; private set; }
        public ItemSet Set { get; private set; }
        public Item (
            string _name,
            int _damage = 0,
            int _cc = 0,
            int _cd = 0,
            double _iv = 0,
            Rarity rare = Rarity.Common,
            ItemType ty = ItemType.Primary,
            ItemBonus _bo = null,
            ItemSet _set = null
        )
        {
            Name = _name;
            Damage = _damage;
            CritChance = _cc;
            CritDamage = _cd;
            ItemValue = _iv;
            Rarity = rare;
            Type = ty;
            Bonus = _bo;
            Set = _set;
        }
        
        public static void CreateItem(string _n, int dmg, int cc, int cd, double iv, Rarity _rare, ItemType t, ItemBonus bo, ItemSet itset) 
        => new Item(_n, _damage: dmg, _cc: cc, _cd: cd, _iv: iv, rare: _rare, ty: t, _bo: bo, _set: itset)
    }
}
