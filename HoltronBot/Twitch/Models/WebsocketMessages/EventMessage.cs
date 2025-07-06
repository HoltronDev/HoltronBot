using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HoltronBot.Twitch.Models.WebsocketMessages
{
    public class EventMessage
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("fragments")]
        public List<EventMessageFragment> Fragments { get; set; }
    }
}