using System.Text.Json.Serialization;

namespace HoltronBot.Twitch.Models
{
    public class GetUserDataResponse
    {
        [JsonPropertyName("data")]
        public UserData[] Data { get; set; }
    }

    public class UserData()
    {
        [JsonPropertyName("broadcaster_type")]
        public string BroadcasterType { get; set; }
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }
        [JsonPropertyName("id")]
        public string ID { get; set; }
        [JsonPropertyName("login")]
        public string Login { get; set; }
        [JsonPropertyName("offline_image_url")]
        public string OfflineImageURL { get; set; }
        [JsonPropertyName("profile_image_url")]
        public string ProfileImageURL { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("view_count")]
        public long ViewCount { get; set; }
    }
}