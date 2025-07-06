using System;
using System.Collections.Generic;
using HoltronBot.Models;
using HoltronBot.Twitch;
using HoltronBot.Twitch.Models.WebsocketMessages;
using Serilog;

namespace HoltronBot.Features
{
    public class ChatHandler(TwitchAPI twitchAPI, BotConfiguration botConfiguration) : IFeature
    {
        public static List<string> Scopes = ["user:write:chat"];

        private readonly TwitchAPI twitchAPI = twitchAPI;
        private readonly List<string> chattersThisStream = [];

        public void HandlePayload(Payload payload)
        {
            if (!chattersThisStream.Contains(payload.Event.ChatterUserName) && payload.Event.ChatterUserID != botConfiguration.BotUserID)
            {
                chattersThisStream.Add(payload.Event.ChatterUserName);
                if (payload.Event.ChatterUserID == botConfiguration.BroadcasterID && !botConfiguration.BotGreetsStreamer)
                {
                    return;
                }

                if (UserHasTalkedBefore(payload.Event.ChatterUserName))
                {
                    chattersThisStream.Add(payload.Event.ChatterUserName);

                    if (payload.Event.ChatterUserID == botConfiguration.BroadcasterID && !botConfiguration.BotGreetsStreamer)
                    {
                        return;
                    }

                    twitchAPI.SendMessage($"Welcome back to the shit show, @{payload.Event.ChatterUserName}!");
                }
                else
                {
                    InsertUserIntoDatabase(payload.Event.ChatterUserName);

                    if (payload.Event.ChatterUserID == botConfiguration.BroadcasterID && !botConfiguration.BotGreetsStreamer)
                    {
                        return;
                    }

                    twitchAPI.SendMessage($"Greetings @{payload.Event.ChatterUserName} and welcome!");
                }
            }

            switch (payload.Event.Message.Text)
            {
                case "!discord":
                    twitchAPI.SendMessage($"Your discord link, @{payload.Event.ChatterUserName}: https://discord.gg/XwqqGNT7mS");
                    break;
                case "!lurk":
                    twitchAPI.SendMessage($"Your presence is appreciated @{payload.Event.ChatterUserName}!");
                    break;
                case "!commands":
                    twitchAPI.SendMessage($"@{payload.Event.ChatterUserName}: !discord, !lurk, !giveSporkFood, !giveSporkDrink, !sporksHoard (more to come once Holtron stops being lazy.)");
                    break;
            }
        }

        public void Update()
        {
            // Do nothing.
        }

        private static bool UserHasTalkedBefore(string username)
        {
            try
            {
                var conn = DatabaseAccess.GetDBConnection();
                var command = conn.CreateCommand();
                command.CommandText = $"SELECT COUNT(*) FROM viewers WHERE name = '{username}';";

                using var reader = command.ExecuteReader();
                var viewerCount = 0;
                while (reader.Read())
                {
                    viewerCount = reader.GetInt32(0);
                }
                return viewerCount == 1;
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while trying to retrieve user with username {username}. Error: {Message}", username, ex.Message);
                return false;
            }
        }

        private static void InsertUserIntoDatabase(string username)
        {
            try
            {
                var conn = DatabaseAccess.GetDBConnection();
                var command = conn.CreateCommand();
                command.CommandText = $"INSERT INTO viewers (name) VALUES ('{username}');";
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while attempting to insert viewers into the database. Error: {Message}", ex.Message);
            }
        }
    }
}