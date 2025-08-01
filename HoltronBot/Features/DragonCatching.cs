using System.Linq;
using HoltronBot.Twitch.Models.WebsocketMessages;

namespace HoltronBot.Features
{
    public class DragonCatching : IFeature
    {
        public void HandlePayload(Payload payload)
        {
            if (payload.Event.Message.Text == "!catch")
            {
                // Handle adding the player to the catch list
            }

            var commandParts = payload.Event.Message.Text.Split(' ');
            if (commandParts.Length > 1)
            {
                
            }
        }

        public void Update()
        {
            
        }
    }
}