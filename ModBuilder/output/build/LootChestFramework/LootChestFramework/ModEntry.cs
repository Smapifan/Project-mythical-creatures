using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace LootChest.Framework
{
    public class ModEntry : Mod
    {
        private List<Code.LootChest> chests = new();

        public override void Entry(IModHelper helper)
        {
            // Initialize chests
            LoadChests();

            // Register events
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.Display.MenuChanged += OnMenuChanged;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        }

        private void LoadChests()
        {
            // Example chest from JSON or hardcoded
            chests.Add(new Code.LootChest(this.Monitor)
            {
                ChestKey = "Chest1",
                MapID = "Farm",
                TileX = 30,
                TileY = 30,
                Unbreakable = true,
                ForWorld = true,
                ForPlayer = false,
                PlayerCount = 1,
                CanStoreItems = false,
                Items = new List<Code.LootChest.ItemEntry>
                {
                    new Code.LootChest.ItemEntry{ ID = "388", Count = 10 },
                    new Code.LootChest.ItemEntry{ ID = "390", Count = 5 },
                    new Code.LootChest.ItemEntry{ ID = "Steak", Count = 3 }
                }
            });
        }

        private void OnDayStarted(object? sender, DayStartedEventArgs e)
        {
            foreach (var chest in chests)
                chest.OnDayStarted();
        }

        private void OnMenuChanged(object? sender, MenuChangedEventArgs e)
        {
            foreach (var chest in chests)
                chest.OnMenuChanged(e);
        }

        private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            foreach (var chest in chests)
                chest.InitializeModData();
        }
    }
}
