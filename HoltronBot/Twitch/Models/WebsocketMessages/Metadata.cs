using System.Text.Json.Serialization;

namespace HoltronBot.Twitch.Models.WebsocketMessages
{
    public class Metadata
    {
        [JsonPropertyName("message_id")]
        public string MessageID { get; set; }

        [JsonPropertyName("message_type")]
        public string MessageType { get; set; }

        [JsonPropertyName("message_timestamp")]
        public string MessageTimestamp { get; set; }
        [JsonPropertyName("subscription_type")]
        public string SubscriptionType { get; set; }
        [JsonPropertyName("subscription_version")]
        public string SubscriptionVersion { get; set; }
    }
}