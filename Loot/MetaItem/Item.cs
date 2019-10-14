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
        public ItemEfficientClass EfficientClass { get; private set; }
        public bool EmptyItem { get; }

        public Item (
            string _name,
            ItemContextInfo info,
            Rarity rare = Rarity.Common,
            ItemType ty = ItemType.Primary,
            ItemBonus bo = null,
            ItemExtraStat _st = null,
            ItemSet _set = null,
            ItemEfficientClass ec = ItemEfficientClass.None,
            bool emptyItem = false
        )
        {
            Name = _name;
            Info = info;
            Rarity = rare;
            Type = ty;
            if (bo is null) bo = ItemBonus.NoBonus;
            if (_st is null) _st = ItemExtraStat.NoExtraStat;
            if (_set is null) _set = ItemSet.NoItemSet;
            Bonus = bo;
            ExtraStat = _st;
            Set = _set;
            EfficientClass = ec;
            EmptyItem = emptyItem;
        }

        public static Item CreateItem (
            string _name,
            ItemContextInfo info,
            Rarity rare = Rarity.Common,
            ItemType ty = ItemType.Primary,
            ItemBonus bo = null,
            ItemExtraStat _st = null,
            ItemSet _set = null,
            ItemEfficientClass ec = ItemEfficientClass.None
        ) => new Item(_name, info, rare, ty, bo, _st, _set, ec);

        public static Item GetEmptyItem()
            => new Item("None", ItemContextInfo.None, Rarity.None, ItemType.None, emptyItem: true);

        public static ItemData GetItemData(Item item, int playerLevel)
        {
            ItemData returnValue = new ItemData();
            double rarityMultiplier = 0;
            double scaleMulti = (playerLevel * (Globals.ScaleMultiBase - (Globals.Rnd.NextDouble() + 0.1))) / 1.789;

            lock (item)
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
            }

            returnValue.Default();
            returnValue.ItemValue = RoundThenToInt(scaleMulti * rarityMultiplier);

            //lock (item)
            switch (item.Info)
            {
                case ItemContextInfo.BasicArmor:
                    returnValue.Defense = RoundThenToInt(1 + (playerLevel / 2) * scaleMulti * rarityMultiplier);
                    break;
                case ItemContextInfo.BasicWeapon:
                    returnValue.Damage = RoundThenToInt(playerLevel / 2 * scaleMulti * rarityMultiplier);
                    returnValue.CritChance = RoundThenToInt((scaleMulti * rarityMultiplier * 1.14) + 0.18);
                    returnValue.CritDamage = RoundThenToInt((scaleMulti * rarityMultiplier * 4.2));
                    break;
                case ItemContextInfo.BasicCharm:
                    double toChoose = Globals.Rnd.NextDouble();
                    if (toChoose < 0.251)
                        returnValue.Damage = RoundThenToInt(1.55 + (playerLevel / 4) * scaleMulti * rarityMultiplier);
                    else if (toChoose > 0.252 && toChoose < 0.501)
                        returnValue.Defense = RoundThenToInt(0.76 + (playerLevel / 5) * scaleMulti * rarityMultiplier);
                    else if (toChoose > 0.502 && toChoose < 0.751)
                        returnValue.CritChance = RoundThenToInt(0.63 + (playerLevel / 6) * scaleMulti * rarityMultiplier);
                    else if (toChoose > 0.752)
                        returnValue.CritDamage = RoundThenToInt(1.22 + (playerLevel / 4.5) * scaleMulti * rarityMultiplier);
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

        private static int RoundThenToInt(double value)
            => Convert.ToInt32(Math.Round(value));
    }
}
