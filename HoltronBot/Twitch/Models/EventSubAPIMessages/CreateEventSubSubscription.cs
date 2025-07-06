using System.Collections.Generic;
using System.Text.Json.Serialization;
using HoltronBot.Twitch.Models.WebsocketMessages;

namespace HoltronBot.Twitch.Models.EventSubAPIMessages
{
    public class CreateEventSubSubscriptionRequest
    {
        public CreateEventSubSubscriptionRequest()
        { }

        public CreateEventSubSubscriptionRequest(string type, string version, List<string> scopes)
        {
            Type = type;
            Version = version;
            Condition = new Condition();
            Transport = new Transport
            {
                Method = "websocket"
            };
            Scopes = scopes;
        }

        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("version")]
        public string Version { get; set; }
        [JsonPropertyName("condition")]
        public Condition Condition { get; set; }
        [JsonPropertyName("transport")]
        public Transport Transport { get; set; }
        [JsonIgnore]
        public List<string> Scopes { get; set; }

        public void SetUserAndSessionData(string broadcasterID, string userID, string moderatorID, string sessionID)
        {
            Condition.BroadcasterUserId = broadcasterID;
            Condition.UserID = userID;
            Condition.ModeratorUserID = moderatorID;
            Transport.SessionID = sessionID;
        }
    }

    public class CreateEventSubSubscriptionResponse
    {
        [JsonPropertyName("data")]
        public List<Data> Data { get; set; }
        [JsonPropertyName("total")]
        public int Total { get; set; }
        [JsonPropertyName("total_cost")]
        public int TotalCost { get; set; }
        [JsonPropertyName("max_total_cost")]
        public int MaxTotalCost { get; set; }
    }
}