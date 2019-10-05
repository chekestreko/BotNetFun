namespace BotNetFun.Loot.Interfaces
{
    using BotNetFun.Loot.Ability;
    internal interface IArmor
    {
        void OnTakingHit();
        ArmorBaseAbility ArmorAbility { get; protected set; }
    }
}
