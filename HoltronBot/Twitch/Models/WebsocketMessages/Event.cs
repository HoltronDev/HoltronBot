using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HoltronBot.Twitch.Models.WebsocketMessages
{
    public class Event
    {
        [JsonPropertyName("user_id")]
        public string UserID { get; set; }
        [JsonPropertyName("user_login")]
        public string UserLogin { get; set; }
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }
        [JsonPropertyName("broadcaster_user_id")]
        public string BroadcasterUserID { get; set; }
        [JsonPropertyName("broadcaster_user_login")]
        public string BroadcasterUserLogin { get; set; }
        [JsonPropertyName("broadcaster_user_name")]
        public string BroadcasterUserName { get; set; }
        [JsonPropertyName("source_broadcaster_user_id")]
        public string SourceBroadcasterUserID { get; set; }
        [JsonPropertyName("source_broadcaster_user_login")]
        public string SourceBroadcasterUserLogin { get; set; }
        [JsonPropertyName("source_broadcaster_user_name")]
        public string SourceBroadcasterUserName { get; set; }
        [JsonPropertyName("followed_at")]
        public string FollowedAt { get; set; }
        [JsonPropertyName("chatter_user_id")]
        public string ChatterUserID { get; set; }
        [JsonPropertyName("chatter_user_login")]
        public string ChatterUserLogin { get; set; }
        [JsonPropertyName("chatter_user_name")]
        public string ChatterUserName { get; set; }
        [JsonPropertyName("message_id")]
        public string MessageID { get; set; }
        [JsonPropertyName("source_message_id")]
        public string SourceMessageId { get; set; }
        [JsonPropertyName("is_source_only")]
        public bool? IsSouceOnly { get; set; }
        [JsonPropertyName("message")]
        public EventMessage Message { get; set; }
        [JsonPropertyName("color")]
        public string Color { get; set; }
        [JsonPropertyName("badges")]
        public List<EventBadge> Badges { get; set; }
        [JsonPropertyName("source_badges")]
        public List<EventBadge> SourceBadges { get; set; }
        [JsonPropertyName("message_type")]
        public string MessageType { get; set; }
        [JsonPropertyName("cheer")]
        public string Cheer { get; set; }
        [JsonPropertyName("reply")]
        public Reply Reply { get; set; }
        [JsonPropertyName("channel_points_custom_reward_id")]
        public string ChannelPointsCustomRewardID { get; set; }
        [JsonPropertyName("channel_points_animation_id")]
        public string ChannelPointsAnimationID { get; set; }
    }
}