using System.Text.Json.Serialization;

namespace HoltronBot.Twitch.Models
{
    public class SendShoutOutMessage
    {
        [JsonPropertyName("from_broadcaster_id")]
        public string FromBroadcasterID { get; set; }
        [JsonPropertyName("to_broadcaster_id")]
        public string ToBroadcasterID { get; set; }
        [JsonPropertyName("moderator_id")]
        public string ModeratorID { get; set; }
    }
}