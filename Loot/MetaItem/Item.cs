using System;

namespace BotNetFun.Loot.MetaItem
{
    using BotNetFun.Data;
    using BotNetFun.Loot.Enums;

    // todo: names
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
            if (data is null) Data = ItemData.IncompleteItemData;
        }

        // Constructor for quick creation of an item with itemdata using an item without itemdata
        public Item(Item createdFrom, ItemData data)
        {
            Name = createdFrom.Name;
            Info = createdFrom.Info;
            Rarity = createdFrom.Rarity;
            Type = createdFrom.Type;
            Bonus = createdFrom.Bonus;
            ExtraStat = createdFrom.ExtraStat;
            EfficientClass = createdFrom.EfficientClass;
            Data = data;
        }

        public static Item GetEmptyItem { get; } = new Item("None", ItemContextInfo.None, Rarity.None, ItemType.None);

        // todo: names
        public static Item GenerateRandomItem(byte playerLevel, bool efficientClass = false)
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
                return GenerateRandomItem(playerLevel);
            
            if ((randomType == ItemType.Helmet ||
                randomType == ItemType.Chestplate ||
                randomType == ItemType.Gauntlets ||
                randomType == ItemType.Pants ||
                randomType == ItemType.Boots) &&
                (randomContextInfo != ItemContextInfo.NormalArmor ||
                randomContextInfo != ItemContextInfo.OffensiveArmor ||
                randomContextInfo != ItemContextInfo.TankyArmor))
            {
                if (randomContextInfo == ItemContextInfo.NormalCharm || randomContextInfo == ItemContextInfo.NormalWeapon)
                    randomContextInfo = ItemContextInfo.NormalArmor;
                else if (randomContextInfo == ItemContextInfo.StrongWeapon || randomContextInfo == ItemContextInfo.DamagingCharm)
                    randomContextInfo = ItemContextInfo.OffensiveArmor;
                else if (randomContextInfo == ItemContextInfo.ParryingWeapon || randomContextInfo == ItemContextInfo.ShieldingCharm)
                    randomContextInfo = ItemContextInfo.TankyArmor;
            } else if ((randomType == ItemType.Primary || randomType == ItemType.Secondary) &&
                (randomContextInfo != ItemContextInfo.NormalWeapon ||
                randomContextInfo != ItemContextInfo.ParryingWeapon ||
                randomContextInfo != ItemContextInfo.StrongWeapon))
            {
                if (randomContextInfo == ItemContextInfo.NormalArmor || randomContextInfo == ItemContextInfo.NormalCharm)
                    randomContextInfo = ItemContextInfo.NormalWeapon;
                else if (randomContextInfo == ItemContextInfo.OffensiveArmor || randomContextInfo == ItemContextInfo.DamagingCharm)
                    randomContextInfo = ItemContextInfo.StrongWeapon;
                else if (randomContextInfo == ItemContextInfo.TankyArmor || randomContextInfo == ItemContextInfo.ShieldingCharm)
                    randomContextInfo = ItemContextInfo.ParryingWeapon;
            } else if ((randomType == ItemType.Charm) &&
                (randomContextInfo != ItemContextInfo.NormalCharm ||
                randomContextInfo != ItemContextInfo.DamagingCharm ||
                randomContextInfo != ItemContextInfo.ShieldingCharm))
            {
                if (randomContextInfo == ItemContextInfo.NormalArmor || randomContextInfo == ItemContextInfo.NormalWeapon)
                    randomContextInfo = ItemContextInfo.NormalCharm;
                else if (randomContextInfo == ItemContextInfo.OffensiveArmor || randomContextInfo == ItemContextInfo.StrongWeapon)
                    randomContextInfo = ItemContextInfo.DamagingCharm;
                else if (randomContextInfo == ItemContextInfo.TankyArmor || randomContextInfo == ItemContextInfo.ParryingWeapon)
                    randomContextInfo = ItemContextInfo.ShieldingCharm;
            }

            ItemBonusContext randomBonusContext = GetRandomEnum<ItemBonusContext>();
            if (randomBonusContext != ItemBonusContext.None && playerLevel < Globals.Rnd.Next(0, 10) + Globals.Rnd.Next(0, 13))
                randomBonusContext = ItemBonusContext.None;
            else if (randomBonusContext == ItemBonusContext.None && playerLevel > 50)
                randomBonusContext = GetRandomEnum<ItemBonusContext>();
                    
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
            
            if ((randomContextInfo != ItemContextInfo.NormalArmor ||
                 randomContextInfo != ItemContextInfo.NormalCharm ||
                 randomContextInfo != ItemContextInfo.NormalWeapon) && playerLevel < Globals.Rnd.Next(0, 8) + Globals.Rnd.Next(0, 11))
            {
                if (randomType == ItemType.Helmet ||
                    randomType == ItemType.Chestplate ||
                    randomType == ItemType.Gauntlets ||
                    randomType == ItemType.Pants ||
                    randomType == ItemType.Boots)
                    randomContextInfo = ItemContextInfo.NormalArmor;
                else if (randomType == ItemType.Primary || randomType == ItemType.Secondary)
                    randomContextInfo = ItemContextInfo.NormalWeapon;
                else
                    randomContextInfo = ItemContextInfo.NormalCharm;
            }

            Item toReturn = new Item (
                // Implement random name list
                "placeholdername",
                randomContextInfo,
                randomRarity,
                randomType,
                randomBonusContext,
                GetRandomItemExtraStat(playerLevel), // Item extra stat
                GetRandomEnum<ItemEfficientClass>(), // Item efficient class
                ItemData.IncompleteItemData
            );

            return new Item(toReturn, GetItemData(toReturn, playerLevel, efficientClass));

        } 

        private static T GetRandomEnum<T>() where T : Enum
            => (T)Enum.GetValues(typeof(T)).GetValue(Globals.Rnd.Next(Enum.GetValues(typeof(T)).Length));

        private static void DecrementRarity(ref Rarity rare)
            => rare = (Rarity)((byte)rare - 1);

        private static void IncrementRarity(ref Rarity rare)
            => rare = (Rarity)((byte)rare + 1);

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

        private static ItemData GetItemData(Item item, byte playerLevel, bool efficientClass = false)
        {
            ItemData returnValue = new ItemData();
            double rarityMultiplier = 0;
            double scaleMulti = playerLevel * (Globals.ScaleMultiBase - (Globals.Rnd.NextDouble() + 0.1)) / 1.789;

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

            returnValue.ItemValue += RoundThenToInt(scaleMulti * rarityMultiplier) / 8;

            lock (item)
            {
                switch (item.Info)
                {
                    case ItemContextInfo.NormalArmor:
                        returnValue.Defense = RoundThenToInt(Globals.Rnd.Next(0, playerLevel / 6) + playerLevel / 4 + (playerLevel / 2) * scaleMulti * rarityMultiplier);
                        returnValue.Damage = RoundThenToInt(playerLevel / 6) > 1 ? RoundThenToInt(playerLevel / 8) : 0;
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
                    case ItemContextInfo.TankyArmor:
                        returnValue.Defense = RoundThenToInt((Globals.Rnd.Next(0, playerLevel / 4) + playerLevel) * scaleMulti * rarityMultiplier) + Globals.Rnd.Next(0, playerLevel);
                        break;
                    case ItemContextInfo.OffensiveArmor:
                        returnValue.Defense = RoundThenToInt(playerLevel / 2 * scaleMulti * rarityMultiplier);
                        returnValue.CritChance = RoundThenToInt((scaleMulti * rarityMultiplier * 0.59) + 0.06);
                        returnValue.CritDamage = RoundThenToInt((scaleMulti * rarityMultiplier) / 3 * 1.45);
                        break;
                    case ItemContextInfo.ParryingWeapon:
                        returnValue.Damage = RoundThenToInt(playerLevel / 1.6 * scaleMulti * rarityMultiplier);
                        returnValue.CritChance = RoundThenToInt((scaleMulti * rarityMultiplier) + 0.12);
                        returnValue.CritDamage = RoundThenToInt(scaleMulti * rarityMultiplier * 3.3);
                        returnValue.Defense = RoundThenToInt(playerLevel / 3 * scaleMulti * rarityMultiplier);
                        break;
                    case ItemContextInfo.StrongWeapon:
                        returnValue.Damage = RoundThenToInt(Globals.Rnd.Next(0, playerLevel / 3) + playerLevel / 3.6 + playerLevel / 1.9 * scaleMulti * rarityMultiplier);
                        returnValue.CritChance = RoundThenToInt((scaleMulti * rarityMultiplier * 1.21) + 0.24);
                        returnValue.CritDamage = RoundThenToInt(scaleMulti * rarityMultiplier * 4.787);
                        break;
                    case ItemContextInfo.DamagingCharm:
                        returnValue.Damage = RoundThenToInt(1.989 + (playerLevel / 3.45) * scaleMulti * rarityMultiplier);
                        returnValue.CritChance = RoundThenToInt(0.69 + (playerLevel / 5.6789) * scaleMulti * rarityMultiplier);
                        returnValue.CritDamage = RoundThenToInt(1.556567 + (playerLevel / 4.2) * scaleMulti * rarityMultiplier);
                        break;
                    case ItemContextInfo.ShieldingCharm:
                        returnValue.Defense = RoundThenToInt(1.43278 + (playerLevel / 4.45678901) * scaleMulti * rarityMultiplier);
                        break;
                    case ItemContextInfo.None:
                        return null;
                    default:
                        return null; 
                }
            }

            if (efficientClass == true)
            {
                returnValue.CritChance += RoundThenToInt(returnValue.CritChance * Globals.ClassEfficiencyPercentBonus);
                returnValue.CritDamage += RoundThenToInt(returnValue.CritDamage * Globals.ClassEfficiencyPercentBonus);
                returnValue.Damage += RoundThenToInt(returnValue.Damage * Globals.ClassEfficiencyPercentBonus);
                returnValue.Defense += RoundThenToInt(returnValue.Defense * Globals.ClassEfficiencyPercentBonus);
                returnValue.ItemValue += returnValue.ItemValue * (Globals.ClassEfficiencyPercentBonus / 2);
            }

            return returnValue;
        }

        private static int RoundThenToInt(double value)
            => Convert.ToInt32(Math.Round(value));
    }
}
