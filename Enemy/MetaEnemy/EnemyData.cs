namespace BotNetFun.Enemy.MetaEnemy
{
    using BotNetFun.MainGame.Enums;

    public sealed class EnemyData
    {
        public double MaxHealth { get; set; } = 0;
        public double Health {
            get => backHealth;
            set
            {
                if (value >= MaxHealth)
                    value = MaxHealth;
                backHealth = value;
            }
        }
        private double backHealth; 

        public double Defense { get; set; } = 0;
        public double Damage { get; set; } = 0;
        public double CritChance { get; set; } = 0;
        public double CritDamage { get; set; } = 0;
        public double DodgeChance { get; set; } = 0;
        public double GoldDrop { get; set; } = 0;

        public static EnemyData IncompleteEnemyData { get; } = new EnemyData
        {
            MaxHealth = 0,
            Health = 0,
            // backing field for health
            backHealth = 0,
            //
            Defense = 0,
            Damage = 0,
            CritChance = 0,
            CritDamage = 0,
            DodgeChance = 0,
            GoldDrop = 0
        };

        public static EnemyData GetEnemyData(Enemy enemy, byte playerLevel, Location locationContext)
        { }
    }
}
