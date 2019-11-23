using System.IO;
using System.Threading.Tasks;

using Discord.Commands;
using Discord.Addons.Interactive;

using Newtonsoft.Json;

namespace BotNetFun.Bot
{
    using BotNetFun.Data;
    using BotNetFun.MainGame;
    using BotNetFun.MainGame.Enums;
    // Later: using BotNetFun.Loot.MetaItem;

    // Base bot runtime class
    public abstract class BotRuntime : InteractiveBase<ShardedCommandContext>
    {
        protected bool HasProvokedRecently { get; set; } = false;

        protected string SaveJson
            => $"{Globals.SavePath}/{Context.User.Id}.json";

        protected async Task<double> XPToLevelUp()
        {
            await using JsonHandler json = new JsonHandler(SaveJson);
            byte PlayerLevel = await json.GetData<byte>(PlayerData.Level);
            return (PlayerLevel * PlayerLevel) + (PlayerLevel > 25 ? (PlayerLevel * 36.591) : (PlayerLevel * 18.2955));
        }

        protected async Task<bool> HasInitialized() =>
            (await File.ReadAllTextAsync(SaveJson)).Contains("Class");

        protected async Task<bool> HasInitialized(string playerPath) =>
            (await File.ReadAllTextAsync(playerPath)).Contains("Class");

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0067:Dispose objects before losing scope", Justification = "Visual Studio doesn't recognize IAsyncDisposable/DisposeAsync as a valid object disposal interface/method")]
        protected async Task StarterSavefileIntegrity()
        {
            if (!File.Exists(SaveJson))
            {
                await File.CreateText(SaveJson).DisposeAsync();
                await File.WriteAllTextAsync(SaveJson, JsonConvert.SerializeObject(new Player()));
            }
        }

        protected async Task PlayerUpdate()
        {
            await using JsonHandler json = new JsonHandler(SaveJson);
            double MaxHealthCheck = await json.GetData<double>(PlayerData.MaxHealth);
            if (await json.GetData<double>(PlayerData.Health) > MaxHealthCheck)
                await json.WriteEntry(PlayerData.Health, MaxHealthCheck);
        }
    }
}
