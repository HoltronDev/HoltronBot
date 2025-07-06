using System.Collections.Generic;
using HoltronBot.Twitch.Models.WebsocketMessages;

namespace HoltronBot.Features
{
    public interface IFeature
    {
        public void HandlePayload(Payload payload);
        public void Update();
    }
}