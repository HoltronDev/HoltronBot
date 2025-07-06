using System.IO;
using System.Text.Json;

namespace HoltronBot.Models
{
    public class BotConfiguration
    {
        public string BotName { get; set; }
        public string BotUserID { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string BroadcasterID { get; set; }
        public string RedirectURI { get; set; }
        public bool BotGreetsStreamer { get; set; }
        public string[] Subscriptions { get; set; }
        public string[] Features { get; set; }

        public static BotConfiguration LoadConfiguration(string filepath = null)
        {
            filepath ??= "botconfig.json";

            if (!File.Exists(filepath))
            {
                return null;
            }

            using var reader = new StreamReader(filepath);
            var json = reader.ReadToEnd();
            return JsonSerializer.Deserialize<BotConfiguration>(json);
        }

        public void SaveConfiguration(string filepath = null)
        {
            filepath ??= "./botconfig.json";

            var json = JsonSerializer.Serialize(this);
            using var writer = new StreamWriter(filepath);
            writer.Write(json);
        }
    }
}