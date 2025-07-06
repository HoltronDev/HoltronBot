using System.Text.Json.Serialization;
using HoltronBot.Twitch.Models.WebsocketMessages;

namespace HoltronBot.Twitch.Models.EventSubAPIMessages
{
    public class Data
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("version")]
        public string Version { get; set; }
        [JsonPropertyName("condition")]
        public Condition Condition { get; set; }
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }
        [JsonPropertyName("transport")]
        public Transport Transport { get; set; }
        [JsonPropertyName("cost")]
        public int Cost { get; set; }
    }
}