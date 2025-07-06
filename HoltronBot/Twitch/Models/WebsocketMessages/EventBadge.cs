using System.Text.Json.Serialization;

namespace HoltronBot.Twitch.Models.WebsocketMessages
{
    public class EventBadge
    {
        [JsonPropertyName("set_id")]
        public string SetId { get; set; }
        [JsonPropertyName("id")]
        public string ID { get; set; }
        [JsonPropertyName("info")]
        public string Info { get; set; }
    }
}