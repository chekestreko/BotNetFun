namespace BotNetFun.Loot.Enums
{
    [System.Serializable]
    public enum ItemContextInfo : byte
    {
        None = 0,

        #region Normal
        NormalArmor = 1,
        NormalWeapon = 2,
        NormalCharm = 3,
        #endregion 

        #region Defensive
        DefensiveArmor = 10,
        ParryingWeapon = 11,
        DefensiveCharm = 12,
        #endregion 

        #region Offensive
        OffensiveArmor = 20,
        StrongWeapon = 21,
        OffensiveCharm = 22
        #endregion
    }
}
