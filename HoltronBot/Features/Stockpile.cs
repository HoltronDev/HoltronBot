using System;
using HoltronBot.Twitch;
using HoltronBot.Twitch.Models.WebsocketMessages;
using Serilog;

namespace HoltronBot.Features
{
    public class Stockpile : IFeature
    {
        private int foodStock = 0;
        private int drinkStock = 0;

        private readonly TwitchAPI twitchAPI;

        public Stockpile(TwitchAPI twitchAPI)
        {
            this.twitchAPI = twitchAPI;
            Initialize();
        }

        public void Update()
        {
            // We will do timed usage in here.
        }

        public void HandlePayload(Payload payload)
        {
            switch (payload.Event.Message.Text)
            {
                case "!giveSporkFood":
                    twitchAPI.SendMessage(AddToFoodStock());
                    break;
                case "!giveSporkDrink":
                    twitchAPI.SendMessage(AddToDrinkStock());
                    break;
                case "!sporksHoard":
                    twitchAPI.SendMessage(GetCurrentStockpile());
                    break;
                case "!commands":
                    twitchAPI.SendMessage($"@{payload.Event.ChatterUserName}: !discord, !lurk, !giveSporkFood, !giveSporkDrink, !sporksHoard (more to come once Holtron stops being lazy.)");
                    break;
            }
        }

        private void Initialize()
        {
            try
            {
                using var conn = DatabaseAccess.GetDBConnection();
                var command = conn.CreateCommand();
                command.CommandText = "SELECT count FROM stockpile WHERE foodName = 'pizzaSlice';";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        foodStock = reader.GetInt32(0);
                    }
                }

                command.CommandText = "SELECT count FROM stockpile WHERE foodName = 'soda';";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        drinkStock = reader.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while initializing Stockpile Database. Error: {Message}", ex.Message);
            }

        }

        private string GetCurrentStockpile()
        {
            var pizzas = foodStock != 1 ? "pizza slices" : "pizza slice";
            var sodas = drinkStock != 1 ? "sodas" : "soda";
            return $"My hoard currently contains {foodStock} {pizzas} and {drinkStock} {sodas}.";
        }

        private string AddToFoodStock()
        {
            foodStock++;
            try
            {
                using var conn = DatabaseAccess.GetDBConnection();
                var command = conn.CreateCommand();
                command.CommandText = $"UPDATE stockpile SET count = {foodStock} WHERE foodName = 'pizzaSlice';";
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Error("Error adding drink to stockpile. Error: {Message}", ex.Message);
                return "Apparently Holtron sucks at coding, tell him so!";
            }
            return $"I suppose I could add this to my hoard...";
        }

        private string AddToDrinkStock()
        {
            drinkStock++;
            try
            {
                using var conn = DatabaseAccess.GetDBConnection();
                var command = conn.CreateCommand();
                command.CommandText = $"UPDATE stockpile SET count = {drinkStock} WHERE foodName = 'soda';";
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Error("Error adding drink to stockpile. Error: {Message}", ex.Message);
                return "Apparently Holtron sucks at coding, tell him so!";
            }
            return $"Ah, yes, a refreshing beverage for me!";
        }
    }
}