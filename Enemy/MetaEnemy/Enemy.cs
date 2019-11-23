namespace BotNetFun.Enemy.MetaEnemy
{
    using BotNetFun.Enemy.Enums;
    using BotNetFun.MainGame.Enums;

    // todo: random generation, actually finish this class, names
    public class Enemy
    {
        public string Name { get; private set; }
        public EnemyContextInfo Info { get; private set; }
        public EnemyType Type { get; private set; }
        public EnemyData Data { get; private set; }
        public Location EnemyLocationContext { get; private set; }

        public Enemy(
            string _name,
            EnemyContextInfo _info,
            EnemyType et = EnemyType.Normal,
            EnemyData data = null,
            Location locContext = Location.Meadow
        )
        {
            Name = _name;
            Info = _info;
            Type = et;
            if (data is null)
                Data = EnemyData.IncompleteEnemyData;
            if (locContext == Location.Meadow)
                EnemyLocationContext = Location.Meadow;
        }
    }
}
