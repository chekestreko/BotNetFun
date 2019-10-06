using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BotNetFun.Bot
{
    using BotNetFun.Data;

    internal sealed class DiscordBot
    {
        public static DiscordBot Bot { get; } = new DiscordBot();

        [MTAThread]
        private static async Task Main()
        {
            if (!Directory.Exists(Constants.SavePath))
                Directory.CreateDirectory(Constants.SavePath);
            await Bot.RunBot();
        }
        

        public IConfigurationRoot Configuration { get; }

        public DiscordBot()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Constants.SavePath);
            Configuration = builder.Build();
        }

        public DiscordSocketClient Client { get; set; }
        public CommandService CommandOperation { get; set; }
        public IServiceProvider Services { get; set; }

        public async Task RunBot()
        {
            Client = new DiscordSocketClient(new DiscordSocketConfig {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 2000,
                ShardId = 1,
                TotalShards = 1
            });
            CommandOperation = new CommandService(new CommandServiceConfig {
                LogLevel = LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async
            }); 

            Services = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(CommandOperation)
                .AddSingleton(Configuration)
                .BuildServiceProvider();
            
            Client.Log += arg => {
                Console.WriteLine(arg.ToString());
                return Task.CompletedTask;
            };
            
            await RegisterCommandsAsync(); 
            await Client.LoginAsync(TokenType.Bot, @"NjI3OTkxODc3MDUyMDA2NDAw.XZkvYg.eUSlE6jl6H_hE3qz-L2EJSrWQSY"); 
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
                        Console.WriteLine(result.ErrorReason);
                        await message.Channel.SendMessageAsync(result.ErrorReason);
                    }
                }
            };

            await CommandOperation.AddModulesAsync(Assembly.GetEntryAssembly(), Services); 
        }
    }
}
