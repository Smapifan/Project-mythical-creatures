using Microsoft.Xna.Framework;
using StardewModdingAPI.Events;
using AudioPlayer.Code;
using StardewModdingAPI;
using System.IO;

namespace AudioPlayer
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            // Initialize modules
            JsonParser.Initialize(this.Monitor);
            Player.Initialize(this.Monitor);

            // Register an empty asset so Content Patcher can EditData into it.
            helper.Events.Content.AssetRequested += OnAssetRequested;

            // Subscribe to game events
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.GameLoop.TimeChanged += OnTimeChanged;
            helper.Events.Player.Warped += OnPlayerWarped;
            helper.Events.Display.MenuChanged += OnMenuChanged;

            this.Monitor.Log("AudioPlayer initialized", LogLevel.Info);
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            // Intentionally do not load here; CP patches may not be applied yet.
        }

        private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            // Check conditions when save is loaded
            JsonParser.LoadAudioData(this.Helper.GameContent);
            Player.CheckAndPlaySounds();
        }

        private void OnDayStarted(object? sender, DayStartedEventArgs e)
        {
            // Check conditions at the start of each day
            this.Monitor.Log("New day started, checking audio conditions", LogLevel.Trace);
            JsonParser.LoadAudioData(this.Helper.GameContent);
            Player.CheckAndPlaySounds();
        }

        private void OnTimeChanged(object? sender, TimeChangedEventArgs e)
        {
            // Check conditions when time changes (every 10 minutes in-game)
            Player.CheckAndPlaySounds();
        }

        private void OnPlayerWarped(object? sender, WarpedEventArgs e)
        {
            // Check conditions when player changes location
            if (e.IsLocalPlayer)
            {
                Player.CheckAndPlaySounds();
            }
        }

        private void OnMenuChanged(object? sender, MenuChangedEventArgs e)
        {
            // Check conditions when menu opens or closes
            Player.CheckAndPlaySounds();
        }

        private void OnAssetRequested(object? sender, AssetRequestedEventArgs e)
        {
            if (!e.NameWithoutLocale.IsEquivalentTo("Mods/Smapifan.AudioPlayer"))
            {
                return;
            }

            e.LoadFrom(
                () => new System.Collections.Generic.Dictionary<string, AudioEntry>(),
                AssetLoadPriority.Exclusive
            );
        }
    }
}

