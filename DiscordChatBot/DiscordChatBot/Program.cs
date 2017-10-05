using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DiscordChatBot
{
    class Program
    {
        Random rng = new Random();

        static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var client = new DiscordSocketClient();

            client.Log += Log;
            client.MessageReceived += MessageReceived;

            var token = File.ReadAllText("C:/Dev/DiscordChatBot/DiscordChatBot/token.txt"); ; // Remember to keep this private!
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Author.IsBot)
                return;

            if (message.Author.Username == "Alriightyman")
            {
                int number = rng.Next(1, 6);
                if (number == 1)
                {
                    await message.Channel.SendMessageAsync("You got unlucky and rolled a 1.");
                    await message.Channel.SendMessageAsync("I do not respond to people who abuse bots...");
                    return;
                }
                else
                {
                    await message.Channel.SendMessageAsync($"You got luckey and rolled a {number}.");
                    await message.Channel.SendMessageAsync($"I'll listen to you this time.");
                }
            }

            if (message.Content.Contains("intellitype"))
            {
                await message.Channel.SendMessageAsync($"No need to bring up a touchy subject...");
                return;
            }

            if (message.Content == "!ping")
            {
                await message.Channel.SendMessageAsync("Pong!");
            }

            else if (message.Content.StartsWith("!roll"))
            {
                await RollDice(message);
            }
        }

        private async Task RollDice(SocketMessage message)
        {
            string[] splitMessage = message.Content.Split(new char[] { ' ', 'd' }, StringSplitOptions.RemoveEmptyEntries);

            int dice = int.Parse(splitMessage[1]);
            int value = int.Parse(splitMessage[2]);

            string resultNumbers = string.Empty;
            int result = 0;
            for (int i = 0; i < dice; i++)
            {
                int tempResult = rng.Next(1, value);

                if (i == dice - 1)
                {
                    resultNumbers += $"{tempResult}";
                }
                else
                {
                    resultNumbers += $"{tempResult} + ";
                }
                result += tempResult;
            }

            await message.Channel.SendMessageAsync($"The dice rolled are: {resultNumbers}");
            await message.Channel.SendMessageAsync($"The total result is {result}");
        }
    }
}
