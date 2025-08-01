using System;
using System.Collections.Generic;
using System.IO;
using HoltronBot.Twitch;
using HoltronBot.Twitch.Models.WebsocketMessages;

namespace HoltronBot.Features
{
    public class Jokes : IFeature
    {
        private Game1 game;
        private TwitchAPI twitchAPI;
        private List<string> jokes;

        public Jokes(Game1 game, TwitchAPI twitchAPI)
        {
            this.game = game;
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
            var joke = jokes[random.Next(0, jokes.Count)];
            twitchAPI.SendMessage(joke);
            game.DisplayText(joke);
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