namespace BotNetFun.Loot.Enums
{
    [System.Serializable]
    public enum ItemContextInfo : byte
    {
        None = 0,

        #region Basic
        BasicArmor = 1,
        BasicWeapon = 2,
        BasicCharm = 3,
        #endregion Basic

        #region Defensive
        DefensiveArmor = 10,
        ParryingWeapon = 11,
        DefensiveCharm = 12,
        #endregion Defensive

        #region Offensive
        OffensiveArmor = 20,
        StrongWeapon = 21,
        OffensiveCharm = 22,
        #endregion Offensive

        #region Balanced
        BalancedArmor = 30,
        BalancedWeapon = 31,
        BalancedCharm = 32,
        #endregion Balanced

        #region BetterStats
        UniqueRare = 40,
        QuestRare = 41,
        SetRare = 42,
        HighTierRare = 43,
        EndgameRare = 44,
        SetEndgameRare = 45,
        Developer = 255,
        #endregion
    }
}
