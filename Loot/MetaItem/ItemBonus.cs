namespace BotNetFun.Loot.MetaItem
{
    internal sealed class ItemBonus
    {
        public string BonusAffects { get; private set; }
        public double BonusContent { get; private set; }
        public ItemBonus(string bonus, double content)
        {
            BonusContent = content;
            switch (bonus)
            {
                case "MaxHealth": return;
                case "DodgeChance": return;
                default:
                    throw new System.InvalidOperationException("Invalid item bonus");
            }
        }
    }
}
