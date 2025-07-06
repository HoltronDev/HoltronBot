using System.Text.Json.Serialization;

namespace HoltronBot.Twitch.Models.WebsocketMessages
{
    public class Mention
    {
        [JsonPropertyName("user_id")]
        public string UserID { get; set; }
        [JsonPropertyName("user_login")]
        public string UserLogin { get; set; }
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }
    }
}