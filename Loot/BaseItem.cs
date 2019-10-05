namespace BotNetFun.Loot
{
    using BotNetFun.Loot.Ability;
    using BotNetFun.Loot.ItemEnums;

    internal abstract class BaseItem
    {
        public string Name { get; protected set; }
        public int Health { get; protected set; }
        public int Damage { get; protected set; }
        public int Defense { get; protected set; }
        public int DodgeChance { get; protected set; }
        public int CritChance { get; protected set; }
        public int CritDamage { get; protected set; }
        public double GoldDrop { get; protected set; }
        public ItemRarity Rarity { get; protected set; }
        public ItemType Type { get; protected set; }
        public BaseAbility Ability { get; protected set; }

        protected BaseItem(
            string _name,
            int _health,
            int _damage,
            int _cc,
            int _cd,
            double _gd,
            ItemRarity rare,
            ItemType ty,
            BaseAbility ab
        )
        {
            Name = _name;
            Health = _health;
            Damage = _damage;
            CritChance = _cc;
            CritDamage = _cd;
            GoldDrop = _gd;
            Rarity = rare;
            Type = ty;
            Ability = ab;
        }

        public static BaseItem Instantiate<T>() where T : BaseItem, new()
            => new T();
    }
}
