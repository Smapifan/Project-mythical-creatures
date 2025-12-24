using Microsoft.Xna.Framework;
using StardewModdingAPI.Events;
using WerewolfStory.Code;
using StardewModdingAPI;
using System.IO;

namespace WerewolfStory
{
    public class ModEntry : Mod
    {
        private Chests.Chest1 chest1 = null!;
        private Chests.Chest2 chest2 = null!;
        private Chests.Chest3 chest3 = null!;
        private Chests.ChestModMap chestModMap = null!;

        public override void Entry(IModHelper helper)
        {
            chest1 = new Chests.Chest1 { MapID = "SilvansHome", Position = new Vector2(7, 7) };
            chest2 = new Chests.Chest2 { MapID = "SilvansHome", Position = new Vector2(8, 8) };
            chest3 = new Chests.Chest3 { MapID = "Farm", Position = new Vector2(30, 30) };
            chestModMap = new Chests.ChestModMap { MapID = "SilvansHome", Position = new Vector2(53, 13) };

            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.Display.MenuChanged += OnMenuChanged;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            string modsPath = Path.GetFullPath(Path.Combine(this.Helper.DirectoryPath, ".."));
            FileChecker.CheckAndRestoreFiles(this.Monitor, modsPath);
        }

        private void OnDayStarted(object? sender, DayStartedEventArgs e)
        {
            chest1.OnDayStarted();
            chest2.OnDayStarted();
            chest3.OnDayStarted();
            chestModMap.OnDayStarted();
        }

        private void OnMenuChanged(object? sender, MenuChangedEventArgs e)
        {
            chest1.OnMenuChanged(e);
            chest2.OnMenuChanged(e);
            chest3.OnMenuChanged(e);
            chestModMap.OnMenuChanged(e);
        }

        private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            Chests.BaseRewardChest.ClearPendingUpdates();
        }
    }
}
