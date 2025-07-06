using System.Text.Json.Serialization;

namespace HoltronBot.Twitch.Models.EventSubAPIMessages
{
    public class SendMessage()
    {
        [JsonPropertyName("broadcaster_id")]
        public string BroadcasterID { get; set; }
        [JsonPropertyName("sender_id")]
        public string SenderID { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("for_source_only")]
        public bool ForSourceOnly { get; set; } = true;
    }
}