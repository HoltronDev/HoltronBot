using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using HoltronBot.Features;
using HoltronBot.Models;
using HoltronBot.Twitch;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;

namespace HoltronBot;

public class Game1 : Game
{
    public string TextToDisplay;

    private TwitchAPI twitchAPI;
    private TwitchAuth twitchAuth;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private TwitchBot twitchBot;
    private List<IFeature> enabledFeatures;
    private BotConfiguration botConfig;
    private Stopwatch stopwatch;
    private bool displayNotification;
    private Texture2D notificationBox;
    private SpriteFont font;
    private Rectangle notificationRect;

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
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.ApplyChanges();

        notificationRect = new Rectangle(new Point((1920 / 2) - (738 / 2), 1080 - 268), new Point(738, 168));

        // Configuration (need to take a path eventually)
        botConfig = BotConfiguration.LoadConfiguration();
        if (botConfig == null)
        {
            Log.Fatal("Failed to load configuration for bot, shutting down!");
            Exit();
        }

        stopwatch = new Stopwatch();
        notificationBox = Content.Load<Texture2D>("Notification Panel");
        font = Content.Load<SpriteFont>("KenneyFuture");

        // Services
        _ = new DatabaseAccess();
        twitchAuth = new TwitchAuth(botConfig);
        twitchAPI = new TwitchAPI(twitchAuth, botConfig);

        // Get enabled Features.
        enabledFeatures = InitializeEnabledFeatures(twitchAPI, botConfig);

        // Finally, the bot
        twitchBot = new TwitchBot(twitchAPI, enabledFeatures);

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

        if (displayNotification && stopwatch.Elapsed.Seconds > botConfig.TimeToDisplayNotifications)
        {
            displayNotification = false;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Green);

        // TODO: Add your drawing code here
        if (displayNotification)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(notificationBox, notificationRect, Color.White);
            DrawChunkedString(TextToDisplay);
            _spriteBatch.End();
        }

        base.Draw(gameTime);
    }

    public void DisplayText(string textToDisplay)
    {
        if (!botConfig.DisplayNotifications)
        {
            return;
        }

        displayNotification = true;
        TextToDisplay = textToDisplay;
        if (TextToDisplay.Length > 30)
        {
            for (int i = 30; i < TextToDisplay.Length; i += 30)
            {
                TextToDisplay = TextToDisplay.Insert(i, "\r");
            }
        }
        stopwatch.Restart();
    }

    private List<IFeature> InitializeEnabledFeatures(TwitchAPI twitchAPI, BotConfiguration botConfig)
    {
        var features = new List<IFeature>();

        foreach (var feature in botConfig.Features)
        {
            switch (feature)
            {
                case "ChatHandler":
                    features.Add(new ChatHandler(this, twitchAPI, botConfig));
                    break;
                case "Jokes":
                    features.Add(new Jokes(this, twitchAPI));
                    break;
                case "Stockpile":
                    features.Add(new Stockpile(this, twitchAPI));
                    break;
                case "Clippinator":
                    features.Add(new Clippinator(this, twitchAPI));
                    break;
                case "ShoutOut":
                    features.Add(new ShoutOut(this, twitchAPI));
                    break;
                case "UserChatLevel":
                    features.Add(new UserChatLevel(this, twitchAPI, botConfig));
                    break;
            }
        }

        return features;
    }

    private void DrawChunkedString(string text)
    {
        var chunks = new List<string>();
        var index = 0;
        while (index + 33 < text.Length)
        {
            chunks.Add(text.Substring(index, 33));
            index += 33;
        }
        chunks.Add(text[index..]);
        for (int i = 0; i < chunks.Count; i++)
        {
            _spriteBatch.DrawString(font, chunks[i], new Vector2(notificationRect.Left + 220, notificationRect.Top + 20 + (26 * i)), Color.White);
        }
    }
}
