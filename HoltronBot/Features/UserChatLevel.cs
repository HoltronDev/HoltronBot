using System;
using HoltronBot.Models;
using HoltronBot.Twitch;
using HoltronBot.Twitch.Models.WebsocketMessages;

namespace HoltronBot.Features
{
    public class UserChatLevel(Game1 game, TwitchAPI twitchAPI, BotConfiguration botConfig) : IFeature
    {
        private readonly Game1 game = game;
        private readonly TwitchAPI twitchAPI = twitchAPI;
        private readonly BotConfiguration botConfig = botConfig;

        public void HandlePayload(Payload payload)
        {
            if (payload.Event.ChatterUserID == botConfig.BotUserID || payload.Event.ChatterUserID == botConfig.BroadcasterID)
            {
                return;
            }

            var userData = GetUserChatLevelData(payload.Event.ChatterUserLogin);
            if (userData == null)
            {
                userData = new UserChatLevelData
                {
                    LoginName = payload.Event.ChatterUserLogin
                };
                InsertUserChatLevelData(userData.LoginName);
            }
            else
            {
                userData.CurrentExperience += 1;
                if (userData.CurrentExperience >= Math.ScaleB(botConfig.UserChatLevelConfig.ExperienceToFirstLevel, userData.CurrentLevel))
                {
                    userData.CurrentLevel++;
                    userData.CurrentExperience = 0;
                    twitchAPI.SendMessage($"@{payload.Event.ChatterUserName} has leveled up to {userData.CurrentLevel}. Congrats!");
                    game.DisplayText($"@{payload.Event.ChatterUserName} has leveled up to {userData.CurrentLevel}. Congrats!");
                    UpdateUserChatLevelData(userData);
                }
            }
        }

        public void Update()
        {
            // Ha ha no
        }

        private static UserChatLevelData GetUserChatLevelData(string loginName)
        {
            var conn = DatabaseAccess.GetDBConnection();

            var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM userchatlevel WHERE loginname = '{loginName}';";

            using var reader = cmd.ExecuteReader();
            UserChatLevelData userChatLevelData = null;
            while (reader.Read())
            {
                userChatLevelData = new UserChatLevelData
                {
                    LoginName = reader.GetString(1),
                    CurrentLevel = reader.GetInt32(2),
                    CurrentExperience = reader.GetInt64(3)
                };
            }
            return userChatLevelData;
        }

        private static void InsertUserChatLevelData(string loginName)
        {
            var conn = DatabaseAccess.GetDBConnection();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO userchatlevel (loginname) VALUES ('{loginName}');";
            cmd.ExecuteNonQuery();
        }

        private static void UpdateUserChatLevelData(UserChatLevelData data)
        {
            var conn = DatabaseAccess.GetDBConnection();

            var cmd = conn.CreateCommand();
            cmd.CommandText = $"UPDATE userchatlevel SET currentlevel = {data.CurrentLevel}, currentexperience = {data.CurrentExperience} WHERE loginname = '{data.LoginName}';";
            cmd.ExecuteNonQuery();
        }
    }
}