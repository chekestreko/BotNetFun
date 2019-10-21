using System;

namespace BotNetFun.Loot.MetaItem
{
    using BotNetFun.Data;
    using BotNetFun.Loot.Enums;

    [Serializable]
    public sealed class Item
    {
        public string Name { get; private set; }
        public ItemContextInfo Info { get; private set; }
        public Rarity Rarity { get; private set; }
        public ItemType Type { get; private set; }
        public ItemBonusContext Bonus { get; private set; }
        public ItemExtraStat ExtraStat { get; private set; }
        public ItemEfficientClass EfficientClass { get; private set; }
        public ItemData Data { get; private set; }

        public Item(
            string _name,
            byte playerLevel,
            ItemContextInfo info = ItemContextInfo.None,
            Rarity rare = Rarity.None,
            ItemType ty = ItemType.None,
            ItemBonusContext bo = ItemBonusContext.None,
            ItemExtraStat _st = null,
            ItemEfficientClass ec = ItemEfficientClass.None,
            ItemData data = null
        )
        {
            Name = _name ?? "None";
            Info = info;
            Rarity = rare;
            Type = ty;
            Bonus = bo;
            if (_st is null) ExtraStat = ItemExtraStat.NoExtraStat;
            EfficientClass = ec;
            if (data is null) Data = GetItemData(this, playerLevel);
        }

        public static Item GetEmptyItem { get; } = new Item("None", 0, ItemContextInfo.None, Rarity.None, ItemType.None);

        // todo: work on
        public static Item GenerateRandomItem(byte playerLevel)
        {
            double levelAsTenth = playerLevel * 0.01;

            Rarity randomRarity = GetRandomEnum<Rarity>();

            if (randomRarity == Rarity.Developer || randomRarity == Rarity.None)
                return GenerateRandomItem(playerLevel);

            ItemContextInfo randomContextInfo = GetRandomEnum<ItemContextInfo>();
            if (randomContextInfo == ItemContextInfo.None)
                return GenerateRandomItem(playerLevel);

            ItemType randomType = GetRandomEnum<ItemType>();
            if (randomType == ItemType.None)
                GenerateRandomItem(playerLevel);

            ItemBonusContext randomBonusContext = GetRandomEnum<ItemBonusContext>();
            ItemEfficientClass randomEfficientClass = GetRandomEnum<ItemEfficientClass>();
            ItemExtraStat randomExtraStat = GetRandomItemExtraStat(playerLevel);

            if (playerLevel <= 84 && randomRarity == Rarity.Extraordinary)
                DecrementRarity(ref randomRarity);

            if (playerLevel <= 67 && randomRarity == Rarity.Mythical)
                DecrementRarity(ref randomRarity);

            if (playerLevel <= 54 && randomRarity == Rarity.Fabled)
                DecrementRarity(ref randomRarity);

            if (playerLevel <= 39 && randomRarity == Rarity.Legendary)
                DecrementRarity(ref randomRarity);

            if (playerLevel <= 26 && randomRarity == Rarity.Epic)
                DecrementRarity(ref randomRarity);

            if (playerLevel <= 17 && randomRarity == Rarity.Rare)
                DecrementRarity(ref randomRarity);

            if (playerLevel <= 12 && randomRarity == Rarity.Refined)
                DecrementRarity(ref randomRarity);

            if (playerLevel <= 7 && randomRarity == Rarity.Uncommon)
                DecrementRarity(ref randomRarity);

            if (playerLevel <= 3 && randomRarity == Rarity.Common)
                DecrementRarity(ref randomRarity);

            if (Globals.Rnd.NextDouble() - (levelAsTenth / 5) < levelAsTenth * 2 && randomRarity != Rarity.Extraordinary)
                IncrementRarity(ref randomRarity);
        } 

        private static T GetRandomEnum<T>() where T : Enum
            => (T)Enum.GetValues(typeof(T)).GetValue(Globals.Rnd.Next(Enum.GetValues(typeof(T)).Length));

        private static void DecrementRarity(ref Rarity rare)
            => rare = (Rarity)((byte)rare - 1);

        private static void IncrementRarity(ref Rarity rare)
            => rare = (Rarity)((byte)rare + 1);

        // todo: balancing fixes 
        private static ItemExtraStat GetRandomItemExtraStat(byte playerLevel)
        {
            double levelAsTenth = playerLevel * 0.01;

            int healthContext = RoundThenToInt(playerLevel * (Globals.Rnd.NextDouble() * 1.55));
            int dodgeChanceContext = RoundThenToInt((Globals.Rnd.NextDouble() + (Globals.Rnd.NextDouble() - 0.10)) * ((levelAsTenth + 0.07) * 5));

            double bonus1ScaleValidation = levelAsTenth * (1.243232 + (levelAsTenth / 1.22));
            double bonus2ScaleValidation = levelAsTenth * (1.0810773 + (levelAsTenth / 1.32));

            bool randomStatContext1Validation = Globals.Rnd.NextDouble() < bonus1ScaleValidation;
            bool randomStatContext2Validation = Globals.Rnd.NextDouble() < bonus2ScaleValidation;

            if (bonus1ScaleValidation > 1)
                healthContext += RoundThenToInt(bonus1ScaleValidation - Math.Truncate(bonus1ScaleValidation));
            if (bonus2ScaleValidation > 1)
                dodgeChanceContext += RoundThenToInt(bonus2ScaleValidation - Math.Truncate(bonus2ScaleValidation));

            if (randomStatContext1Validation == true && randomStatContext2Validation == false)
                return ItemExtraStat.HealthBonus(healthContext);
            else if (randomStatContext1Validation == false && randomStatContext2Validation == true)
                return ItemExtraStat.DodgeBonus(dodgeChanceContext);
            else if (randomStatContext1Validation == true && randomStatContext2Validation == true)
                return ItemExtraStat.BothBonuses(healthContext, dodgeChanceContext);
            else return ItemExtraStat.NoExtraStat;
        }

