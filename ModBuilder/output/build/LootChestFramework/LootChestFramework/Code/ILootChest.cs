using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;

namespace LootChestFramework.Code
{
    public interface ILootChest
    {
        string ChestKey { get; }
        Vector2 Position { get; set; }
        string MapID { get; set; }
        bool Unbreakable { get; }
        bool ForWorld { get; }
        bool ForPlayer { get; }
        int PlayerCount { get; }
        bool CanStoreItems { get; }

        List<LootItem> GetItemsForPlayer(Farmer player);
        void OnMenuOpened(ItemGrabMenu menu, Farmer player);
        void OnMenuClosed(ItemGrabMenu menu, Farmer player);
    }

    public class LootItem
    {
        public string ID { get; set; } = "";
        public int Count { get; set; } = 0;
    }
}
