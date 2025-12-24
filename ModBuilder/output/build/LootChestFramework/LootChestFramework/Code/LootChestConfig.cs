using System.Collections.Generic;

namespace WerewolfStory.Code
{
    public class LootChestConfig
    {
        public string? LogName { get; set; }
        public string Target { get; set; } = "Smapifan/LootChest.Framework";
        public Dictionary<string, LootChest> Entries { get; set; } = new();
    }
}
