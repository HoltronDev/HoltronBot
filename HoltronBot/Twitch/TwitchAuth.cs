using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using HoltronBot.Models;
using HoltronBot.Twitch.Models.EventSubAPIMessages;
using RestSharp;

namespace HoltronBot.Twitch
{
    public class TwitchAuth
    {
        public const string twitchAuthURL = "https://id.twitch.tv/";

        private readonly string clientID;
        private readonly string clientSecret;
        private readonly string redirectURI;
        private string appToken = null;
        private string appRefreshToken = null;
        private string userToken = null;
        private string accessCode = null;

        private readonly List<string> scopes;

        public TwitchAuth(BotConfiguration botConfig)
        {
            var scopes = new List<string>();
            foreach (var subscription in botConfig.Subscriptions)
            {
                scopes.AddRange(EventSubSubscriptionMessages.EventSubSubscriptionsRequests[subscription].Scopes);
            }
            this.scopes = [.. scopes.Distinct()];
            clientID = botConfig.ClientID;
            clientSecret = botConfig.ClientSecret;
            redirectURI = botConfig.RedirectURI;
            GetUserToken();
            GetAppToken();
        }

        public string GetAppToken()
        {
            if (appToken != null)
            {
                return appToken;
            }

            // TODO: Refresh Logic

            Task.Run(GetAuthorizationCodeFromTwitch).Wait();
            Task.Run(GetAppTokenWithCodeFromTwitch).Wait();

            return appToken;
        }

        public string GetUserToken()
        {
            if (userToken != null)
            {
                return userToken;
            }

            // TODO: Refresh Logic

            GetUserTokenFromTwitch();

            return userToken;
        }

        private void GetUserTokenFromTwitch()
        {
            var client = new RestClient(twitchAuthURL);

            var request = new RestRequest("oauth2/token", Method.Post)
                .AddHeader("Content-Type", "application/x-www-form-urlencoded")
                .AddParameter("client_id", clientID)
                .AddParameter("client_secret", clientSecret)
                .AddParameter("grant_type", "client_credentials")
                .AddParameter("scope", string.Join(' ', scopes));

            var response = client.Post(request);
            var authResponse = JsonSerializer.Deserialize<TwitchTokenResponse>(response.Content);
            userToken = authResponse.AccessToken;
        }

        private void GetAuthorizationCodeFromTwitch()
        {
            var url = new Uri("https://id.twitch.tv/oauth2/authorize" +
            "?response_type=code" +
            "&client_id=" + clientID +
            "&redirect_uri=" + redirectURI +
            //"&scope=" + formattedScopes);
            "&scope=" + HttpUtility.UrlEncode(string.Join(' ', scopes)));
            Process.Start(new ProcessStartInfo
            {
                FileName = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
                Arguments = url.ToString(),
                UseShellExecute = true
            });

            var cancelToken = new CancellationToken();
            var authResponseTask = HttpServer.HandleIncomingConnection(cancelToken);
            authResponseTask.Wait();
            var authResponse = authResponseTask.Result;

            if (authResponse != null)
            {
                accessCode = authResponse.Code;
            }
        }

        private void GetAppTokenWithCodeFromTwitch()
        {
            var client = new RestClient(twitchAuthURL);

            var request = new RestRequest("oauth2/token", Method.Post)
                .AddHeader("Content-Type", "application/x-www-form-urlencoded")
                .AddParameter("client_id", clientID)
                .AddParameter("client_secret", clientSecret)
                .AddParameter("code", accessCode)
                .AddParameter("grant_type", "authorization_code")
                .AddParameter("redirect_uri", "http://localhost:3000");

            var response = client.Post(request);
            var authResponse = JsonSerializer.Deserialize<TwitchTokenResponse>(response.Content);
            appToken = authResponse.AccessToken;
            appRefreshToken = authResponse.RefreshToken;
        }
    }
}