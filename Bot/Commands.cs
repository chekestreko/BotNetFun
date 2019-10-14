using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using SnowynxHelpers.Extensions;

namespace BotNetFun.Bot
{
    using BotNetFun.Data;
    using BotNetFun.Loot.MetaItem;
    using BotNetFun.MetaEnemy;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Handled by Discord.NET to parse commands encapsulated as an async method via Command attribute")]
    // Commands
    public sealed class Commands : BotRuntime
    {
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("forceexit")]
        [Alias("exit")]
        private async Task ForceExit()
        {
            await ReplyAsync("Exiting! (exit code: 0)");
            Environment.Exit(0);
        }

        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("injectdependency")]
        private async Task EjectDependency(string dep)
        {
            await ReplyAsync($"Dependency injected! (eject code: 0){Globals.NL}Dependency hashcode: {dep.GetHashCode()}");
        }

        [Command("start")]
        [Alias("initialize", "init")]
        [Summary("Start your adventure! (and with a starting class!)")]
        private async Task Start()
        {
            await StarterSavefileIntegrity();
            JsonHandler.Path = SaveJson;
            JsonHandler.ItemPath = SaveItemJson;
            if (await HasInitialized())
            {
                await ReplyAsync($"{Context.User.Username}, you already initialized. Use the `.help` command if you need help.");
                return;
            }
            if (HasProvokedRecently == true)
            {
                HasProvokedRecently = false;
                return;
            }
            EmbedBuilder classEmbedInfo = new EmbedBuilder {
                Title = "Initalization",
                Description = "Starting your adventure",
                Color = Color.Blue
            };
            classEmbedInfo.AddField("Class selection ", $"Hello {Context.User.Username}! Please choose a starter class by responding with any of the following starter class keywords: ");
            classEmbedInfo.AddField("Available starter classes: ", "`Barbarian`, `Ninja`, `Rogue`, and `Knight`");
            classEmbedInfo.AddField("Barbarian ", "Tanky, deals great upfront damage, but lacks in critical expertise");
            classEmbedInfo.AddField("Ninja ", "Good balance of offense, making it perfect if you want to deal good criticals while still packing a punch with regular attacks");
            classEmbedInfo.AddField("Rogue ", "Lacks in health and base damage, but makes up for its devastating criticals");
            classEmbedInfo.AddField("Knight ", "Similar to the `Barbarian`, but mitigates more damage rather than taking it upfront");
            await ReplyAsync(embed: classEmbedInfo.Build());
            TimeSpan timer = new TimeSpan(0, 1, 20);
            SocketMessage classQuestion = await NextMessageAsync(timeout: timer);
            if (classQuestion == null)
            {
                await ReplyAsync("You didn't reply in time, please try again. (`.start`)");
                return;
            }
            else if (!classQuestion.Content.RemoveWhitespace().Equals("barbarian", StringComparison.OrdinalIgnoreCase) &&
                !classQuestion.Content.RemoveWhitespace().Equals("ninja", StringComparison.OrdinalIgnoreCase) &&
                !classQuestion.Content.RemoveWhitespace().Equals("rogue", StringComparison.OrdinalIgnoreCase) &&
                !classQuestion.Content.RemoveWhitespace().Equals("knight", StringComparison.OrdinalIgnoreCase))
            {
                if (classQuestion.Content.RemoveWhitespace().StartsWith('.'))
                {
                    await ReplyAsync("No commands allowed --- you were currently being awaited for input, please try again. (`.start`, input context: choosing class)");
                    HasProvokedRecently = true;
                    return;
                }
                await ReplyAsync("Unknown class, please try again. (`.start`, remember to check spelling)");
                return;
            }
            else if (classQuestion.Content.RemoveWhitespace().StartsWith('.'))
            {
                await ReplyAsync("No commands allowed --- you were currently being awaited for input, please try again. (`.start`, input context: choosing class)");
                HasProvokedRecently = true;
                return;
            }
            string starterclassstr = classQuestion.Content.RemoveWhitespace().ToLower();
            Item emptyItem = Item.GetEmptyItem;
            await JsonHandler.WriteItemEntry("Helmet", emptyItem);
            await JsonHandler.WriteItemEntry("Chestplate", emptyItem);
            await JsonHandler.WriteItemEntry("Gauntlet", emptyItem);
            await JsonHandler.WriteItemEntry("Pants", emptyItem);
            await JsonHandler.WriteItemEntry("Boots", emptyItem);
            await JsonHandler.WriteItemEntry("Primary", emptyItem);
            await JsonHandler.WriteItemEntry("Secondary", emptyItem);
            await JsonHandler.WriteItemEntry("Charm", emptyItem);
            await JsonHandler.WriteItemEntry("I1", emptyItem);
            await JsonHandler.WriteItemEntry("I2", emptyItem);
            await JsonHandler.WriteItemEntry("I3", emptyItem);
            await JsonHandler.WriteItemEntry("I4", emptyItem);
            await JsonHandler.WriteItemEntry("I5", emptyItem);
            await JsonHandler.WriteItemEntry("I6", emptyItem);
            await JsonHandler.WriteItemEntry("I7", emptyItem);
            await JsonHandler.WriteItemEntry("I8", emptyItem);
            await JsonHandler.WriteItemEntry("I9", emptyItem);
            await JsonHandler.WriteItemEntry("I10", emptyItem);
            await JsonHandler.WriteItemEntry("I11", emptyItem);
            await JsonHandler.WriteItemEntry("I12", emptyItem);
            await JsonHandler.WriteItemEntry("I13", emptyItem);
            await JsonHandler.WriteItemEntry("I14", emptyItem);
            await JsonHandler.WriteItemEntry("I15", emptyItem);
            await JsonHandler.WriteItemEntry("I16", emptyItem);
            await JsonHandler.WriteItemEntry("I17", emptyItem);
            await JsonHandler.WriteItemEntry("I18", emptyItem);
            await JsonHandler.WriteItemEntry("I19", emptyItem);
            await JsonHandler.WriteItemEntry("I20", emptyItem);

            await JsonHandler.WriteEntry("InBattle", false);
            await JsonHandler.WriteEntry("Gold", 5);
            await JsonHandler.WriteEntry("Level", 1);
            await JsonHandler.WriteEntry("XP", 0);
            switch (starterclassstr)
            {
                case "barbarian":
                    await ReplyAsync("You picked the `Barbarian` class! Please say `confirm` to confirm.");
                    SocketMessage confirm1 = await NextMessageAsync(timeout: timer);
                    if (confirm1.Content.RemoveWhitespace().Contains("confirm", StringComparison.OrdinalIgnoreCase))
                    {
                        await JsonHandler.WriteEntry("Class", "Barbarian");
                        await ReplyAsync($"`Barbarian` class confirmed. Welcome {Context.User.Username}! Use the `.help` command to get a list of commands!");
                    }
                    else if (confirm1.Content.RemoveWhitespace().StartsWith('.'))
                    {
                        await ReplyAsync("No commands allowed --- you were currently being awaited for input, please try again. (`.start`, input context: choosing class)");
                        HasProvokedRecently = true;
                        return;
                    }
                    else if (confirm1 == null)
                    {
                        await ReplyAsync("You didn't reply in time, please try again. (`.start`)");
                        return;
                    }
                    else
                    {
                        await ReplyAsync("Input doesn't match `confirm`, class selection canceled (use the `.start` command to try again)");
                        return;
                    }
                    await JsonHandler.WriteEntry("MaxHealth", 20);
                    await JsonHandler.WriteEntry("Health", 20);
                    await JsonHandler.WriteEntry("DodgeChance", 2);
                    await JsonHandler.WriteEntry("Defense", 4);
                    await JsonHandler.WriteEntry("BaseDamage", 5);
                    await JsonHandler.WriteEntry("BaseCriticalChance", 6);
                    await JsonHandler.WriteEntry("BaseCriticalDamage", 20);
                    break;
                case "ninja":
                    await ReplyAsync("You picked the `Ninja` class! Please say `confirm` to confirm.");
                    SocketMessage confirm2 = await NextMessageAsync(timeout: timer); 
                    if (confirm2.Content.RemoveWhitespace().Contains("confirm", StringComparison.OrdinalIgnoreCase))
                    {
                        await JsonHandler.WriteEntry("Class", "Ninja");
                        await ReplyAsync($"`Ninja` class confirmed. Welcome {Context.User.Username}! Use the `.help` command to get a list of commands!");
                    }
                    else if (confirm2.Content.RemoveWhitespace().StartsWith('.'))
                    {
                        await ReplyAsync("No commands allowed --- you were currently being awaited for input, please try again. (`.start`, input context: choosing class)");
                        HasProvokedRecently = true;
                        return;
                    }
                    else if (confirm2 == null)
                    {
                        await ReplyAsync("You didn't reply in time, please try again. (`.start`)");
                        return;
                    }
                    else
                    {
                        await ReplyAsync("Input doesn't match `confirm`, class selection canceled (use the `.start` command to try again)");
                        return;
                    }
                    await JsonHandler.WriteEntry("MaxHealth", 9);
                    await JsonHandler.WriteEntry("Health", 9);
                    await JsonHandler.WriteEntry("DodgeChance", 6);
                    await JsonHandler.WriteEntry("Defense", 2);
                    await JsonHandler.WriteEntry("BaseDamage", 3);
                    await JsonHandler.WriteEntry("BaseCriticalChance", 12);
                    await JsonHandler.WriteEntry("BaseCriticalDamage", 45);
                    break;
                case "rogue":
                    await ReplyAsync("You picked the `Rogue` class! Please say `confirm` to confirm.");
                    SocketMessage confirm3 = await NextMessageAsync(timeout: timer);
                    if (confirm3.Content.RemoveWhitespace().Contains("confirm", StringComparison.OrdinalIgnoreCase))
                    {
                        await JsonHandler.WriteEntry("Class", "Rogue");
                        await ReplyAsync($"`Rogue` class confirmed. Welcome {Context.User.Username}! Use the `.help` command to get a list of commands!");
                    }
                    else if (confirm3.Content.RemoveWhitespace().StartsWith('.'))
                    {
                        await ReplyAsync("No commands allowed --- you were currently being awaited for input, please try again. (`.start`, input context: choosing class)");
                        HasProvokedRecently = true;
                        return;
                    }
                    else if (confirm3 == null)
                    {
                        await ReplyAsync("You didn't reply in time, please try again. (`.start`)");
                        return;
                    }
                    else
                    {
                        await ReplyAsync("Input doesn't match `confirm`, class selection canceled (use the `.start` command to try again)");
                        return;
                    }
                    await JsonHandler.WriteEntry("MaxHealth", 6);
                    await JsonHandler.WriteEntry("Health", 6);
                    await JsonHandler.WriteEntry("DodgeChance", 8);
                    await JsonHandler.WriteEntry("Defense", 1);
                    await JsonHandler.WriteEntry("BaseDamage", 2);
                    await JsonHandler.WriteEntry("BaseCriticalChance", 18);
                    await JsonHandler.WriteEntry("BaseCriticalDamage", 60);
                    break;
                case "knight":
                    await ReplyAsync("You picked the `Knight` class! Please say `confirm` to confirm.");
                    SocketMessage confirm4 = await NextMessageAsync(timeout: timer);
                    if (confirm4.Content.RemoveWhitespace().Contains("confirm", StringComparison.OrdinalIgnoreCase))
                    {
                        await JsonHandler.WriteEntry("Class", "Knight");
                        await ReplyAsync($"`Knight` class confirmed. Welcome {Context.User.Username}! Use the `.help` command to get a list of commands!");
                    }
                    else if (confirm4.Content.RemoveWhitespace().StartsWith('.'))
                    {
                        await ReplyAsync("No commands allowed --- you were currently being awaited for input, please try again. (`.start`, input context: choosing class)");
                        HasProvokedRecently = true;
                        return;
                    }
                    else if (confirm4 == null)
                    {
                        await ReplyAsync("You didn't reply in time, please try again. (`.start`)");
                        return;
                    }
                    else
                    {
                        await ReplyAsync("Input doesn't match `confirm`, class selection canceled (use the `.start` command to try again)");
                        return;
                    }

                    await JsonHandler.WriteEntry("MaxHealth", 15);
                    await JsonHandler.WriteEntry("Health", 15);
                    await JsonHandler.WriteEntry("DodgeChance", 4);
                    await JsonHandler.WriteEntry("Defense", 6);
                    await JsonHandler.WriteEntry("BaseDamage", 4);
                    await JsonHandler.WriteEntry("BaseCriticalChance", 8);
                    await JsonHandler.WriteEntry("BaseCriticalDamage", 30);
                    break;
            }
        }

        [Command("givegold")]
        [Summary("Give gold to another player")]
        private async Task GiveGold(IUser toGiveTo, double goldToGive)
        {
            JsonHandler.Path = SaveJson;
            JsonHandler.ItemPath = SaveItemJson;
            double currentGold = await JsonHandler.GetData<double>("Gold");
            string toGiveToPath = $"{AppDomain.CurrentDomain.BaseDirectory}/SaveData/{toGiveTo.Id}.json";
            if (!File.Exists(toGiveToPath))
            {
                await ReplyAsync($"Error: specified user ({toGiveTo.Username}) has not initialized yet");
            }
            string fileValidity = await File.ReadAllTextAsync(toGiveToPath);
            if (!fileValidity.Contains("Class"))
            {
                await ReplyAsync($"Error: specified user ({toGiveTo.Username}) has not initialized yet");
                return;
            } else if (currentGold < goldToGive)
            {
                await ReplyAsync($"Error: you don't have enough gold ({currentGold}), compared to the amount you want to give ({goldToGive})");
                return;
            } else if (toGiveTo == null)
            {
                await ReplyAsync($"Error: specified user doesn't seem to exist");
                return;
            }
            await ReplyAsync($"Giving {goldToGive} gold to {toGiveTo.Username}, please say `confirm` to confirm.");
            SocketMessage confirm;
            confirm = await NextMessageAsync(timeout: new TimeSpan(0, 0, 20));
            if (confirm.Content.RemoveWhitespace().Contains("confirm", StringComparison.OrdinalIgnoreCase))
            {
                await JsonHandler.WriteEntry("Gold", currentGold - goldToGive);
                JsonHandler.Path = toGiveToPath;
                await JsonHandler.WriteEntry("Gold", await JsonHandler.GetData<double>("Gold") + goldToGive);
                JsonHandler.Path = SaveJson;
                await ReplyAsync($"Successfully gave {goldToGive} gold to {toGiveTo.Username}!");
                return;
            }
            else if (confirm.Content.RemoveWhitespace().StartsWith('.'))
            {
                await ReplyAsync("No commands allowed --- you were currently being awaited for input, please try again. (`.givegold`, input context: giving gold)");
                return;
            }
            else if (confirm == null)
            {
                await ReplyAsync("You didn't reply in time, please try again. (`.givegold`)");
                return;
            }
            else
            {
                await ReplyAsync("Input doesn't match `confirm`, gold giving canceled (use the `.givegold` command again to try again if that was a mistake)");
                return;
            }
        }

        // TODO: actual encounter
        [Command("encounter")]
        [Alias("fight")]
        [Summary("Fight a random enemy...")]
        private async Task Encounter()
        {
            JsonHandler.Path = SaveJson;
            JsonHandler.ItemPath = SaveItemJson;
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
            await JsonHandler.WriteEntry("InBattle", "true");
            EmbedBuilder encounterMessage = new EmbedBuilder
            {
                Title = "Enemy Encounter",
                Description = "Finding enemy to fight...",
                Color = Color.Gold
            };
            IUserMessage message = await ReplyAsync(embed: encounterMessage.Build());

            //Dictionary<string, Enemy> enemyCollection = Collections.Enemies;
            //Enemy encounteredEnemy = GetRandomFromDictionary<Enemy>(enemyCollection); 
            
            encounterMessage.EditEmbed("Enemy found!", "Get ready!", Color.DarkRed);

            await Task.Delay(Globals.Rnd.Next(350, 850));
            await message.ModifyAsync(msg => msg.Embed = encounterMessage.Build());
            await Task.Delay(Globals.Rnd.Next(350, 450));
            await JsonHandler.WriteEntry("InBattle", "false");
        }

        [Command("help")]
        [Summary("Get a list of commands")]
        private async Task Help()
        {
            JsonHandler.Path = SaveJson;
            List<CommandInfo> commands = DiscordBot.Bot.CommandOperation.Commands.ToList();
            EmbedBuilder embedBuilder = new EmbedBuilder { Color = Color.Blue };

            foreach (CommandInfo command in commands)
            {
                if ("forceexit" == command.Name.RemoveWhitespace()) continue;
                if ("exit" == command.Name.RemoveWhitespace()) continue;
                if (command.Summary is null) continue;
                string embedFieldText = command.Summary;
                embedBuilder.AddField("❯ " + command.Name, embedFieldText);
            }

            await ReplyAsync("List of commands: ", embed: embedBuilder.Build());
        }

    }
}
