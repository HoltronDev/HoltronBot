using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HoltronBot.Features;
using HoltronBot.Twitch.Models.WebsocketMessages;
using Serilog;

namespace HoltronBot.Twitch
{
    public class TwitchBot
    {
        private readonly TwitchAPI twitchAPI;
        private readonly Dictionary<string, Action<Message>> websocketHandlers;
        private readonly List<IFeature> enabledFeatures;

        private ClientWebSocket websocketClient;
        private ClientWebSocket websocketReconnectClient;
        private bool reconnectingClient = false;

        public TwitchBot(TwitchAPI twitchAPI, List<IFeature> enabledFeatures)
        {
            websocketClient = new ClientWebSocket();
            websocketClient.Options.KeepAliveInterval = TimeSpan.FromSeconds(20);
            websocketClient.ConnectAsync(new Uri("wss://eventsub.wss.twitch.tv/ws?keepalive_timeout_seconds=30"), default);

            websocketReconnectClient = new ClientWebSocket();
            websocketReconnectClient.Options.KeepAliveInterval = TimeSpan.FromSeconds(20);

            this.twitchAPI = twitchAPI;
            websocketHandlers = InitializeWebsocketHandlerDictionary();
            this.enabledFeatures = enabledFeatures;
        }

        public async Task Update()
        {
            await HandleWebsocketUpdate(websocketClient);

            if (reconnectingClient)
            {
                await HandleWebsocketUpdate(websocketReconnectClient);
            }
        }
        
        private Dictionary<string, Action<Message>> InitializeWebsocketHandlerDictionary()
        {
            return new Dictionary<string, Action<Message>>()
            {
                {"session_welcome", HandleWelcomeMessage},
                {"session_keepalive", HandleKeepAliveMessage},
                {"notification", HandleNotificationMessage},
                {"session_reconnect", HandleReconnectMessage},
                {"revocation", HandleRevocationMessage}
            };
        }

        private void HandleWelcomeMessage(Message message)
        {
            //Console.WriteLine("Handling Welcome Message.");
            Log.Information("Handling Welcome Message");

            if (reconnectingClient)
            {
                Log.Information("Client has reconnected to new edge server.");
                //Console.WriteLine("Client has reconnected to new edge server.");
                websocketClient.CloseAsync(WebSocketCloseStatus.Empty, null, default);
                websocketClient = websocketReconnectClient;
                websocketReconnectClient = new ClientWebSocket();
                websocketReconnectClient.Options.KeepAliveInterval = TimeSpan.FromSeconds(20);
                reconnectingClient = false;
                return;
            }

            twitchAPI.SubscribeToChannels(message.Payload.Session.ID);
        }

        private void HandleKeepAliveMessage(Message message)
        {
            // Nada
        }

        private void HandleReconnectMessage(Message message)
        {
            reconnectingClient = true;
            websocketReconnectClient.ConnectAsync(new Uri(message.Payload.Session.ReconnectURL), default);
        }

        private void HandleRevocationMessage(Message message)
        {
            // You screwed homey.
        }

        private async Task HandleWebsocketUpdate(ClientWebSocket websocketClient)
        {
            if (websocketClient.State != WebSocketState.Open)
            {
                return;
            }

            var buffer = new byte[4092];
            var result = await websocketClient.ReceiveAsync(buffer, default);

            if (result.Count > 0)
            {
                var data = Encoding.UTF8.GetString([.. buffer], 0, result.Count);
                Message message = null;
                try
                {
                    message = JsonSerializer.Deserialize<Message>(data);
                }
                catch (Exception ex)
                {
                    Log.Error("Somethin' borked in the message deserialize. Error: {Message} | Data: {data}", ex.Message, data);
                    //Console.WriteLine($"Somethin' borked in the message deserialize. Error: {ex.Message} | Data: {data}");
                }
                
                var handler = websocketHandlers[message.Metadata.MessageType];
                if (handler != null)
                {
                    handler.Invoke(message);
                }
                else
                {
                    Log.Error("Unknown Message: {MessageType}", message.Metadata.MessageType);
                    //Console.WriteLine($"Unknown Message: {message.Metadata.MessageType}");
                }
            }
        }

        private void HandleNotificationMessage(Message message)
        {
            switch (message.Metadata.SubscriptionType)
            {
                case "channel.chat.message":
                    HandleChatMessage(message.Payload);
                    break;
                case "channel.follow":
                    twitchAPI.SendMessage($"Thank you for following Lord Holtron, @{message.Payload.Event.UserName}!");
                    break;
                case "channel.subscribe":
                    twitchAPI.SendMessage($"Your subscription shall go in the halls of legend, @{message.Payload.Event.UserName}!");
                    break;
                case "channel.ban":
                    twitchAPI.SendMessage($"HA! GET WRECKED {message.Payload.Event.UserName}! Though if you weren't spamming, you might try and appeal to our Lord.");
                    break;
            }
        }

        private void HandleChatMessage(Payload payload)
        {
            foreach (var feature in enabledFeatures)
            {
                feature.HandlePayload(payload);
            }
        }
    }
}