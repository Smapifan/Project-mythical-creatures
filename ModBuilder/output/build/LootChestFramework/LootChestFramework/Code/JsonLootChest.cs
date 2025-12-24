using System.Collections.Generic;

namespace LootChestFramework.Code
{
    public class JsonLootChest
    {
        public string? LogName { get; set; }
        public string? Target { get; set; }
        public Dictionary<string, JsonChest>? Entries { get; set; }

        public class JsonChest
        {
            public Location Location { get; set; } = new();
            public bool Unbreakable { get; set; } = true;
            public bool ForWorld { get; set; } = true;
            public bool ForPlayer { get; set; } = false;
            public int PlayerCount { get; set; } = 1;
            public bool CanStoreItems { get; set; } = false;
            public List<LootItem> Items { get; set; } = new();
        }

        public class Location
        {
            public string MapID { get; set; } = "";
            public int TileX { get; set; }
            public int TileY { get; set; }
        }
    }
}
