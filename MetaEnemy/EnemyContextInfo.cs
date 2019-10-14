namespace BotNetFun.MetaEnemy
{
    public enum EnemyContextInfo : byte
    {
        #region NormalContextInfo - Dual
        Normal = 0,
        NormalPlus = 1,
        NormalPlusPlus = 2,
        LiterallyHarmless = 9,
        #endregion 

        #region TankyContextInfo - Health
        Tough = 10,
        Strong = 11,
        Resilient = 12,
        Turtle = 13,
        #endregion

        #region HurtfulContextInfo - Damage
        Unkind = 20,
        Harmful = 21,
        Malicious = 22,
        Destructive = 23,
        #endregion HurtfulContextInfo - Damage

        #region BigContextInfo - EliteOrBoss
        DualBig = 30,
        HealthBig = 31,
        DamageBig = 32,
        RaidDualBig = 33,
        #endregion

        #region EndgameContextInfo
        Legendary = 40,
        Godlike = 50,
        Impossible = 255
        #endregion 

    }
}
