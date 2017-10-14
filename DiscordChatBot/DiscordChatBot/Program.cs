using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordChatBot
{
    class Program
    {
        private CommandService commands;
        private DiscordSocketClient client;
        private IServiceProvider serivces;
        private YouTubeService youtubeService;

        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            client = new DiscordSocketClient();
            this.commands = new CommandService();

            BuildServices();

            this.serivces = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(commands)
                .AddSingleton(youtubeService)
                .BuildServiceProvider();

            await InstallCommandsAsync();

            var token = File.ReadAllText(Directory.GetCurrentDirectory() + "\\token.txt"); // Remember to keep this private!
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        public async Task InstallCommandsAsync()
        {
            client.Log += Log;
            client.MessageReceived += HandleCommandAsync;

            await this.commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            if (!(messageParam is SocketUserMessage message))
            {
                return;
            }
            var argPos = 0;
            if (!message.HasCharPrefix('!', ref argPos)) return;

            var context = new SocketCommandContext(this.client, message);
            IResult result = await this.commands.ExecuteAsync(context, argPos, this.serivces);
            if (!result.IsSuccess)
            {
                await context.Channel.SendMessageAsync(result.ErrorReason);
            }
        }

        private void BuildServices()
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = File.ReadAllText(Directory.GetCurrentDirectory() + "\\youtubekey.txt"),
                ApplicationName = this.GetType().ToString()
            });
            this.youtubeService = youtubeService;
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageReceived(IMessage message)
        {
            if (message.Author.IsBot)
                return;

            if (message.Content.Contains("intellitype"))
            {
                await message.Channel.SendMessageAsync($"No need to bring up a touchy subject...");
                return;
            }

            if (message.Content == "!ping")
            {
                await message.Channel.SendMessageAsync("Pong!");
            }

            else if (MessageParser.MessageStatsWith(message.Content, "!roll"))
            {
                await RollDice(message);
            }
        }

        private async Task RollDice(IMessage message)
        {
            Tuple<string, int> result = MessageParser.ParseDiceMessage(message.Content, new[] {' ', 'd'});

            await message.Channel.SendMessageAsync($"The dice rolled are: {result.Item1}");
            await message.Channel.SendMessageAsync($"The total result is {result.Item2}");
        }
    }
}
