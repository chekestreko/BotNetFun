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

        public DiscordSocketClient Client { get; set; }
        public CommandService CommandOperation { get; set; }
        public IServiceProvider Services { get; set; }
        public IConfigurationRoot Configuration { get; }

        public async Task RunBotClient()
        {
            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 2000,
                ShardId = 1,
                TotalShards = 2
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
                .BuildServiceProvider();

            Client.Log += arg =>
            {
                Console.WriteLine(arg.ToString());
                return Task.CompletedTask;
            };

            await RegisterCommandsAsync();
            await Client.LoginAsync(TokenType.Bot, @"NjI3OTkxODc3MDUyMDA2NDAw.XaKS_g.G7l4xX9EfrTANTcCU3ihbl5DJww");
            await Client.StartAsync();
            await Client.SetGameAsync("tutorials on how to grind gold easily", type: ActivityType.Watching);
            await Task.Delay(-1);
                    
        }

        private async Task RegisterCommandsAsync()
        {
            Client.MessageReceived += async arg => {
                SocketUserMessage message = (SocketUserMessage) arg;
                if (message is null || message.Author.IsBot) return;
                int argumentPos = 0;
                if (message.HasStringPrefix(@".", ref argumentPos) || message.HasMentionPrefix(Client.CurrentUser, ref argumentPos))
                {
                    SocketCommandContext context = new SocketCommandContext(Client, message);
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
