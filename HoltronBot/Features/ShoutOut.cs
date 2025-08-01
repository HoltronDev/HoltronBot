using HoltronBot.Twitch;
using HoltronBot.Twitch.Models.WebsocketMessages;

namespace HoltronBot.Features
{
    public class ShoutOut(Game1 game, TwitchAPI twitchAPI) : IFeature
    {
        private readonly Game1 game = game;
        private readonly TwitchAPI twitchAPI = twitchAPI;

        public void HandlePayload(Payload payload)
        {
            if (payload.Event.Message.Text.StartsWith("!so"))
            {
                var parts = payload.Event.Message.Text.Split(' ');
                if (parts.Length < 2)
                {
                    return;
                }

                var userId = parts[1];
                if (userId.StartsWith('@'))
                {
                    userId = userId[1..];
                }

                var userData = twitchAPI.GetUserData(userId.ToLower());
                if (userData == null)
                {
                    return;
                }

                twitchAPI.SendShoutOut(userData.ID);
                game.DisplayText($"Oh look, a streamer! Heya @{userData.DisplayName}");
            }
        }

        public void Update()
        {
            // Negative
        }
    }
}