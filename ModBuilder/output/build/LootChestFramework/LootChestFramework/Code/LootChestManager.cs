using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.IO;
using Newtonsoft.Json;

namespace LootChestFramework.Code
{
    public class LootChestManager
    {
        private IMonitor Monitor;
        private IModHelper Helper;
        private Dictionary<string, JsonLootChest.JsonChest> Chests = new();

        public LootChestManager(IMonitor monitor, IModHelper helper)
        {
            Monitor = monitor;
            Helper = helper;
            LoadJsonChests();
        }

        private void LoadJsonChests()
        {
            try
            {
                string path = Path.Combine(Helper.DirectoryPath, "Templates", "ExampleChest.json");
                if (!File.Exists(path)) return;

                string json = File.ReadAllText(path);
                var data = JsonConvert.DeserializeObject<JsonLootChest>(json);
                if (data == null || data.Entries == null) return;

                foreach (var kvp in data.Entries)
                    Chests[kvp.Key] = kvp.Value;

                Monitor.Log($"Loaded {Chests.Count} chests from JSON", LogLevel.Info);
            }
            catch (System.Exception ex)
            {
                Monitor.Log($"Error loading chests JSON: {ex}", LogLevel.Error);
            }
        }

        public void OnDayStarted()
        {
            foreach (var chest in Chests.Values)
            {
                // Optional: Reset logic for daily loot if needed
            }
        }

        public void OnMenuChanged(MenuChangedEventArgs e)
        {
            foreach (var kvp in Chests)
            {
                var chest = kvp.Value;
                if (e.NewMenu is ItemGrabMenu newMenu)
                {
                    Vector2 pos = new(chest.Location.TileX, chest.Location.TileY);
                    GameLocation loc = Game1.getLocationFromName(chest.Location.MapID);
                    if (loc != null && loc.objects.TryGetValue(pos, out var obj) && obj is Chest c)
                    {
                        // Loot laden
                        LoadChestItems(chest, c);
                        // Unzerstörbar
                        c.CanBeGrabbed = !chest.Unbreakable;
                    }
                }
            }
        }

        public void OnSaveLoaded()
        {
            // Initialisieren oder ModData prüfen
        }

        private void LoadChestItems(JsonLootChest.JsonChest chestJson, Chest chest)
        {
            chest.clearNulls();
            chest.clearHeldItems();
            chest.clearItems();
            foreach (var item in chestJson.Items)
            {
                var obj = CreateItem(item.ID, item.Count);
                if (obj != null)
                    chest.addItem(obj);
            }
        }

        private StardewValley.Item? CreateItem(string id, int count)
        {
            if (int.TryParse(id, out int numeric))
                return new StardewValley.Object(numeric, count);

            // Fallback: Name
            return new StardewValley.Object(388, count) { Name = id };
        }
    }
}
