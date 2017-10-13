using System.IO;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace DiscordChatBot
{
    public class YoutubeServiceWrapper
    {
        public YouTubeService youtubeService { get; }

        public YoutubeServiceWrapper()
        {
            youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = File.ReadAllText(Directory.GetCurrentDirectory() + "\\youtubekey.txt"),
                ApplicationName = GetType().ToString()
            });
        }
    }
}