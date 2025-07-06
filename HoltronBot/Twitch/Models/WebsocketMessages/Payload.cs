using System.Text.Json.Serialization;

namespace HoltronBot.Twitch.Models.WebsocketMessages
{
    public class Payload
    {
        [JsonPropertyName("session")]
        public Session Session { get; set; }
        [JsonPropertyName("event")]
        public Event Event { get; set; }
        [JsonPropertyName("subscription")]
        public Subsciption Subsciption { get; set; }
    }
}