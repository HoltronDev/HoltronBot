using HoltronBot.Twitch;
using HoltronBot.Twitch.Models.WebsocketMessages;
using Serilog;

namespace HoltronBot.Features
{
    public class Clippinator(Game1 game, TwitchAPI twitchAPI) : IFeature
    {
        private readonly Game1 game = game;
        private readonly TwitchAPI twitchAPI = twitchAPI;

        public void HandlePayload(Payload payload)
        {
            if (payload.Event.Message.Text == "!clip")
            {
                var url = twitchAPI.CreateClip();
                if (!string.IsNullOrEmpty(url))
                {
                    twitchAPI.SendMessage($"Clip created, you can view it here! {url}");
                    game.DisplayText($"Clip created, you can view it here! {url}");
                }
                else
                {
                    Log.Warning("Failed to create clip.");
                }
            }
        }

        public void Update()
        {
            // Niet
        }
    }
}