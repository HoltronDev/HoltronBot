using System.Text.Json.Serialization;

namespace HoltronBot.Twitch.Models.WebsocketMessages
{
    public class Reply
    {
        [JsonPropertyName("parent_message_id")]
        public string ParentMessageID { get; set; }
        [JsonPropertyName("parent_message_body")]
        public string ParentMessageBody { get; set; }
        [JsonPropertyName("parent_user_id")]
        public string ParentUserID { get; set; }
        [JsonPropertyName("parent_user_name")]
        public string ParentUserName { get; set; }
        [JsonPropertyName("parent_user_login")]
        public string ParentUserLogin { get; set; }
        [JsonPropertyName("thread_message_id")]
        public string ThreadMessageID { get; set; }
        [JsonPropertyName("thread_user_id")]
        public string ThreadUserID { get; set; }
        [JsonPropertyName("thread_user_name")]
        public string ThreadUserName { get; set; }
        [JsonPropertyName("thread_user_login")]
        public string ThreadUserLogin { get; set; }
    }
}