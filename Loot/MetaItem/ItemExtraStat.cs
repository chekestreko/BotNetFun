namespace BotNetFun.Loot.MetaItem
{
    using BotNetFun.Loot.Enums;

    [System.Serializable]
    public sealed class ItemExtraStat
    {
        public ItemExtraStatContext BonusAffect { get; private set; }
        public ItemExtraStatContext ExtraBonusAffect { get; private set; } = ItemExtraStatContext.NoItemBonus;
        public int BonusContent { get; private set; }
        public int ExtraBonusContent { get; private set; } = 0;
        public bool MultiBonus { get; } = false;

        public ItemExtraStat(ItemExtraStatContext bonus, int content = 0, ItemExtraStatContext extraBonus = ItemExtraStatContext.NoItemBonus, int extraContent = 0)
        {
            BonusAffect = bonus;
            if (bonus == ItemExtraStatContext.NoItemBonus) return;
            if (bonus != ItemExtraStatContext.NoItemBonus && content != 0)
                throw new System.InvalidOperationException("Item bonus is specified but content isn't specified");

            BonusContent = content;
            if (extraBonus != ItemExtraStatContext.NoItemBonus && extraBonus != bonus && extraContent != 0)
            {
                ExtraBonusAffect = extraBonus;
                ExtraBonusContent = extraContent;
                MultiBonus = true;
            }
        }

        public static ItemExtraStat HealthBonus(int health)
            => new ItemExtraStat(ItemExtraStatContext.MaxHealthBonus, health);

        public static ItemExtraStat DodgeBonus(int dodgeChance)
            => new ItemExtraStat(ItemExtraStatContext.DodgeChanceBonus, dodgeChance);

        /// <summary>
        /// NOTE: first parameter is health
        /// </summary>
        public static ItemExtraStat BothBonuses(int health, int dodgeChance)
            => new ItemExtraStat(ItemExtraStatContext.MaxHealthBonus, health, ItemExtraStatContext.DodgeChanceBonus, dodgeChance);

        public static ItemExtraStat NoExtraStat { get; } = new ItemExtraStat(ItemExtraStatContext.NoItemBonus);
    }
}
