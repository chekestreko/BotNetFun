﻿using System.Collections.Generic;

namespace BotNetFun.MetaEnemy
{
    using BotNetFun.Loot.MetaItem;

    public class Enemy
    {
        public string Name { get; private set; }
        public EnemyContextInfo Info { get; private set; }
        public EnemyType EnemyType { get; private set; }

        public Enemy (
            string _name,
            EnemyContextInfo _info,
            EnemyType et = EnemyType.Normal
        )
        {
            Name = _name;
            Info = _info;
            EnemyType = et;
        }
    }
}