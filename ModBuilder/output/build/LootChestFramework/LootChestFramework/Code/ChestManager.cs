using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Menus;
using StardewModdingAPI;

namespace LootChest.Framework.Code
{
    public class ChestManager
    {
        private IModHelper Helper;
        private IMonitor Monitor;
        private Dictionary<string, IChest> Chests = new();
        private dynamic JsonData;

        public ChestManager(IModHelper helper, IMonitor monitor)
        {
            Helper = helper;
            Monitor = monitor;
        }

        public void LoadJson()
        {
            try
            {
                string path = Path.Combine(Helper.DirectoryPath, "assets", "content_en.json"); // adapt per language
                JsonData = Helper.Data.ReadJsonFile<dynamic>(path);
                foreach (var entry in JsonData.Entries)
                {
                    // Create IChest instances dynamically (Chest1, Chest2 etc.)
                    // Add to Chests dictionary
                }
                Monitor.Log("✅ LootChest JSON loaded", LogLevel.Info);
            }
            catch (Exception ex)
            {
                Monitor.Log($"❌ Failed to load JSON: {ex}", LogLevel.Error);
            }
        }

        public void LoadModData()
        {
            // Load loot states for all chests from save ModData
        }

        public void OnDayStarted()
        {
            foreach (var chest in Chests.Values)
            {
                chest.OnDayStarted();
            }
        }

        public void OnMenuChanged(MenuChangedEventArgs e)
        {
            foreach (var chest in Chests.Values)
            {
                chest.OnMenuChanged(e);
            }
        }

        // Weitere Funktionen:
        // - Gruppenbildung basierend auf PlayerCount
        // - Loot laden/speichern in ModData
        // - Chest leeren/auffüllen
        // - Texturen optional überschreiben
    }
}    {
        public string MapID { get; set; } = "";
        public int TileX { get; set; }
        public int TileY { get; set; }
    }
}
