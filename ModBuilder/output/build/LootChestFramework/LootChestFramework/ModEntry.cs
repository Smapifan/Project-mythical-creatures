using StardewModdingAPI;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework;
using LootChestFramework.Code;

namespace WerewolfStory
{
    public class ModEntry : Mod
    {
        private LootChestManager chestManager = null!;

        public override void Entry(IModHelper helper)
        {
            chestManager = new LootChestManager(Monitor, helper);
            
            // Events
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.Display.MenuChanged += OnMenuChanged;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
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
            chestManager.OnSaveLoaded();
        }
    }
}
