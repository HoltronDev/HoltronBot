using System.Text.Json.Serialization;

namespace HoltronBot.Twitch.Models
{
    public class SendMessageBody
    {
        public SendMessageBody(string broadcasterID, string senderID, string message)
        {
            BroadcasterID = broadcasterID;
            SenderID = senderID;
            Message = message;
        }

        [JsonPropertyName("broadcaster_id")]
        public string BroadcasterID {get;set;}

        [JsonPropertyName("sender_id")]
        public string SenderID {get;set;}

        [JsonPropertyName("message")]
        public string Message {get;set;}
    }
}