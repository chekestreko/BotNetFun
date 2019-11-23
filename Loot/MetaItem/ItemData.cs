namespace BotNetFun.Loot.MetaItem
{
    using BotNetFun.Data;
    using BotNetFun.Loot.Enums;

    [System.Serializable]
    public sealed class ItemData
    {
        public double Damage { get; set; } = 0;
        public double Defense { get; set; } = 0;
        public double CritChance { get; set; } = 0;
        public double CritDamage { get; set; } = 0;
        public double ItemValue { get; set; } = 0;

        public static ItemData IncompleteItemData { get; } = new ItemData
        {
            Damage = 0,
            Defense = 0,
            CritChance = 0,
            CritDamage = 0,
            ItemValue = 0
        };

        public static ItemData GetItemData(Item item, byte playerLevel, ItemEfficientClass efficientClass = ItemEfficientClass.None)
        {
            ItemData returnValue = new ItemData();

            double rarityMultiplier;
            double scaleMulti = playerLevel * (Globals.ScaleMultiBase - (Globals.Rnd.NextDouble() + 0.1)) / 1.789;

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

            returnValue.ItemValue += Globals.RoundSlightly(scaleMulti * rarityMultiplier) / 8;

            switch (item.Info)
            {
                case ItemContextInfo.NormalArmor:
                    returnValue.Defense = Globals.RoundSlightly(Globals.Rnd.Next(0, playerLevel / 6) + playerLevel / 4 + (playerLevel / 2) * scaleMulti * rarityMultiplier);
                    returnValue.Damage = Globals.RoundSlightly(playerLevel / 6) > 1 ? Globals.RoundSlightly(playerLevel / 8) : 0;
                    break;
                case ItemContextInfo.NormalWeapon:
                    returnValue.Damage = Globals.RoundSlightly(playerLevel / 2 * scaleMulti * rarityMultiplier);
                    returnValue.CritChance = Globals.RoundSlightly((scaleMulti * rarityMultiplier * 1.14) + 0.18);
                    returnValue.CritDamage = Globals.RoundSlightly(scaleMulti * rarityMultiplier * 4.2);
                    break;
                case ItemContextInfo.NormalCharm:
                    double toChoose = Globals.Rnd.NextDouble();
                    if (toChoose <= 0.251)
                        returnValue.Damage = Globals.RoundSlightly(1.55 + (playerLevel / 4) * scaleMulti * rarityMultiplier);
                    else if (toChoose > 0.252 && toChoose <= 0.501)
                        returnValue.Defense = Globals.RoundSlightly(0.76 + (playerLevel / 5) * scaleMulti * rarityMultiplier);
                    else if (toChoose > 0.502 && toChoose <= 0.751)
                        returnValue.CritChance = Globals.RoundSlightly(0.63 + (playerLevel / 6) * scaleMulti * rarityMultiplier);
                    else if (toChoose > 0.752)
                        returnValue.CritDamage = Globals.RoundSlightly(1.22 + (playerLevel / 4.5) * scaleMulti * rarityMultiplier);
                    break;
                case ItemContextInfo.TankyArmor:
                    returnValue.Defense = Globals.RoundSlightly((Globals.Rnd.Next(0, playerLevel / 4) + playerLevel) * scaleMulti * rarityMultiplier) + Globals.Rnd.Next(0, playerLevel);
                    break;
                case ItemContextInfo.OffensiveArmor:
                    returnValue.Defense = Globals.RoundSlightly(playerLevel / 2 * scaleMulti * rarityMultiplier);
                    returnValue.CritChance = Globals.RoundSlightly((scaleMulti * rarityMultiplier * 0.59) + 0.06);
                    returnValue.CritDamage = Globals.RoundSlightly((scaleMulti * rarityMultiplier) / 3 * 1.45);
                    break;
                case ItemContextInfo.ParryingWeapon:
                    returnValue.Damage = Globals.RoundSlightly(playerLevel / 1.6 * scaleMulti * rarityMultiplier);
                    returnValue.CritChance = Globals.RoundSlightly((scaleMulti * rarityMultiplier) + 0.12);
                    returnValue.CritDamage = Globals.RoundSlightly(scaleMulti * rarityMultiplier * 3.3);
                    returnValue.Defense = Globals.RoundSlightly(playerLevel / 3 * scaleMulti * rarityMultiplier);
                    break;
                case ItemContextInfo.StrongWeapon:
                    returnValue.Damage = Globals.RoundSlightly(Globals.Rnd.Next(0, playerLevel / 3) + playerLevel / 3.6 + playerLevel / 1.9 * scaleMulti * rarityMultiplier);
                    returnValue.CritChance = Globals.RoundSlightly((scaleMulti * rarityMultiplier * 1.21) + 0.24);
                    returnValue.CritDamage = Globals.RoundSlightly(scaleMulti * rarityMultiplier * 4.787);
                    break;
                case ItemContextInfo.DamagingCharm:
                    returnValue.Damage = Globals.RoundSlightly(1.989 + (playerLevel / 3.45) * scaleMulti * rarityMultiplier);
                    returnValue.CritChance = Globals.RoundSlightly(0.69 + (playerLevel / 5.6789) * scaleMulti * rarityMultiplier);
                    returnValue.CritDamage = Globals.RoundSlightly(1.556567 + (playerLevel / 4.2) * scaleMulti * rarityMultiplier);
                    break;
                case ItemContextInfo.ShieldingCharm:
                    returnValue.Defense = Globals.RoundSlightly(1.43278 + (playerLevel / 4.45678901) * scaleMulti * rarityMultiplier);
                    break;
                case ItemContextInfo.None:
                    return null;
                default:
                    return null;
            }


            if (efficientClass == item.EfficientClass)
            {
                returnValue.CritChance += Globals.RoundSlightly(returnValue.CritChance * Globals.ClassEfficiencyPercentBonus);
                returnValue.CritDamage += Globals.RoundSlightly(returnValue.CritDamage * Globals.ClassEfficiencyPercentBonus);
                returnValue.Damage += Globals.RoundSlightly(returnValue.Damage * Globals.ClassEfficiencyPercentBonus);
                returnValue.Defense += Globals.RoundSlightly(returnValue.Defense * Globals.ClassEfficiencyPercentBonus);
                returnValue.ItemValue += returnValue.ItemValue * (Globals.ClassEfficiencyPercentBonus / 2.4);
            }

            return returnValue;
        }
    }
}
