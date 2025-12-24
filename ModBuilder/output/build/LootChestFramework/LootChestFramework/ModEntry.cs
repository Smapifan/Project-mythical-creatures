using StardewModdingAPI;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework;
using LootChestFramework.Code;

namespace WerewolfStory
{
    public class ModEntry : Mod
    {
        private LootManager lootManager = null!;

        public override void Entry(IModHelper helper)
        {
            // LootManager initialisieren
            lootManager = new LootManager(this.Monitor, helper);
            
            // Events registrieren
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.Display.MenuChanged += OnMenuChanged;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        }

        private void OnDayStarted(object? sender, DayStartedEventArgs e)
        {
            lootManager.OnDayStarted();
        }

        private void OnMenuChanged(object? sender, MenuChangedEventArgs e)
        {
            lootManager.OnMenuChanged(e);
        }

        private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            lootManager.OnSaveLoaded();
        }
    }
}
