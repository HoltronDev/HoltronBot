using System.Text.Json.Serialization;

namespace HoltronBot.Twitch.Models
{
    public class CreateClipResponse
    {
        [JsonPropertyName("data")]
        public CreateClipData ClipData { get; set; }
    }

    public class CreateClipData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("edit_url")]
        public string EditURL { get; set; }
    }
}