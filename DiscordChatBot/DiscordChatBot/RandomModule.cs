using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordChatBot
{
    public class RandomModule : ModuleBase<SocketCommandContext>
    {
        [Command("intellitype")]
        [Summary("Inside joke from work")]
        public async Task IntellitypeCommand()
        {
            await ReplyAsync("No need to bring up a touchy subject...");
        }

        [Command("ping")]
        [Summary("Replys with Pong!")]
        public async Task PingCommand()
        {
            await ReplyAsync("Pong!");
        }

        [Command("roll")]
        [Summary("Makes a dice roll using the xdx format")]
        public async Task RollCommand(string message)
        {
            await RollDice(message);
        }

        [Command("github")]
        [Summary("Links to this repository")]
        public async Task GithubCommand()
        {
            await ReplyAsync("https://github.com/Jasuutu/DiscordChatBot");
        }

        private async Task RollDice(string message)
        {
            var result = MessageParser.ParseDiceMessage(message, new[] { ' ', 'd' });

            await ReplyAsync($"The dice rolled are: {result.Item1}");
            await ReplyAsync($"The total result is {result.Item2}");
        }
    }
}