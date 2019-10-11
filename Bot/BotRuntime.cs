using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Discord.Addons.Interactive;

namespace BotNetFun.Bot
{
    using BotNetFun.Data;
    using BotNetFun.Loot.MetaItem;
    
    // Base bot runtime class
    internal abstract class BotRuntime : InteractiveBase
    {
        protected string SaveJson
        {
            get => $"{Constants.SavePath + "/" + Context.User.Id}.json";
        }

        protected async Task<double> XPToLevelUp()
        {
            double PlayerLevel = double.Parse(await JsonHandler.GetData("XP", SaveJson));
            return (PlayerLevel * 12.7) + (PlayerLevel * PlayerLevel);
        }

        protected async Task<bool> HasInitialized()
        {
            string file = await File.ReadAllTextAsync(SaveJson);
            return file.Contains("Class");
        }

        protected async Task StarterSavefileIntegrity()
        {
            if (!File.Exists(SaveJson))
            {
                File.CreateText(SaveJson);
                await File.WriteAllTextAsync(SaveJson, "{}");
            }
        }

        protected async Task PlayerUpdate()
        {
            long MaxHealthCheck = long.Parse(await JsonHandler.GetData("MaxHealth", SaveJson));
            if (long.Parse(await JsonHandler.GetData("Health", SaveJson)) > MaxHealthCheck)
                await JsonHandler.WriteEntry("Health", MaxHealthCheck, SaveJson);
        }

        protected T GetRandomFromDictionary<T>(Dictionary<string, T> dict) where T : class
        {
            List<T> CollectionList = new List<T>();
            foreach (KeyValuePair<string, T> vals in dict)
            {
                CollectionList.Add(vals.Value);
            }

            lock (CollectionList)
            {
                int rand = rnd.Next(CollectionList.Count);
                return CollectionList[rand] as T;
            }
        }

        protected string GetItemInfo(Item item)
        {
            string retVal = $@"Item type: {Collections.ParseItemInfo(item) + Constants.NL}";
            return retVal;
        }

        protected readonly Random rnd = new Random();
    }
}
