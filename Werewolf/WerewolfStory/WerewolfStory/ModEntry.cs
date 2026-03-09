using Microsoft.Xna.Framework;
using StardewModdingAPI.Events;
using WerewolfStory.Code;
using StardewModdingAPI;
using System.IO;

namespace WerewolfStory
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            // Initialize AudioPlayer module
            AudioPlayer.Code.JsonParser.Initialize(this.Monitor);
            AudioPlayer.Code.Player.Initialize(this.Monitor);

            // Register an empty asset so Content Patcher can EditData into it.
            helper.Events.Content.AssetRequested += OnAssetRequested;

            // Subscribe to AudioPlayer events
            helper.Events.GameLoop.GameLaunched += (sender, e) => {
                AudioPlayer.Code.JsonParser.LoadAudioData(this.Helper.GameContent);
            };
            helper.Events.GameLoop.SaveLoaded += (sender, e) => {
                AudioPlayer.Code.JsonParser.LoadAudioData(this.Helper.GameContent);
                AudioPlayer.Code.Player.CheckAndPlaySounds();
            };
            helper.Events.GameLoop.DayStarted += (sender, e) => {
                AudioPlayer.Code.JsonParser.LoadAudioData(this.Helper.GameContent);
                AudioPlayer.Code.Player.CheckAndPlaySounds();
            };
            helper.Events.GameLoop.TimeChanged += (sender, e) => AudioPlayer.Code.Player.CheckAndPlaySounds();
            helper.Events.Player.Warped += (sender, e) => { if (e.IsLocalPlayer) AudioPlayer.Code.Player.CheckAndPlaySounds(); };
            helper.Events.Display.MenuChanged += (sender, e) => AudioPlayer.Code.Player.CheckAndPlaySounds();

            // Initialize Commands module
            Commands.Code.Luck.Initialize(this.Monitor);
            Commands.Code.Friendship.Initialize(this.Monitor);

            // Register console commands
            helper.ConsoleCommands.Add(
                "player_luck",
                "Applies a custom luck buff to the player.\n\nUsage: player_luck <value> <time>\nExample: player_luck 5 60 (5 luck for 60 seconds)",
                Commands.Code.Luck.ApplyLuckBuff
            );

            helper.ConsoleCommands.Add(
                "friendship",
                "Sets the friendship level with an NPC.\n\nUsage: friendship <NPC_ID> <value>\nExample: friendship Abigail 1000\nValue range: 0-2500 (250 points per heart)",
                Commands.Code.Friendship.SetFriendship
            );

            helper.ConsoleCommands.Add(
                "list_npcs",
                "Lists all NPCs and their current friendship levels.",
                Commands.Code.Friendship.ListNPCs
            );

            // WerewolfStory specific initialization
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;

            this.Monitor.Log("WerewolfStory mod initialized with AudioPlayer and Commands modules", LogLevel.Info);
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            string modsPath = Path.GetFullPath(Path.Combine(this.Helper.DirectoryPath, ".."));
            FileChecker.CheckAndRestoreFiles(this.Monitor, modsPath);
        }

        private void OnAssetRequested(object? sender, AssetRequestedEventArgs e)
        {
            if (!e.NameWithoutLocale.IsEquivalentTo("Mods/Smapifan.AudioPlayer"))
            {
                return;
            }

            e.LoadFrom(
                () => new System.Collections.Generic.Dictionary<string, AudioPlayer.Code.AudioEntry>(),
                AssetLoadPriority.Exclusive
            );
        }
    }
}

