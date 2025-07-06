using System.Collections.Generic;

namespace HoltronBot.Twitch.Models.EventSubAPIMessages
{
    public class EventSubSubscriptionMessages
    {
        public static Dictionary<string, CreateEventSubSubscriptionRequest> EventSubSubscriptionsRequests = new()
        {
            {"channel.follow", new CreateEventSubSubscriptionRequest("channel.follow", "2", ["moderator:read:followers"])},
            {"channel.chat.message", new CreateEventSubSubscriptionRequest("channel.chat.message", "1", ["user:read:chat"])},
            {"channel.subscribe", new CreateEventSubSubscriptionRequest("channel.subscribe", "1", ["channel:read:subscriptions"])},
            {"channel.ban", new CreateEventSubSubscriptionRequest("channel.ban", "1", ["channel:moderate"])}
        };
    }
}