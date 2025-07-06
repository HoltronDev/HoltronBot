using System;
using System.Text.Json.Serialization;

namespace HoltronBot.Twitch.Models.WebsocketMessages
{
    public class Message
    {
        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; }
        [JsonPropertyName("payload")]
        public Payload Payload { get; set; }
    }
}