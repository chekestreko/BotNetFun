using System;

namespace BotNetFun.Loot.MetaItem
{
    using BotNetFun.Data;
    using BotNetFun.Loot.Enums;

    // todo: names
    // todo - priority: low - dynamic itemefficientclass bonus --- probably only implement if trading is implementeds
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

        public Item (
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
            if (_st is null)
                ExtraStat = ItemExtraStat.NoExtraStat;
            EfficientClass = ec;
            if (data is null)
                Data = ItemData.IncompleteItemData;
        }

        // Constructor for quick creation of an item with itemdata using an item without itemdata
        // this is used for thread-safety, as using ItemData.GetItemData() is unsafe with multiple threads, if it was going to be used in the constructor the first time
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
        public static Item GenerateRandomItem(byte playerLevel, ItemEfficientClass efficientClass)
        {
            double levelAsTenth = playerLevel * 0.01;

            Rarity randomRarity = Globals.GetRandomEnum<Rarity>();
            if (randomRarity == Rarity.Developer || randomRarity == Rarity.None)
                return GenerateRandomItem(playerLevel, efficientClass);

            ItemContextInfo randomContextInfo = Globals.GetRandomEnum<ItemContextInfo>();
            if (randomContextInfo == ItemContextInfo.None)
                return GenerateRandomItem(playerLevel, efficientClass);

            ItemType randomType = Globals.GetRandomEnum<ItemType>();
            if (randomType == ItemType.None)
                return GenerateRandomItem(playerLevel, efficientClass);
            
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

            ItemBonusContext randomBonusContext = Globals.GetRandomEnum<ItemBonusContext>();
            if (randomBonusContext != ItemBonusContext.None && playerLevel < Globals.Rnd.Next(0, 10) + Globals.Rnd.Next(0, 13))
                randomBonusContext = ItemBonusContext.None;
            else if (randomBonusContext == ItemBonusContext.None && playerLevel > 50)
                randomBonusContext = Globals.GetRandomEnum<ItemBonusContext>();
                    
            if (playerLevel <= 84 && randomRarity == Rarity.Extraordinary)
                DecrementEnum(ref randomRarity);

            if (playerLevel <= 67 && randomRarity == Rarity.Mythical)
                DecrementEnum(ref randomRarity);

            if (playerLevel <= 54 && randomRarity == Rarity.Fabled)
                DecrementEnum(ref randomRarity);

            if (playerLevel <= 39 && randomRarity == Rarity.Legendary)
                DecrementEnum(ref randomRarity);

            if (playerLevel <= 26 && randomRarity == Rarity.Epic)
                DecrementEnum(ref randomRarity);

            if (playerLevel <= 17 && randomRarity == Rarity.Rare)
                DecrementEnum(ref randomRarity);

            if (playerLevel <= 12 && randomRarity == Rarity.Refined)
                DecrementEnum(ref randomRarity);

            if (playerLevel <= 7 && randomRarity == Rarity.Uncommon)
                DecrementEnum(ref randomRarity);

            if (playerLevel <= 3 && randomRarity == Rarity.Common)
                DecrementEnum(ref randomRarity);

            if (Globals.Rnd.NextDouble() - (levelAsTenth / 5) < levelAsTenth * 2 && randomRarity != Rarity.Extraordinary)
                IncrementEnum(ref randomRarity);
            
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
                ItemExtraStat.GetRandomItemExtraStat(playerLevel), // Item extra stat
                Globals.GetRandomEnum<ItemEfficientClass>(), // Item efficient class
                ItemData.IncompleteItemData
            );

            return new Item(toReturn, ItemData.GetItemData(toReturn, playerLevel, efficientClass));

        } 

        private static void DecrementEnum(ref Rarity rare)
            => rare = (Rarity)((byte)rare - 1);

        private static void IncrementEnum(ref Rarity rare)
            => rare = (Rarity)((byte)rare + 1);
    }
}
