using StardewModdingAPI;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace WerewolfStory
{
    public class ModEntry : Mod
    {
        private LootChestFramework.LootManager lootManager = null!;

        public override void Entry(IModHelper helper)
        {
            lootManager = new LootChestFramework.LootManager(this.Monitor, helper);
            
            // Events
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
            lootManager.InitializeChests();
        }
    }
}
