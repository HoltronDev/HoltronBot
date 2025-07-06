using System;
using System.Collections.Generic;
using System.IO;
using HoltronBot.Twitch;
using HoltronBot.Twitch.Models.WebsocketMessages;

namespace HoltronBot.Features
{
    public class Jokes : IFeature
    {
        private TwitchAPI twitchAPI;
        private List<string> jokes;

        public Jokes(TwitchAPI twitchAPI)
        {
            this.twitchAPI = twitchAPI;
            InitializeJokeLibrary();
        }

        public void HandlePayload(Payload payload)
        {
            if (payload.Event.Message.Text != "!joke")
            {
                return;
            }

            var random = new Random();
            twitchAPI.SendMessage(jokes[random.Next(0, jokes.Count)]);
        }

        public void Update()
        {
            // Nothing to update.
        }

        private void InitializeJokeLibrary()
        {
            jokes = [.. File.ReadAllLines("FeatureFiles\\jokes.txt")];
        }
    }
}