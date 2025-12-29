using StardewModdingAPI;
using StardewModdingAPI.Events;
using LootChest.Framework.Code;

namespace LootChest.Framework
{
    public class ModEntry : Mod
    {
        private ChestManager chestManager = null!;

        public override void Entry(IModHelper helper)
        {
            chestManager = new ChestManager(this.Helper, this.Monitor);

            // Game events
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.Display.MenuChanged += OnMenuChanged;
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            chestManager.LoadJson(); // Load JSON from assets/
        }

        private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            chestManager.LoadModData(); // Load loot states from save
        }

        private void OnDayStarted(object? sender, DayStartedEventArgs e)
        {
            chestManager.OnDayStarted();
        }

        private void OnMenuChanged(object? sender, MenuChangedEventArgs e)
        {
            chestManager.OnMenuChanged(e);
        }
    }
}
