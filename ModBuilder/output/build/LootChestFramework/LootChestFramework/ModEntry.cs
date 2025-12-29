using StardewModdingAPI;
using StardewModdingAPI.Events;
using LootChest.Framework.Code;

namespace LootChest.Framework
{
    /// <summary>
    /// Main Mod Entry. Registriert Events und initialisiert ChestManager.
    /// </summary>
    public class ModEntry : Mod
    {
        private ChestManager chestManager = null!;

        public override void Entry(IModHelper helper)
        {
            chestManager = new ChestManager(helper, Monitor);

            // Events registrieren
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.Display.MenuChanged += OnMenuChanged;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            Monitor.Log("LootChest Framework initialized.", LogLevel.Info);
            chestManager.LoadChestsFromContent(); // Lädt Content-Patcher JSONs
        }

        private void OnDayStarted(object? sender, DayStartedEventArgs e)
        {
            chestManager.OnDayStarted();
        }

        private void OnMenuChanged(object? sender, MenuChangedEventArgs e)
        {
            chestManager.OnMenuChanged(e);
        }

        private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            chestManager.LoadModData();
        }
    }
}
