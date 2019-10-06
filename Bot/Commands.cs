using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using SnowynxHelpers.Extensions;

namespace BotNetFun.Bot
{
    using BotNetFun.Data;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Handled by Discord.NET to parse commands encapsulated as a method via attribute")]
    public sealed partial class InternalWorkings : ModuleBase<SocketCommandContext>
    {
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
            await StarterSavefileIntegrity();
            if (await HasInitialized())
            {
                await ReplyAsync($"{Context.User.Username}, you already initialized. Use the `.help` command if you need help.");
                return;
            }
            await JsonHandler.WriteEntry("InBattle", "false", SaveJson);
            await JsonHandler.WriteEntry("MaxHealth", 10, SaveJson);
            await JsonHandler.WriteEntry("Health", 10, SaveJson);
            await JsonHandler.WriteEntry("Gold", 5, SaveJson);
            await JsonHandler.WriteEntry("DodgeChance", 5, SaveJson);
            await JsonHandler.WriteEntry("Defense", 5, SaveJson);
            await JsonHandler.WriteEntry("Level", 1, SaveJson);
            await JsonHandler.WriteEntry("XP", 0, SaveJson);
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
            await ReplyAsync($"Welcome {Context.User.Username}! Use the `.help` command to get a list of commands!");
        }

        [Command("encounter")]
        [Alias("fight")]
        [Summary("Fight a random enemy...")]
        private async Task Encounter()
        {
            await StarterSavefileIntegrity();
            if (!File.Exists(SaveJson))
            {
                File.CreateText(SaveJson);
                await File.WriteAllTextAsync(SaveJson, "{}");
            }
            if (!await HasInitialized())
            {
                await ReplyAsync("Please use the `.start` command first to initialize.");
                return;
            }
            await JsonHandler.WriteEntry("InBattle", "true", SaveJson);
            Embed encounterMessage = new EmbedBuilder
            {
                Title = "Enemy Encounter",
                Description = "Finding enemy to fight...",
                Color = Color.Gold
            }.Build();
            IUserMessage message = await ReplyAsync(embed: encounterMessage);
            /*
            Dictionary<string, BaseEnemy> enemyCollection = Collections.Enemies;
            BaseEnemy encounteredEnemy = GetRandomFromDictionary(enemyCollection); */
            await Task.Delay(300);
            await message.ModifyAsync(msg => msg.Embed = new EmbedBuilder {
                Title = "Enemy found!",
                Description = "Get ready!",
                Color = Color.Red
            }.Build());
            await JsonHandler.WriteEntry("InBattle", "false", SaveJson);
        }

        [Command("help")]
        [Summary("Get a list of commands")]
        private async Task Help()
        { 
            List<CommandInfo> commands = DiscordBot.Bot.CommandOperation.Commands.ToList();
            EmbedBuilder embedBuilder = new EmbedBuilder {
                Color = Color.Blue
            }; 

            foreach (CommandInfo command in commands)
            {
                if ("forceexit" == command.Name.RemoveWhitespace()) continue;
                if ("exit" == command.Name.RemoveWhitespace()) continue;

                string embedFieldText = command.Summary ?? "No description available" + Constants.NL;

                embedBuilder.AddField(command.Name, embedFieldText);
            }

            await ReplyAsync("List of commands: ", embed: embedBuilder.Build());
        }
    }
}
