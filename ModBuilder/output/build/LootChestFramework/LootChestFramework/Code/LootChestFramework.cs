using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using StardewValley;
using Microsoft.Xna.Framework;
using StardewModdingAPI;

namespace LootChestFramework.Code
{
    public class LootChestFramework
    {
        private readonly IMonitor Monitor;
        private readonly string JsonPath;
        public Dictionary<string, ILootChest> Chests = new();

        public LootChestFramework(IMonitor monitor, string jsonPath)
        {
            Monitor = monitor;
            JsonPath = jsonPath;
            LoadJson();
        }

        private void LoadJson()
        {
            if (!File.Exists(JsonPath))
            {
                Monitor.Log($"Loot JSON not found at {JsonPath}", LogLevel.Error);
                return;
            }

            string jsonText = File.ReadAllText(JsonPath);
            try
            {
                var root = JsonConvert.DeserializeObject<LootJsonRoot>(jsonText);
                if (root?.Entries != null)
                {
                    foreach (var kvp in root.Entries)
                    {
                        var chest = kvp.Value;
                        chest.ChestKey = kvp.Key; // Use entry key as ChestKey
                        Chests[kvp.Key] = chest;
                    }
                    Monitor.Log($"Loaded {Chests.Count} loot chests", LogLevel.Info);
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed to parse Loot JSON: {ex}", LogLevel.Error);
            }
        }

        public ILootChest? GetChest(string chestKey)
        {
            if (Chests.TryGetValue(chestKey, out var chest))
                return chest;
            return null;
        }

        // Calculate which players belong to which loot group
        public List<List<long>> GetLootGroups(ILootChest chest, List<long> allPlayerIDs)
        {
            var groups = new List<List<long>>();
            if (chest.ForWorld)
            {
                groups.Add(allPlayerIDs);
            }
            else if (chest.ForPlayer && chest.PlayerCount > 0)
            {
                for (int i = 0; i < allPlayerIDs.Count; i += chest.PlayerCount)
                {
                    groups.Add(allPlayerIDs.GetRange(i, Math.Min(chest.PlayerCount, allPlayerIDs.Count - i)));
                }
            }
            return groups;
        }
    }

    // JSON deserialization classes
    public class LootJsonRoot
    {
        public string? LogName { get; set; }
        public string Target { get; set; } = "";
        public Dictionary<string, LootChestJson> Entries { get; set; } = new();
    }

    public class LootChestJson : ILootChest
    {
        [JsonIgnore] public string ChestKey { get; set; } = "";
        public LocationJson Location { get; set; } = new();
        public bool Unbreakable { get; set; }
        public bool ForWorld { get; set; }
        public bool ForPlayer { get; set; }
        public int PlayerCount { get; set; }
        public bool CanStoreItems { get; set; }
        public List<LootItem> Items { get; set; } = new();

        public string MapID => Location.MapID;
        public int TileX => Location.TileX;
        public int TileY => Location.TileY;
    }

    public class LocationJson
    {
        public string MapID { get; set; } = "";
        public int TileX { get; set; }
        public int TileY { get; set; }
    }
}
