using System.Text.Json.Serialization;

namespace HoltronBot.Twitch.Models.WebsocketMessages
{
    public class EventMessageFragment
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("cheermote")]
        public Cheermote Cheermote { get; set; }
        [JsonPropertyName("emote")]
        public Emote Emote { get; set; }
        [JsonPropertyName("mention")]
        public Mention Mention { get; set; }
    }
}