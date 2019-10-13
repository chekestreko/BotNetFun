namespace BotNetFun.Loot.MetaItem
{
    using BotNetFun.Loot.Enums;

    public sealed class ItemExtraStat
    {
        public ItemBonusType BonusAffect { get; private set; }
        public ItemBonusType ExtraBonusAffect { get; private set; } = ItemBonusType.NoItemBonus;
        public int BonusContent { get; private set; }
        public int ExtraBonusContent { get; private set; } = 0;
        public bool MultiBonus { get; } = false;

        public ItemExtraStat(ItemBonusType bonus, int content = 0, ItemBonusType extraBonus = ItemBonusType.NoItemBonus, int extraContent = 0)
        {
            BonusAffect = bonus;
            if (bonus == ItemBonusType.NoItemBonus) return;
            if (bonus != ItemBonusType.NoItemBonus && content != 0)
                throw new System.InvalidOperationException("Item bonus is specified but content isn't specified");

            BonusContent = content;
            if (extraBonus != ItemBonusType.NoItemBonus && extraBonus != bonus && extraContent != 0)
            {
                ExtraBonusAffect = extraBonus;
                ExtraBonusContent = extraContent;
                MultiBonus = true;
            }
        }

        public static ItemExtraStat HealthBonus(int health)
            => new ItemExtraStat(ItemBonusType.MaxHealthBonus, health);

        public static ItemExtraStat DodgeBonus(int dodgeChance)
            => new ItemExtraStat(ItemBonusType.DodgeChanceBonus, dodgeChance);

        /// <summary>
        /// NOTE: first parameter is health
        /// </summary>
        public static ItemExtraStat BothBonuses(int health, int dodgeChance)
            => new ItemExtraStat(ItemBonusType.MaxHealthBonus, health, ItemBonusType.DodgeChanceBonus, dodgeChance);

        public static ItemExtraStat NoBonus()
            => new ItemExtraStat(ItemBonusType.NoItemBonus);
    }
}
