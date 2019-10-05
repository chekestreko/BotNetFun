using System;
using System.IO;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

namespace BotNetFun.Bot
{
    using BotNetFun.Data;

    public sealed class Commands : ModuleBase<SocketCommandContext>
    {
        #region Commands
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("forceexit")]
        [Alias("exit")]
        private async Task ForceExit()
        {
            await ReplyAsync("Exiting! (exit code: 0)");
            Environment.Exit(0);
        }

        [Command("start")]
        [Alias("initialize", "init")]
        [Summary("Start your adventure!")]
        private async Task Start()
        {
            if (!File.Exists(SaveJson))
            {
                File.CreateText(SaveJson);
                await File.WriteAllTextAsync(SaveJson, "{}");
            }
            if (await HasInitialized())
            {
                await ReplyAsync($"{Context.User.Username}, you already initialized. Use the **!help** command if you need help.");
                return;
            }
            await JsonHandler.WriteEntry("created", "true", SaveJson);
            await JsonHandler.WriteEntry("InBattle", "true", SaveJson);
            await JsonHandler.WriteEntry("MaxHealth", 10, SaveJson);
            await JsonHandler.WriteEntry("Health", 10, SaveJson);
            await JsonHandler.WriteEntry("Gold", 5, SaveJson);
            await JsonHandler.WriteEntry("DodgeChance", 5, SaveJson);
            await JsonHandler.WriteEntry("Defense", 5, SaveJson);
            await JsonHandler.WriteEntry("Level", 1, SaveJson);
            await JsonHandler.WriteEntry("XP", 0 , SaveJson);
            await JsonHandler.WriteEntry("BaseDamage", 2, SaveJson);
            await JsonHandler.WriteEntry("BaseCriticalChance", 5, SaveJson);
            await JsonHandler.WriteEntry("BaseCriticalDamage", 25, SaveJson);
            await JsonHandler.WriteEntry("Helmet", string.Empty, SaveJson);
            await JsonHandler.WriteEntry("Chestplate", string.Empty, SaveJson);
            await JsonHandler.WriteEntry("Gauntlet", string.Empty, SaveJson);
            await JsonHandler.WriteEntry("Pants", string.Empty, SaveJson);
            await JsonHandler.WriteEntry("Boots", string.Empty, SaveJson);
            await JsonHandler.WriteEntry("Primary", string.Empty, SaveJson);
            await JsonHandler.WriteEntry("Secondary", string.Empty, SaveJson);
            await JsonHandler.WriteEntry("Charm", string.Empty, SaveJson);
            await JsonHandler.WriteEntry("Aura", string.Empty, SaveJson);
            await JsonHandler.WriteEntry("Pet", string.Empty, SaveJson);
            await ReplyAsync($"Welcome {Context.User.Username}! Use the **!help** command to get a list of commands!");
        }

        [Command("help")]
        [Summary("Get a list of commands")]
        private async Task Help()
        {
            await ReplyAsync();
        }
        #endregion

        #region Helpers
        private string SaveJson
        {
            get => $"{Constants.SavePath + "/" + Context.User.Id}.json";
        }

        private async Task<double> XPToLevelUp()
        {
            double PlayerLevel = double.Parse(await JsonHandler.GetData("XP", SaveJson));
            return (PlayerLevel * 7.7) + (PlayerLevel * PlayerLevel);
        }

        private async Task<bool> HasInitialized()
        {
            string text = await File.ReadAllTextAsync(SaveJson);
            return text.Contains("created");
        }
          
        private async Task PlayerUpdate()
        {
            long MaxHealthCheck = long.Parse(await JsonHandler.GetData("MaxHealth", SaveJson));
            if (long.Parse(await JsonHandler.GetData("Health", SaveJson)) > MaxHealthCheck)
                await JsonHandler.WriteEntry("Health", MaxHealthCheck, SaveJson);
        }
        #endregion
    }
}
