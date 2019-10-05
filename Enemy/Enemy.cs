namespace BotNetFun.Enemy
{
    using BotNetFun.Loot;

    internal abstract class Enemy
    {
        public string Name { get; protected set; }
        public int Health { get; protected set; }
        public int Damage { get; protected set; }
        public int Defense { get; protected set; }
        public int DodgeChance { get; protected set; }
        public int CritChance { get; protected set; }
        public int CritDamage { get; protected set; }
        public double GoldDrop { get; protected set; }
        public EnemyType EnemyType { get; protected set; }
        public DropPool LootDropPool { get; protected set; }

        protected Enemy(
            string _name,
            int _health,
            int _damage,
            int _cc,
            int _cd,
            double _gd,
            EnemyType et,
            DropPool dp
        )
        {
            Name = _name;
            Health = _health;
            Damage = _damage;
            CritChance = _cc;
            CritDamage = _cd;
            GoldDrop = _gd;
            EnemyType = et;
            LootDropPool = dp;
        }
    }
}
