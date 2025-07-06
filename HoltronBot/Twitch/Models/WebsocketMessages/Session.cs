using System.Text.Json.Serialization;

namespace HoltronBot.Twitch.Models.WebsocketMessages
{
    public class Session
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("connected_at")]
        public string ConnectedAt { get; set; }
        [JsonPropertyName("keepalive_timeout_seconds")]
        public int? KeepaliveTimeoutSeconds { get; set; }
        [JsonPropertyName("reconnect_url")]
        public string ReconnectURL { get; set; }
        [JsonPropertyName("recovery_url")]
        public string RecoveryURL { get; set; }
    }
}