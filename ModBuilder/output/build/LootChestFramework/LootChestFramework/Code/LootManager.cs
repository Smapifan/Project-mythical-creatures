using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace WerewolfStory.Code
{
    public class LootManager
    {
        private IMonitor Monitor;
        private IModHelper Helper;
        private Dictionary<string, LootChest> chests = new();

        public LootManager(IMonitor monitor, IModHelper helper)
        {
            Monitor = monitor;
            Helper = helper;

            LoadChests();
        }

        private void LoadChests()
        {
            string path = Path.Combine(Helper.DirectoryPath, "LootChests.json");
            if (!File.Exists(path))
            {
                Monitor.Log($"LootChests.json nicht gefunden: {path}", LogLevel.Warn);
                return;
            }

            string json = File.ReadAllText(path);
            var doc = JsonSerializer.Deserialize<LootChestConfig>(json);
            if (doc?.Entries != null)
            {
                foreach (var kv in doc.Entries)
                    chests[kv.Key] = kv.Value;
            }

            Monitor.Log($"Geladene LootChests: {chests.Count}", LogLevel.Info);
        }

        public void OnDayStarted()
        {
            foreach (var chest in chests.Values)
                chest.InitializeForWorld();
        }

        public void OnMenuChanged(MenuChangedEventArgs e)
        {
            foreach (var chest in chests.Values)
                chest.HandleMenu(e);
        }

        public void OnSaveLoaded()
        {
            foreach (var chest in chests.Values)
                chest.RestoreFromModData();
        }
    }
}
