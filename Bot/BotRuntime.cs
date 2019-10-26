using System.IO;
using System.Threading.Tasks;

using Discord.Commands;
using Discord.Addons.Interactive;

namespace BotNetFun.Bot
{
    using BotNetFun.Data;
    // Later: using BotNetFun.Loot.MetaItem;

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
            await using JsonHandler json = new JsonHandler(SaveJson);
            double PlayerLevel = await json.GetData<int>("Level");
            return (PlayerLevel * PlayerLevel) + (PlayerLevel > 25 ? (PlayerLevel * 36.591) : (PlayerLevel * 18.2955));
        }

        protected async Task<bool> HasInitialized() =>
            (await File.ReadAllTextAsync(SaveJson)).Contains("Class");
        

        protected async Task StarterSavefileIntegrity()
        {
            if (!File.Exists(SaveJson))
            {
                await File.CreateText(SaveJson).DisposeAsync();
                await File.WriteAllTextAsync(SaveJson, "{}");
            }
            if (!File.Exists(SaveItemJson))
            {
                await File.CreateText(SaveItemJson).DisposeAsync();
                await File.WriteAllTextAsync(SaveItemJson, "{}");
            }
        }

        protected async Task PlayerUpdate()
        {
            await using JsonHandler json = new JsonHandler(SaveJson);
            double MaxHealthCheck = await json.GetData<double>("MaxHealth");
            if (await json.GetData<double>("Health") > MaxHealthCheck)
                await json.WriteEntry("Health", MaxHealthCheck);
        }
    }
}
