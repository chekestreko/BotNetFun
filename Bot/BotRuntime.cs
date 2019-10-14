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
            => $"{Globals.SavePath}/{Context.User.Id}.json";

        protected string SaveItemJson
            => $"{Globals.SavePath}/{Context.User.Id}.Items.json";

        protected async Task<double> XPToLevelUp()
        {
            JsonHandler.Path = SaveJson;
            double PlayerLevel = await JsonHandler.GetData<int>("Level");
            return (PlayerLevel * PlayerLevel) + (PlayerLevel > 25 ? (PlayerLevel * 36.591) : (PlayerLevel * 18.2955));
        }

        protected async Task<bool> HasInitialized()
        {
            JsonHandler.Path = SaveJson;
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
            if (!File.Exists(SaveItemJson))
            {
                File.CreateText(SaveItemJson);
                await File.WriteAllTextAsync(SaveItemJson, "{}");
            }
        }

        protected async Task PlayerUpdate()
        {
            JsonHandler.Path = SaveJson;
            double MaxHealthCheck = await JsonHandler.GetData<double>("MaxHealth");
            if (await JsonHandler.GetData<double>("Health") > MaxHealthCheck)
                await JsonHandler.WriteEntry("Health", MaxHealthCheck);
        }
        /* <---- might be deprecated ---->
        protected T GetRandomFromDictionary<T>(Dictionary<string, T> dict) where T : class
        {
            JsonHandler.Path = SaveJson;
            List<T> CollectionList = new List<T>();

            foreach (KeyValuePair<string, T> vals in dict)
                CollectionList.Add(vals.Value);
            
            lock (CollectionList)
            {
                int rand = Globals.Rnd.Next(0, CollectionList.Count + 1);
                return CollectionList[rand] as T;
            }
        }
        */
        // todo: fix
        protected string GetItemInfo(Item item)
        {
            JsonHandler.Path = SaveJson;
            string retVal = $@"Item type: {Collections.ParseItemInfo(item) + Globals.NL}";
            return retVal;
        }
    }
}
