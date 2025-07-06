using System.Text.Json.Serialization;

namespace HoltronBot.Twitch.Models.WebsocketMessages
{
    public class Cheermote
    {
        [JsonPropertyName("prefix")]
        public string Prefix { get; set; }
        [JsonPropertyName("bits")]
        public int Bits { get; set; }
        [JsonPropertyName("tier")]
        public int Tier { get; set; }
    }
}