using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Menus;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using Newtonsoft.Json;

namespace LootChestFramework
{
    public class LootManager
    {
        private readonly IMonitor Monitor;
        private readonly IModHelper Helper;
        private Dictionary<string, LootChest> chests = new();

        public LootManager(IMonitor monitor, IModHelper helper)
        {
            Monitor = monitor;
            Helper = helper;
        }

        public void InitializeChests()
        {
            string path = Path.Combine(Helper.DirectoryPath, "LootChests.json");
            if (!File.Exists(path))
            {
                Monitor.Log($"LootChests.json not found at {path}", LogLevel.Warn);
                return;
            }

            string json = File.ReadAllText(path);
            var data = JsonConvert.DeserializeObject<LootChestData>(json);
            if (data?.Entries != null)
            {
                chests = data.Entries;
                Monitor.Log($"Loaded {chests.Count} loot chests", LogLevel.Info);
            }
        }

        public void OnDayStarted()
        {
            foreach (var chest in chests.Values)
            {
                chest.OnDayStarted();
            }
        }

        public void OnMenuChanged(MenuChangedEventArgs e)
        {
            foreach (var chest in chests.Values)
            {
                chest.OnMenuChanged(e);
            }
        }
    }
}
