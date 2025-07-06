using System.Text.Json.Serialization;

namespace HoltronBot.Twitch.Models.WebsocketMessages
{
    public class Transport
    {
        [JsonPropertyName("method")]
        public string Method { get; set; }
        [JsonPropertyName("callback")]
        public string Callback { get; set; }
        [JsonPropertyName("secret")]
        public string Secret { get; set; }
        [JsonPropertyName("session_id")]
        public string SessionID { get; set; }
    }
}