using System.Collections.Generic;
using System.Threading.Tasks;
using HoltronBot.Features;
using HoltronBot.Models;
using HoltronBot.Twitch;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;

namespace HoltronBot;

public class Game1 : Game
{
    private Microsoft.Extensions.Logging.ILogger logger;
    private TwitchAPI twitchAPI;
    private TwitchAuth twitchAuth;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private TwitchBot twitchBot;
    private List<IFeature> enabledFeatures;
    private BotConfiguration botConfig;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File("logs/spork.txt", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
    }

    protected override void Initialize()
    {
        // Configuration (need to take a path eventually)
        botConfig = BotConfiguration.LoadConfiguration();
        if (botConfig == null)
        {
            // TODO: This will need to be changed later to be more visible to users.'
            logger.Log(LogLevel.Critical, "Failed to load configuration for bot, shutting down!");
            //Console.WriteLine("Failed to load configuration for bot, shutting down!");
            Exit();
        }

        // Services
        _ = new DatabaseAccess();
        twitchAuth = new TwitchAuth(botConfig);
        twitchAPI = new TwitchAPI(twitchAuth, botConfig);

        // Get enabled Features.
        enabledFeatures = InitializeEnabledFeatures(twitchAPI, botConfig);

        // Finally, the bot
        twitchBot = new Twitch.TwitchBot(twitchAPI, enabledFeatures);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        foreach (var feature in enabledFeatures)
        {
            feature.Update();
        }

        Task.Run(twitchBot.Update);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }

    private List<IFeature> InitializeEnabledFeatures(TwitchAPI twitchAPI, BotConfiguration botConfig)
    {
        var features = new List<IFeature>();

        foreach (var feature in botConfig.Features)
        {
            switch (feature)
            {
                case "ChatHandler":
                    features.Add(new ChatHandler(twitchAPI, botConfig));
                    break;
                case "Jokes":
                    features.Add(new Jokes(twitchAPI));
                    break;
                case "Stockpile":
                    features.Add(new Stockpile(twitchAPI));
                    break;
            }
        }

        return features;
    }
}
