using System.Text.Json.Serialization;

namespace HoltronBot.Twitch.Models.WebsocketMessages
{
    public class Condition
    {
        [JsonPropertyName("broadcaster_user_id")]
        public string BroadcasterUserId { get; set; }
        [JsonPropertyName("user_id")]
        public string UserID { get; set; }
        [JsonPropertyName("moderator_user_id")]
        public string ModeratorUserID { get; set; }
    }
}