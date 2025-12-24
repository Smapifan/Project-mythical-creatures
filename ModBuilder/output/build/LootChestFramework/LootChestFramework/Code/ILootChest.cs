using System.Collections.Generic;

namespace WerewolfStory.Code
{
    public interface ILootChest
    {
        string ChestKey { get; } // Will be set from JSON entry key
        string MapID { get; }
        int TileX { get; }
        int TileY { get; }
        bool Unbreakable { get; }
        bool ForWorld { get; }
        bool ForPlayer { get; }
        int PlayerCount { get; } 
        bool CanStoreItems { get; }
        List<LootItem> Items { get; }
    }

    public class LootItem
    {
        public string ID { get; set; } = "";
        public int Count { get; set; }
    }
}
