﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Discord.Commands;
using Discord.Addons.Interactive;

namespace BotNetFun.Bot
{
    using BotNetFun.Data;
    using BotNetFun.Loot.MetaItem;

    // Base bot runtime class
    public abstract class BotRuntime : InteractiveBase<ShardedCommandContext>
    {
        protected bool HasProvokedRecently { get; set; }

        protected string SaveJson
            => $"{Globals.SavePath + "/" + Context.User.Id}.json";

        protected async Task<double> XPToLevelUp()
        {
            JsonHandler.Path = SaveJson;
            double PlayerLevel = await JsonHandler.GetData<double>("XP");
            return (PlayerLevel * PlayerLevel) + (PlayerLevel * 36.591); 
        }

        protected async Task<bool> HasInitialized()
        {
            JsonHandler.Path = SaveJson;
            string file = await File.ReadAllTextAsync(SaveJson);
            return file.Contains("Class");
        }

        protected async Task StarterSavefileIntegrity()
        {
            JsonHandler.Path = SaveJson;
            if (!File.Exists(SaveJson))
            {
                File.CreateText(SaveJson);
                await File.WriteAllTextAsync(SaveJson, "{}");
            }
        }

        protected async Task PlayerUpdate()
        {
            JsonHandler.Path = SaveJson;
            double MaxHealthCheck = await JsonHandler.GetData<double>("MaxHealth");
            if (await JsonHandler.GetData<double>("Health") > MaxHealthCheck)
                await JsonHandler.WriteEntry("Health", MaxHealthCheck);
        }

        protected T GetRandomFromDictionary<T>(Dictionary<string, T> dict) where T : class
        {
            JsonHandler.Path = SaveJson;
            List<T> CollectionList = new List<T>();

            foreach (KeyValuePair<string, T> vals in dict)
                CollectionList.Add(vals.Value);
            
            lock (CollectionList)
            {
                int rand = rnd.Next(0, CollectionList.Count);
                return CollectionList[rand] as T;
            }
        }

        // todo: fix
        protected string GetItemInfo(Item item)
        {
            JsonHandler.Path = SaveJson;
            string retVal = $@"Item type: {Collections.ParseItemInfo(item) + Globals.NL}";
            return retVal;
        }

        protected readonly Random rnd = new Random();
    }
}
