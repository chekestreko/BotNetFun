namespace BotNetFun.Loot.Interfaces
{
    using BotNetFun.Loot.Ability;
    internal interface IHeldItem
    {
        void OnUse();
        void CriticalUse();
        HeldItemBaseAbility HeldItemAbility { get; protected set; }
    }
}
