using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BotNetFun.Bot
{
    using BotNetFun.Data;

    public sealed class DiscordBot
    {
        public static DiscordBot Bot { get; } = new DiscordBot();

        [MTAThread]
        private static async Task Main()
        {
            if (!Directory.Exists(Globals.SavePath))
                Directory.CreateDirectory(Globals.SavePath);
            await Bot.RunBotClient();
        }
        
        public DiscordBot()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .Build();
        }

        public DiscordShardedClient Client { get; set; }
        public CommandService CommandOperation { get; set; }
        public ServiceProvider Services { get; set; }
        public IConfigurationRoot Configuration { get; }

        public async Task RunBotClient()
        {
            Client = new DiscordShardedClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 2000,
                TotalShards = 3
            });

            CommandOperation = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async
            });
            
            Services = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(CommandOperation)
                .AddSingleton(Configuration)
                .AddSingleton<InteractiveService>()
                .AddSingleton<Random>()
                .BuildServiceProvider();

            Client.ShardReady += arg =>
            {
                Console.WriteLine($"Shard {arg.ShardId} is connected and ready!");
                return Task.CompletedTask;
            };

            Client.Log += arg =>
            {
                Console.WriteLine(arg.ToString());
                return Task.CompletedTask;
            };

            await RegisterCommandsAsync();
            await Client.LoginAsync(TokenType.Bot, @"NjI3OTkxODc3MDUyMDA2NDAw.XaTLYg.5oscD5pCzXDjqu_YjLPcAAIb7tc");
            await Client.StartAsync();
            await Client.SetGameAsync("tutorials on how to grind gold easily", type: ActivityType.Watching);
            await Task.Delay(System.Threading.Timeout.Infinite);
        }

        private async Task RegisterCommandsAsync()
        {
            Client.MessageReceived += async arg => {
                SocketUserMessage message = (SocketUserMessage) arg;
                if (message is null || message.Author.IsBot) return;
                int argumentPos = 0;
                if (message.HasStringPrefix(@".", ref argumentPos) || message.HasMentionPrefix(Client.CurrentUser, ref argumentPos))
                {
                    ShardedCommandContext context = new ShardedCommandContext(Client, message);
                    IResult result = await CommandOperation.ExecuteAsync(context, argumentPos, Services);
                    if (!result.IsSuccess)
                    {
                        if (result.Error == CommandError.UnknownCommand)
                            return;
                        
                        Console.WriteLine(result.ErrorReason);
                        await message.Channel.SendMessageAsync(result.ErrorReason);
                    }
                }
            };

            await CommandOperation.AddModulesAsync(Assembly.GetEntryAssembly(), Services); 
        }
    }
}
