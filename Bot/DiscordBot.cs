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
        [MTAThread]
        private static async Task Main()
        {
            if (!Directory.Exists(Constants.SavePath))
                Directory.CreateDirectory(Constants.SavePath);
            DiscordBot bot = new DiscordBot();
            await bot.RunBot();
        }

        public IConfigurationRoot Configuration { get; }

        public DiscordBot()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Constants.SavePath);
            Configuration = builder.Build();
        }

        private DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider services;

        public async Task RunBot()
        {
            client = new DiscordSocketClient(new DiscordSocketConfig {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 2000,
                ShardId = 1,
                TotalShards = 1
            });
            commands = new CommandService(new CommandServiceConfig {
                LogLevel = LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async
            }); 

            services = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(commands)
                .AddSingleton(Configuration)
                .BuildServiceProvider();
            
            client.Log += arg => {
                Console.WriteLine(arg.ToString());
                return Task.CompletedTask;
            };
            
            await RegisterCommandsAsync(); 
            await client.LoginAsync(TokenType.Bot, @"NjI3OTkxODc3MDUyMDA2NDAw.XZFPiQ.vFRPe1zVr_TTevTfu3Fhx-dxXPA"); 
            await client.StartAsync(); 
            await client.SetGameAsync("tutorials on how to grind gold easily", type: ActivityType.Watching);
            await Task.Delay(-1);
        }

        private async Task RegisterCommandsAsync()
        {
            client.MessageReceived += async arg => {
                SocketUserMessage message = (SocketUserMessage) arg;
                if (message is null || message.Author.IsBot) return;
                int argumentPos = 0;
                if (message.HasStringPrefix(@".", ref argumentPos) || message.HasMentionPrefix(client.CurrentUser, ref argumentPos))
                {
                    SocketCommandContext context = new SocketCommandContext(client, message);
                    IResult result = await commands.ExecuteAsync(context, argumentPos, services);
                    if (!result.IsSuccess)
                    {
                        Console.WriteLine(result.ErrorReason);
                        await message.Channel.SendMessageAsync(result.ErrorReason);
                        Environment.Exit(1);
                    }
                }
            };

            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services); 
        }
    }
}
