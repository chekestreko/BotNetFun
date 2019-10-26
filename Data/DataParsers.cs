using System;

namespace BotNetFun.Data
{
    using BotNetFun.Loot.Enums;
    using BotNetFun.Loot.MetaItem;
    // later maybe? using BotNetFun.MetaEnemy;

    public static class DataParsers
    {
        public static string ParseItemInfo(Item item)
        {
            string retVal = string.Empty;
            retVal += $"Item name: {item.Name}" + Globals.NL;
            retVal += $"Item type: {ParseItemType(item)}" + Globals.NL;
            retVal += $"Item rarity: {ParseItemRarity(item)} (the better the rarity, the better the stats!)" + Globals.NL;
            retVal += $"Item stance: {ParseItemContextInfo(item)} (decides whether an item is more offensive or defensive)" + Globals.NL;
            retVal += $"Item class synergy: {ParseItemEfficientClass(item)} (if your class is the same as this item's class synergy, this item will have a 25% stat bonus!)" + Globals.NL;
            retVal += $"Item bonus: {ParseItemBonusContext(item)} (cool little extra effect from your item)" + Globals.NL;
            if (item.ExtraStat.BonusAffect == ItemExtraStatContext.MaxHealthBonus)
            {
                retVal += $"Item extra stat 1 (Max Health): {item.ExtraStat.BonusContent}" + Globals.NL;
                if (item.ExtraStat.ExtraBonusAffect == ItemExtraStatContext.DodgeChanceBonus)
                    retVal += $"Item extra stat 2 (Dodge Chance): {item.ExtraStat.ExtraBonusContent}" + Globals.NL;
                else
                    retVal += "Item extra stat 2: none" + Globals.NL;
            }
            else if (item.ExtraStat.BonusAffect == ItemExtraStatContext.DodgeChanceBonus)
            {
                retVal += $"Item extra stat 1 (Dodge Chance): {item.ExtraStat.BonusContent}" + Globals.NL;
                if (item.ExtraStat.ExtraBonusAffect == ItemExtraStatContext.MaxHealthBonus)
                    retVal += $"Item extra stat 2 (Max Health): {item.ExtraStat.ExtraBonusContent}" + Globals.NL;
                else
                    retVal += "Item extra stat 2: none" + Globals.NL;
            } else
                retVal += "Item extra stat 1: none" + Globals.NL + "Item extra stat 2: none" + Globals.NL;
            
            return retVal;
        }

        private static string ParseItemType(Item item) => item.Type switch
        {
            ItemType.None => "N/A",
            ItemType.Helmet => "Helmet",
            ItemType.Chestplate => "Chestplate",
            ItemType.Gauntlets => "Gauntlet",
            ItemType.Pants => "Pants",
            ItemType.Boots => "Boots",
            ItemType.Primary => "Primary Item",
            ItemType.Secondary => "Secondary Item",
            ItemType.Charm => "Charm",
            _ => throw new InvalidOperationException("Item has unknown type (?)"),
        };

        private static string ParseItemContextInfo(Item item) => item.Info switch
        {
            ItemContextInfo.None => "N/A",
            ItemContextInfo.NormalArmor => "Basic Armor",
            ItemContextInfo.NormalWeapon => "Basic Weapon",
            ItemContextInfo.NormalCharm => "Basic Charm",
            ItemContextInfo.OffensiveArmor => "Offensive-stanced Armor",
            ItemContextInfo.StrongWeapon => "Strong Weapon",
            ItemContextInfo.DamagingCharm => "Damage-based Charm",
            ItemContextInfo.TankyArmor => "Tanky Armor",
            ItemContextInfo.ParryingWeapon => "Parry-stanced Weapon",
            ItemContextInfo.ShieldingCharm => "Shielding Charm",
            _ => throw new InvalidOperationException("Item has unknown ContextInfo (?)")
        };

        private static string ParseItemRarity(Item item) => item.Rarity switch
        {
            Rarity.None => "N/A",
            Rarity.Trash => "Trash",
            Rarity.Common => "Common",
            Rarity.Uncommon => "Uncommon",
            Rarity.Refined => "Refined",
            Rarity.Rare => "Rare",
            Rarity.Epic => "Epic",
            Rarity.Legendary => "Legendary",
            Rarity.Fabled => "Fabled",
            Rarity.Mythical => "Mythical",
            Rarity.Extraordinary => "Extraordinary",
            Rarity.Developer => "Developer",
            _ => throw new InvalidOperationException("Item has unknown rarity (?)")
        };

        private static string ParseItemBonusContext(Item item) => item.Bonus switch
        {
            ItemBonusContext.None => "None",
            ItemBonusContext.Fire => "Fire",
            ItemBonusContext.Poison => "Poison",
            ItemBonusContext.Thorns => "Thorns",
            ItemBonusContext.StunChance => "Stun Chance",
            ItemBonusContext.GiantHunter => "Giant Hunter",
            ItemBonusContext.Bleeding => "Bleeding",
            ItemBonusContext.GhostCopy => "Ghost Copy",
            ItemBonusContext.Cripple => "Cripple",
            ItemBonusContext.Risktaker => "Risktaker",
            ItemBonusContext.Precision => "Precision",
            _ => throw new InvalidOperationException("Item has unknown BonusContext (?)")
        };

        private static string ParseItemEfficientClass(Item item) => item.EfficientClass switch
        {
            ItemEfficientClass.None => "None",
            ItemEfficientClass.Barbarian => "Barbarian",
            ItemEfficientClass.Knight => "Knight",
            ItemEfficientClass.Ninja => "Ninja",
            ItemEfficientClass.Rogue => "Rogue",
            _ => throw new InvalidOperationException("Item has unknown EfficientClass (?)")
        };
    }
}
