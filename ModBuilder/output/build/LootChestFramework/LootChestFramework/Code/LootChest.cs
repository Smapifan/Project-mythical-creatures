using StardewValley;
using StardewValley.Menus;
using StardewModdingAPI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace WerewolfStory.Code
{
    public class LootChest
    {
        public string ChestKey { get; set; } = "";
        public Location Location { get; set; } = new();
        public bool Unbreakable { get; set; } = true;
        public bool ForWorld { get; set; } = true;
        public bool ForPlayer { get; set; } = false;
        public int PlayerCount { get; set; } = 1;
        public bool CanStoreItems { get; set; } = false;
        public List<LootItem> Items { get; set; } = new();

        private Dictionary<long, List<Item>> playerLootCache = new();

        public void InitializeForWorld()
        {
            // Hier könnte man World-Loot initialisieren
        }

        public void RestoreFromModData()
        {
            // Loot-Daten aus ModData laden
        }

        public void HandleMenu(MenuChangedEventArgs e)
        {
            if (e.NewMenu is ItemGrabMenu menu && menu.context is Chest chest && chest.Name == ChestKey)
            {
                // LootChest öffnen, Items setzen
            }
        }
    }

    public class Location
    {
        public string MapID { get; set; } = "";
        public int TileX { get; set; }
        public int TileY { get; set; }
    }

    public class LootItem
    {
        public string ID { get; set; } = "";
        public int Count { get; set; }
    }
}
