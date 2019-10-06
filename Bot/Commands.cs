using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;

using SnowynxHelpers.Extensions;

namespace BotNetFun.Bot
{
    using BotNetFun.Data;
    using BotNetFun.Enemy;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Handled by Discord.NET to parse commands encapsulated as a method via attribute")]
    public sealed partial class InternalWorkings : InteractiveBase
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
        [Summary("Start your adventure! (and with a starting class!)")]
        [Priority(-1)]
        private async Task Start()
        {
            await StarterSavefileIntegrity();
            if (await HasInitialized())
            {
                await ReplyAsync($"{Context.User.Username}, you already initialized. Use the `.help` command if you need help.");
                return;
            }
            await ReplyAsync($"Hello {Context.User.Username}! Please choose a starter class. The available starter classes are `Barbarian`, `Ninja`, `Rogue`, and `Knight`. The `Barbarian` is tanky, " +
                $"deals great upfront damage, but lacks in critical expertise. The `Ninja` has a good balance of offense, making it perfect if you want to deal good criticals" +
                $"while still packing a punch with regular attacks. The `Rogue` lacks in health and base damage, but makes up for its devastating " +
                $"criticals. Last but not least, there's the `Knight` which is similar to the `Barbarian`, but mitigates more damage rather than taking it upfront.");
            TimeSpan timer = new TimeSpan(40000000000);
            SocketMessage classQuestion = await NextMessageAsync(timeout: timer);
            if (classQuestion == null)
            {
                await ReplyAsync("You didn't reply in time, please try again. (`.start`)");
                return;
            }
            if (!classQuestion.Content.RemoveWhitespace().Equals("barbarian", StringComparison.InvariantCultureIgnoreCase) &&
                    !classQuestion.Content.RemoveWhitespace().Equals("ninja", StringComparison.InvariantCultureIgnoreCase) &&
                    !classQuestion.Content.RemoveWhitespace().Equals("rogue", StringComparison.InvariantCultureIgnoreCase) &&
                    !classQuestion.Content.RemoveWhitespace().Equals("knight", StringComparison.InvariantCultureIgnoreCase)) {
                await ReplyAsync("Unknown class, please try again. (Remember to check spelling, also use the `.start` command to try again)");
                return;
            }
            string starterclassstr = classQuestion.Content.RemoveWhitespace();
            await JsonHandler.WriteEntry("InBattle", "false", SaveJson);
            await JsonHandler.WriteEntry("Helmet", string.Empty, SaveJson);
            await JsonHandler.WriteEntry("Chestplate", string.Empty, SaveJson);
            await JsonHandler.WriteEntry("Gauntlet", string.Empty, SaveJson);
            await JsonHandler.WriteEntry("Pants", string.Empty, SaveJson);
            await JsonHandler.WriteEntry("Boots", string.Empty, SaveJson);
            await JsonHandler.WriteEntry("Primary", string.Empty, SaveJson);
            await JsonHandler.WriteEntry("Secondary", string.Empty, SaveJson);
            await JsonHandler.WriteEntry("Charm", string.Empty, SaveJson);
            await JsonHandler.WriteEntry("Gold", 5, SaveJson);
            await JsonHandler.WriteEntry("Level", 1, SaveJson);
            await JsonHandler.WriteEntry("XP", 0, SaveJson);
            switch (starterclassstr)
            {
                case "Barbarian":
                case "barbarian":
                    await ReplyAsync($"You picked the `Barbarian` class! Please say `confirm` to confirm.");
                    SocketMessage confirm1 = await NextMessageAsync(timeout: timer);
                    if (confirm1.Content.RemoveWhitespace().Contains("confirm", StringComparison.InvariantCultureIgnoreCase))
                    {
                        await JsonHandler.WriteEntry("Class", "Barbarian", SaveJson);
                        await ReplyAsync("`Barbarian` class confirmed. Welcome {Context.User.Username}! Use the `.help` command to get a list of commands!");
                    }
                    else await ReplyAsync("Error... either you didn't confirm, or you didn't reply in time. Please try again. (`.start`)");
                    await JsonHandler.WriteEntry("MaxHealth", 20, SaveJson);
                    await JsonHandler.WriteEntry("Health", 20, SaveJson);
                    await JsonHandler.WriteEntry("DodgeChance", 2, SaveJson);
                    await JsonHandler.WriteEntry("Defense", 4, SaveJson);
                    await JsonHandler.WriteEntry("BaseDamage", 5, SaveJson);
                    await JsonHandler.WriteEntry("BaseCriticalChance", 6, SaveJson);
                    await JsonHandler.WriteEntry("BaseCriticalDamage", 20, SaveJson);
                    break;
                case "Ninja":
                case "ninja":
                    await ReplyAsync($"You picked the `Ninja` class! Please say `confirm` to confirm.");
                    SocketMessage confirm2 = await NextMessageAsync(timeout: timer);
                    if (confirm2.Content.RemoveWhitespace().Contains("confirm", StringComparison.InvariantCultureIgnoreCase))
                    {
                        await JsonHandler.WriteEntry("Class", "Ninja", SaveJson);
                        await ReplyAsync($"`Ninja` class confirmed. Welcome {Context.User.Username}! Use the `.help` command to get a list of commands!");
                    }
                    else await ReplyAsync("Error... either you didn't confirm, or you didn't reply in time. Please try again. (`.start`)");
                    await JsonHandler.WriteEntry("MaxHealth", 9, SaveJson);
                    await JsonHandler.WriteEntry("Health", 9, SaveJson);
                    await JsonHandler.WriteEntry("DodgeChance", 6, SaveJson);
                    await JsonHandler.WriteEntry("Defense", 2, SaveJson);
                    await JsonHandler.WriteEntry("BaseDamage", 3, SaveJson);
                    await JsonHandler.WriteEntry("BaseCriticalChance", 12, SaveJson);
                    await JsonHandler.WriteEntry("BaseCriticalDamage", 45, SaveJson);
                    break;
                case "Rogue":
                case "rogue":
                    await ReplyAsync($"You picked the `Rogue` class! Please say `confirm` to confirm.");
                    SocketMessage confirm3 = await NextMessageAsync(timeout: timer);
                    if (confirm3.Content.RemoveWhitespace().Contains("confirm", StringComparison.InvariantCultureIgnoreCase))
                    {
                        await JsonHandler.WriteEntry("Class", "Rogue", SaveJson);
                        await ReplyAsync("`Rogue` class confirmed. Welcome {Context.User.Username}! Use the `.help` command to get a list of commands!");
                    }
                    else await ReplyAsync("Error... either you didn't confirm, or you didn't reply in time. Please try again. (`.start`)");
                    await JsonHandler.WriteEntry("MaxHealth", 6, SaveJson);
                    await JsonHandler.WriteEntry("Health", 6, SaveJson);
                    await JsonHandler.WriteEntry("DodgeChance", 8, SaveJson);
                    await JsonHandler.WriteEntry("Defense", 1, SaveJson);
                    await JsonHandler.WriteEntry("BaseDamage", 2, SaveJson);
                    await JsonHandler.WriteEntry("BaseCriticalChance", 18, SaveJson);
                    await JsonHandler.WriteEntry("BaseCriticalDamage", 60, SaveJson);
                    break;
                case "Knight":
                case "knight":
                    await ReplyAsync($"You picked the `Knight` class! Please say `confirm` to confirm.");
                    SocketMessage confirm4 = await NextMessageAsync(timeout: timer);
                    if (confirm4.Content.RemoveWhitespace().Contains("confirm", StringComparison.InvariantCultureIgnoreCase))
                    {
                        await JsonHandler.WriteEntry("Class", "Knight", SaveJson);
                        await ReplyAsync("`Knight` class confirmed. Welcome {Context.User.Username}! Use the `.help` command to get a list of commands!");
                    }
                    else await ReplyAsync("Error... either you didn't confirm, or you didn't reply in time. Please try again. (`.start`)");
                    await JsonHandler.WriteEntry("MaxHealth", 15, SaveJson);
                    await JsonHandler.WriteEntry("Health", 15, SaveJson);
                    await JsonHandler.WriteEntry("DodgeChance", 4, SaveJson);
                    await JsonHandler.WriteEntry("Defense", 6, SaveJson);
                    await JsonHandler.WriteEntry("BaseDamage", 4, SaveJson);
                    await JsonHandler.WriteEntry("BaseCriticalChance", 8, SaveJson);
                    await JsonHandler.WriteEntry("BaseCriticalDamage", 30, SaveJson);
                    break;
            }
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
            Dictionary<string, BaseEnemy> enemyCollection = Collections.Enemies;
            BaseEnemy encounteredEnemy = GetRandomFromDictionary(enemyCollection); 
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
            string alreadyIterated = string.Empty;

            foreach (CommandInfo command in commands)
            {
                if ("forceexit" == command.Name.RemoveWhitespace()) continue;
                if ("exit" == command.Name.RemoveWhitespace()) continue;
                if (alreadyIterated == command.Name.RemoveWhitespace()) continue;
                alreadyIterated = command.Name.RemoveWhitespace();
                string embedFieldText = command.Summary ?? "No description available" + Constants.NL;
                embedBuilder.AddField("❯ " + command.Name, embedFieldText);
            }

            await ReplyAsync("List of commands: ", embed: embedBuilder.Build());
        }
    }
}
