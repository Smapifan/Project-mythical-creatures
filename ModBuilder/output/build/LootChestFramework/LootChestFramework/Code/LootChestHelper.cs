using System.Collections.Generic;
using StardewValley;
using StardewModdingAPI;

namespace LootChestFramework.Code
{
    public static class LootChestHelper
    {
        // Save loot info to ModData for World/Player group
        public static void SaveLootGroup(IMonitor monitor, string chestKey, int groupId, Dictionary<string, int> remainingItems)
        {
            var key = $"LootChest.{chestKey}.Group{groupId}";
            Game1.player.modData[key] = string.Join(";", remainingItems); // Simple serialization
            monitor.Log($"Saved loot group {groupId} for chest {chestKey}", LogLevel.Trace);
        }

        // Load loot info from ModData
        public static Dictionary<string, int> LoadLootGroup(string chestKey, int groupId)
        {
            var key = $"LootChest.{chestKey}.Group{groupId}";
            if (Game1.player.modData.TryGetValue(key, out var value))
            {
                var dict = new Dictionary<string, int>();
                foreach (var pair in value.Split(';'))
                {
                    var parts = pair.Split(':');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int count))
                        dict[parts[0]] = count;
                }
                return dict;
            }
            return new Dictionary<string, int>();
        }
    }
}
