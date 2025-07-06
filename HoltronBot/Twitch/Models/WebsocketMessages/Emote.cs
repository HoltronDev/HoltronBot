using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HoltronBot.Twitch.Models.WebsocketMessages
{
    public class Emote
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }
        [JsonPropertyName("emote_set_id")]
        public string EmoteSetID { get; set; }
        [JsonPropertyName("owner_id")]
        public string OwnerID { get; set; }
        [JsonPropertyName("format")]
        public List<string> Format { get; set; }
    }
}