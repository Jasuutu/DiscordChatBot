using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace DiscordChatBot
{
    public class YouTubeBase : ModuleBase<SocketCommandContext>
    {
        [Command("youtube")]
        [Summary("Finds a single video based on the search term")]
        public async Task YoutubeVideoSearch()
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
    }
}