        private static ItemData GetItemData(Item item, byte playerLevel)
        {
            ItemData returnValue = new ItemData();
            double rarityMultiplier = 0;
            double scaleMulti = playerLevel * (Globals.ScaleMultiBase - (Globals.Rnd.NextDouble() + 0.1)) / 1.789;
            double levelAsTenth = playerLevel * 0.01;
            lock (item)
            {
                switch (item.Rarity)
                {
                    case Rarity.Trash:
                        rarityMultiplier = 0.5;
                        returnValue.ItemValue = playerLevel / 10 > 1 ? playerLevel / 10 : 1.1;
                        break;
                    case Rarity.Common:
                        rarityMultiplier = 1.0;
                        returnValue.ItemValue = playerLevel / 2 > 2 ? playerLevel / 2 : 2.2;
                        break;
                    case Rarity.Uncommon:
                        rarityMultiplier = 1.09;
                        returnValue.ItemValue = playerLevel + Globals.Rnd.Next(1, 3);
                        break;
                    case Rarity.Refined:
                        rarityMultiplier = 1.14;
                        returnValue.ItemValue = playerLevel + (playerLevel / 2) + Globals.Rnd.Next(2, 5);
                        break;
                    case Rarity.Rare:
                        rarityMultiplier = 1.28;
                        returnValue.ItemValue = (playerLevel * 2) + Globals.Rnd.Next(3, 6);
                        break;
                    case Rarity.Epic:
                        rarityMultiplier = 1.43;
                        returnValue.ItemValue = (playerLevel * 3) + Globals.Rnd.Next(4, 8);
                        break;
                    case Rarity.Legendary:
                        rarityMultiplier = 1.76;
                        returnValue.ItemValue = (playerLevel * 5) + Globals.Rnd.Next(7, 13);
                        break;
                    case Rarity.Fabled:
                        rarityMultiplier = 2.44;
                        returnValue.ItemValue = (playerLevel * 6) + Globals.Rnd.Next(10, 19);
                        break;
                    case Rarity.Mythical:
                        rarityMultiplier = 3.11;
                        returnValue.ItemValue = (playerLevel * 7) + Globals.Rnd.Next(15, 26);
                        break;
                    case Rarity.Extraordinary:
                        rarityMultiplier = 4.22;
                        returnValue.ItemValue = (playerLevel * 8) + Globals.Rnd.Next(21, 33);
                        break;
                    case Rarity.Developer:
                        rarityMultiplier = 12.66;
                        returnValue.ItemValue = (playerLevel * 10) + Globals.Rnd.Next(43, 65);
                        break;
                    case Rarity.None:
                        return null;
                    default:
                        return null;
                }
            }

            returnValue.ItemValue = RoundThenToInt(scaleMulti * rarityMultiplier);

            lock (item)
            {
                switch (item.Info)
                {
                    case ItemContextInfo.NormalArmor:
                        returnValue.Defense = RoundThenToInt(1 + (playerLevel / 2) * scaleMulti * rarityMultiplier);
                        break;
                    case ItemContextInfo.NormalWeapon:
                        returnValue.Damage = RoundThenToInt(playerLevel / 2 * scaleMulti * rarityMultiplier);
                        returnValue.CritChance = RoundThenToInt((scaleMulti * rarityMultiplier * 1.14) + 0.18);
                        returnValue.CritDamage = RoundThenToInt(scaleMulti * rarityMultiplier * 4.2);
                        break;
                    case ItemContextInfo.NormalCharm:
                        double toChoose = Globals.Rnd.NextDouble();
                        if (toChoose <= 0.251)
                            returnValue.Damage = RoundThenToInt(1.55 + (playerLevel / 4) * scaleMulti * rarityMultiplier);
                        else if (toChoose > 0.252 && toChoose <= 0.501)
                            returnValue.Defense = RoundThenToInt(0.76 + (playerLevel / 5) * scaleMulti * rarityMultiplier);
                        else if (toChoose > 0.502 && toChoose <= 0.751)
                            returnValue.CritChance = RoundThenToInt(0.63 + (playerLevel / 6) * scaleMulti * rarityMultiplier);
                        else if (toChoose > 0.752)
                            returnValue.CritDamage = RoundThenToInt(1.22 + (playerLevel / 4.5) * scaleMulti * rarityMultiplier);
                        break;
                }
            }
            return returnValue;
        }

        private static int RoundThenToInt(double value)
            => Convert.ToInt32(Math.Round(value));
    }
}
