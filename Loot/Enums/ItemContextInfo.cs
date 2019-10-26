namespace BotNetFun.Loot.Enums
{
    [System.Serializable]
    [System.Flags]
    public enum ItemContextInfo : byte
    {
        None = 0,

        NormalArmor = 1,
        NormalWeapon = 2,
        NormalCharm = 3,

        TankyArmor = 10,
        ParryingWeapon = 11,
        ShieldingCharm = 12,

        OffensiveArmor = 20,
        StrongWeapon = 21,
        DamagingCharm = 22,
    }
}
