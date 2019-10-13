using System.Collections.Generic;

namespace BotNetFun.MetaEnemy
{
    using BotNetFun.Loot.MetaItem;

    public class Enemy
    {
        public string Name { get; private set; }
        public EnemyContextInfo Info { get; private set; }
        public EnemyType EnemyType { get; private set; }
        public List<Item> LootDropPool { get; private set; }

        public Enemy (
            string _name,
            EnemyContextInfo _info,
            EnemyType et,
            List<Item> dp
        )
        {
            Name = _name;
            Info = _info;
            EnemyType = et;
            LootDropPool = dp;
        }
    }
}
