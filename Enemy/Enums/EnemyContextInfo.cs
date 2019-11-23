namespace BotNetFun.Enemy.Enums
{
    public enum EnemyContextInfo : byte
    {
        // Normal enemies
        Normal = 0,
        NormalPlus = 1,
        NormalPlusPlus = 2,
        LiterallyHarmless = 3,

        // Tanky enemies
        Tough = 4,
        Strong = 5,
        Resilient = 6,
        Turtle = 7,

        // Hurtful enemies
        Unkind = 8,
        Harmful = 9,
        Malicious = 10,
        Destructive = 11,

        // Enemies which are "efficient in both"
        DualBig = 12,
        RaidDualBig = 13,

        // Midgame/High-midgame/Endgame
        Myth = 14,
        Legendary = 15,
        Beastlike = 16,
        Godlike = 17,
        Impossible = 100,
    }
}
