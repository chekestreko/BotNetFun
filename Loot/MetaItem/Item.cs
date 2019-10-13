using System;

namespace BotNetFun.Loot.MetaItem
{
    using BotNetFun.Data;
    using BotNetFun.Loot.Enums;

    public sealed class Item
    {
        public string Name { get; private set; }
        public ItemContextInfo Info { get; private set; }
        public Rarity Rarity { get; private set; }
        public ItemType Type { get; private set; }
        public ItemBonus Bonus { get; private set; }
        public ItemExtraStat ExtraStat { get; private set; }
        public ItemSet Set { get; private set; }
        public Item(
            string _name,
            ItemContextInfo info,
            Rarity rare = Rarity.Common,
            ItemType ty = ItemType.Primary,
            ItemBonus bo = null,
            ItemExtraStat _st = null,
            ItemSet _set = null
        )
        {
            Name = _name;
            Info = info;
            Rarity = rare;
            if (_st is null) _st = new ItemExtraStat(ItemBonusType.NoItemBonus);
            if (_set is null) _set = null;
            Bonus = bo;
            Type = ty;
            ExtraStat = _st;
            Set = _set;
        }

        public static Item CreateItem (
            string _name,
            ItemContextInfo info,
            Rarity rare = Rarity.Common,
            ItemType ty = ItemType.Primary,
            ItemBonus bo = null,
            ItemExtraStat _st = null,
            ItemSet _set = null
        ) => new Item(_name, info, rare, ty, bo, _st, _set);

        public static ItemData GetItemData (Item item, int playerLevel)
        {
            ItemData returnValue = new ItemData();
            double rarityMultiplier = 0;
            double scaleMulti = playerLevel * Globals.ScaleMultiBase;
            switch (item.Rarity)
            {
                case Rarity.Trash:
                    rarityMultiplier = 0.5;
                    returnValue.ItemValue = 0;
                    break;
                case Rarity.Common:
                    rarityMultiplier = 1.0; 
                    break;
                case Rarity.Uncommon:
                    rarityMultiplier = 1.09;
                    break;
                case Rarity.Refined:
                    rarityMultiplier = 1.14;
                    break;
                case Rarity.Rare:
                    rarityMultiplier = 1.28;
                    break;
                case Rarity.Epic:
                    rarityMultiplier = 1.43;
                    break;
                case Rarity.Legendary:
                    rarityMultiplier = 1.76;
                    break;
                case Rarity.Fabled:
                    rarityMultiplier = 2.44;
                    break;
                case Rarity.Mythical:
                    rarityMultiplier = 3.11;
                    break;
                case Rarity.Extraordinary:
                    rarityMultiplier = 4.22;
                    break;
                case Rarity.Developer:
                    rarityMultiplier = 12.66;
                    break;
            }

            returnValue.Default();
            returnValue.ItemValue = Convert.ToInt32(Math.Round(scaleMulti * rarityMultiplier)); 

            switch (item.Info)
            {
                case ItemContextInfo.BasicArmor:
                    break;
                case ItemContextInfo.BasicWeapon:
                    break;
                case ItemContextInfo.BasicCharm:
                    break;
                case ItemContextInfo.DefensiveArmor:
                    break;
                case ItemContextInfo.ParryingWeapon:
                    break;
                case ItemContextInfo.DefensiveCharm:
                    break;
                case ItemContextInfo.OffensiveArmor:
                    break;
                case ItemContextInfo.StrongWeapon:
                    break;
                case ItemContextInfo.OffensiveCharm:
                    break;
                case ItemContextInfo.BalancedArmor:
                    break;
                case ItemContextInfo.BalancedWeapon:
                    break;
                case ItemContextInfo.BalancedCharm:
                    break;
                case ItemContextInfo.UniqueRare:
                    break;
                case ItemContextInfo.QuestRare:
                    break;
                case ItemContextInfo.SetRare:
                    break;
                case ItemContextInfo.HighTierRare:
                    break;
                case ItemContextInfo.EndgameRare:
                    break;
                case ItemContextInfo.SetEndgameRare:
                    break;
                case ItemContextInfo.Developer:
                    break;
            }
            return returnValue;
        }
    }
}
