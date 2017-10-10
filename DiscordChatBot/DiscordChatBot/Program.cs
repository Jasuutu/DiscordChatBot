using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace DiscordChatBot
{
    class Program
    {
        private YouTubeService youtubeService;

        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var client = new DiscordSocketClient();
            client.Log += Log;
            client.MessageReceived += MessageReceived;

            var token = File.ReadAllText(Directory.GetCurrentDirectory() + "\\token.txt"); // Remember to keep this private!
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = File.ReadAllText(Directory.GetCurrentDirectory() + "\\youtubekey.txt"),
                ApplicationName = GetType().ToString()
            });

            Console.WriteLine(this.youtubeService.ApiKey);

            // Block this task until the program is closed.
            await Task.Delay(-1);
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

            else if (MessageParser.MessageStatsWith(message.Content, "!github"))
            {
                await message.Channel.SendMessageAsync("If you wish to see the source or propose a new idea for me," +
                                                       "go to https://github.com/Jasuutu/DiscordChatBot");
            }

            else if (MessageParser.MessageStatsWith(message.Content, "!youtube"))
            {
                await YoutubeSearch(message);
            }
        }

        private async Task YoutubeSearch(IMessage message)
        {
            SearchResource.ListRequest searchlistRequest = youtubeService.Search.List("snippet");
            searchlistRequest.Q = MessageParser.RemoveTriggerWord(message.Content); //Search term
            searchlistRequest.MaxResults = 5;

            SearchListResponse searchListResponse = await searchlistRequest.ExecuteAsync();

            List<string> videos = (from response 
                                   in searchListResponse.Items
                                   where response.Id.Kind == "youtube#video"
                                   select $"{response.Id.VideoId}").ToList();

            await message.Channel.SendMessageAsync($"https://www.youtube.com/watch?v={videos.FirstOrDefault()}");
        }

        private async Task RollDice(IMessage message)
        {
            Tuple<string, int> result = MessageParser.ParseDiceMessage(message.Content, new[] {' ', 'd'});

            await message.Channel.SendMessageAsync($"The dice rolled are: {result.Item1}");
            await message.Channel.SendMessageAsync($"The total result is {result.Item2}");
        }
    }
}
