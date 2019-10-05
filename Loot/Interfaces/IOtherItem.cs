namespace BotNetFun.Loot.Interfaces
{
    using BotNetFun.Loot.Ability;
    internal interface IOtherItem
    {
        void AbilityAction();
        OtherBaseAbility OtherAbility { get; protected set; }
    }
}
