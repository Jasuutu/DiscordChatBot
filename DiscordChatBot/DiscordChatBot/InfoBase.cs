using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordChatBot
{
    public class InfoBase : ModuleBase<SocketCommandContext>
    {
        [Command("github")]
        [Summary("Echos a message.")]
        public async Task GithubInfo()
        {
            await ReplyAsync("If you wish to see the source or propose a new idea for me," +
                             "go to https://github.com/Jasuutu/DiscordChatBot");
        }


    }
}