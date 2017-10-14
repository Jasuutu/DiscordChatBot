using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace DiscordChatBot
{
    public class YouTubeModule : ModuleBase<SocketCommandContext>
    {
        private readonly YouTubeService youTubeService;

        public YouTubeModule(YouTubeService service)
        {
            youTubeService = service;
        }

        [Command("youtube")]
        [Summary("Finds a single video based on the search term")]
        public async Task YoutubeVideoSearch([Remainder] [Summary("String to search YouTube on")] string message)
        {
            SearchResource.ListRequest searchlistRequest = youTubeService.Search.List("snippet");
            searchlistRequest.Q = MessageParser.RemoveTriggerWord(message); //Search term
            searchlistRequest.MaxResults = 5;

            SearchListResponse searchListResponse = await searchlistRequest.ExecuteAsync();

            List<string> videos = (from response
                    in searchListResponse.Items
                where response.Id.Kind == "youtube#video"
                select $"{response.Id.VideoId}").ToList();

            await ReplyAsync($"https://www.youtube.com/watch?v={videos.FirstOrDefault()}");
        }
    }
}