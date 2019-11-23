using System;

namespace BotNetFun.Loot.MetaItem
{
    using BotNetFun.Data;
    using BotNetFun.Loot.Enums;

    [Serializable]
    public sealed class ItemExtraStat
    {
        public ItemExtraStatContext BonusAffect { get; private set; }
        public ItemExtraStatContext ExtraBonusAffect { get; private set; } = ItemExtraStatContext.NoItemBonus;
        public double BonusContent { get; private set; }
        public double ExtraBonusContent { get; private set; } = 0;
        public bool MultiBonus { get; } = false;

        public ItemExtraStat(ItemExtraStatContext bonus, double content = 0, ItemExtraStatContext extraBonus = ItemExtraStatContext.NoItemBonus, double extraContent = 0)
        {
            BonusAffect = bonus;
            if (bonus == ItemExtraStatContext.NoItemBonus) return;
            if (bonus != ItemExtraStatContext.NoItemBonus && content != 0)
                throw new InvalidOperationException("Item bonus is specified but content isn't specified");

            BonusContent = content;
            if (extraBonus != ItemExtraStatContext.NoItemBonus && extraBonus != bonus && extraContent != 0)
            {
                ExtraBonusAffect = extraBonus;
                ExtraBonusContent = extraContent;
                MultiBonus = true;
            }
        }

        public static ItemExtraStat HealthBonus(double health)
            => new ItemExtraStat(ItemExtraStatContext.MaxHealthBonus, health);

        public static ItemExtraStat DodgeBonus(double dodgeChance)
            => new ItemExtraStat(ItemExtraStatContext.DodgeChanceBonus, dodgeChance);

        /// <summary>
        /// NOTE: first parameter is health
        /// </summary>
        public static ItemExtraStat BothBonuses(double health, double dodgeChance)
            => new ItemExtraStat(ItemExtraStatContext.MaxHealthBonus, health, ItemExtraStatContext.DodgeChanceBonus, dodgeChance);

        public static ItemExtraStat NoExtraStat { get; } = new ItemExtraStat(ItemExtraStatContext.NoItemBonus);

        public static ItemExtraStat GetRandomItemExtraStat(byte playerLevel)
        {
            double levelAsTenth = playerLevel * 0.01;

            double healthContext = Globals.RoundSlightly(playerLevel * (Globals.Rnd.NextDouble() * 1.55));
            double dodgeChanceContext = Globals.RoundSlightly((Globals.Rnd.NextDouble() + (Globals.Rnd.NextDouble() - 0.10)) * ((levelAsTenth + 0.07) * 5));

            double bonus1ScaleValidation = levelAsTenth * (1.243232 + (levelAsTenth / 1.22));
            double bonus2ScaleValidation = levelAsTenth * (1.0810773 + (levelAsTenth / 1.32));

            bool randomStatContext1Validation = Globals.Rnd.NextDouble() < bonus1ScaleValidation;
            bool randomStatContext2Validation = Globals.Rnd.NextDouble() < bonus2ScaleValidation;

            if (bonus1ScaleValidation > 1)
                healthContext += Globals.RoundSlightly(bonus1ScaleValidation - Math.Truncate(bonus1ScaleValidation));
            if (bonus2ScaleValidation > 1)
                dodgeChanceContext += Globals.RoundSlightly(bonus2ScaleValidation - Math.Truncate(bonus2ScaleValidation));

            if (randomStatContext1Validation == true && randomStatContext2Validation == false)
                return HealthBonus(healthContext);
            else if (randomStatContext1Validation == false && randomStatContext2Validation == true)
                return DodgeBonus(dodgeChanceContext);
            else if (randomStatContext1Validation == true && randomStatContext2Validation == true)
                return BothBonuses(healthContext, dodgeChanceContext);
            else return NoExtraStat;
        }
    }
}
