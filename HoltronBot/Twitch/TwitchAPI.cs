using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using HoltronBot.Models;
using HoltronBot.Twitch.Models.EventSubAPIMessages;
using HoltronBot.Twitch.Models.WebsocketMessages;
using RestSharp;
using Serilog;

namespace HoltronBot.Twitch
{
    public class TwitchAPI
    {
        private const string baseURL = "https://api.twitch.tv/";
        private readonly string broadcasterID;
        private readonly string clientID;
        private readonly string userID;

        private readonly TwitchAuth twitchAuth;
        private readonly JsonSerializerOptions jsonOptions;

        public TwitchAPI(TwitchAuth twitchAuth, BotConfiguration botConfig)
        {
            this.twitchAuth = twitchAuth;
            jsonOptions = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
            broadcasterID = botConfig.BroadcasterID;
            clientID = botConfig.ClientID;
            userID = botConfig.BotUserID;
        }

        public void SendMessage(string message)
        {
            var sendMessage = new SendMessage
            {
                BroadcasterID = broadcasterID,
                SenderID = userID,
                Message = message
            };

            var jsonBody = JsonSerializer.Serialize(sendMessage);

            var request = new RestRequest(baseURL + "helix/chat/messages", Method.Post)
                .AddHeader("Authorization", $"Bearer {twitchAuth.GetUserToken()}")
                .AddHeader("Client-Id", clientID)
                .AddHeader("Content-Type", "application/json")
                .AddBody(jsonBody);

            var client = new RestClient();
            var response = client.Execute(request);

            Log.Debug("{Content}", response.Content);
            //Console.WriteLine(response.Content);
        }

        public void SubscribeToChannels(string sessionID)
        {
            var subscribeMessage = new CreateEventSubSubscriptionRequest()
            {
                Type = "channel.chat.message",
                Version = "1",
                Condition = new Condition
                {
                    BroadcasterUserId = broadcasterID,
                    UserID = userID
                },
                Transport = new Transport
                {
                    Method = "websocket",
                    SessionID = sessionID
                }
            };

            var json = JsonSerializer.Serialize(subscribeMessage, jsonOptions);
            var subscribeRequest = new RestRequest(new Uri("https://api.twitch.tv/helix/eventsub/subscriptions"), Method.Post)
                .AddHeader("Authorization", "Bearer " + twitchAuth.GetAppToken())
                .AddHeader("Client-Id", clientID)
                .AddHeader("Content-Type", "application/json")
                .AddBody(json);

            Log.Information("Sending Subscribe Message");
            var client = new RestClient();
            var response = client.Execute(subscribeRequest);

            if (response.IsSuccessful)
            {
                var result = JsonSerializer.Deserialize<CreateEventSubSubscriptionResponse>(response.Content);
                Log.Debug("{Content}", response.Content);
            }
        }
    }
}