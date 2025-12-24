using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace WerewolfStory.Chests
{
    public abstract class BaseRewardChest
    {
        protected abstract string ChestKey { get; }
        protected abstract List<(string id, int amount)> Rewards { get; }

        public string MapID { get; set; } = string.Empty;
        public Vector2 Position { get; set; }
        public IMonitor Monitor { get; set; } = null!;

        private static readonly Dictionary<long, List<Item>> playerChestCache = new();

        public static void ClearPendingUpdates() => playerChestCache.Clear();

        public void OnDayStarted()
        {
            if (string.IsNullOrWhiteSpace(MapID)) 
                return;

            GameLocation loc = Game1.getLocationFromName(MapID);
            if (loc == null || loc.objects == null) 
                return;

            Vector2 tilePos = Position;

            if (!loc.objects.ContainsKey(tilePos))
            {
                Chest chest = new Chest(playerChest: true)
                {
                    Name = ChestKey,
                    CanBeGrabbed = false
                };
                loc.objects[tilePos] = chest;
            }
            else if (loc.objects[tilePos] is Chest existing)
            {
                existing.CanBeGrabbed = false;
            }
        }

        public void OnMenuChanged(MenuChangedEventArgs e)
        {
            if (e.NewMenu is ItemGrabMenu newGrab && newGrab.context is Chest openChest && openChest.Name == ChestKey)
            {
                Farmer player = Game1.player;

                if (!playerChestCache.TryGetValue(player.UniqueMultiplayerID, out var cachedItems))
                {
                    cachedItems = BuildItemsForPlayer(player);
                    playerChestCache[player.UniqueMultiplayerID] = cachedItems;
                }

                SetMenuInventory(newGrab, cachedItems);

                try 
                { 
                    newGrab.inventory.highlightMethod = _ => false; 
                } 
                catch { }

                newGrab.behaviorOnItemGrab = (item, who) =>
                {
                    if (item == null) 
                        return;

                    int taken = item.Stack;
                    string keyId = NormalizeItemId(item);
                    string saveKey = $"playerid.{who.UniqueMultiplayerID}.{ChestKey}.{keyId}";

                    // NICHT addItemToInventoryBool aufrufen - Menu macht das bereits!
                    // who.addItemToInventoryBool(item);

                    if (playerChestCache.TryGetValue(who.UniqueMultiplayerID, out var list))
                    {
                        var existing = list.FirstOrDefault(it => it != null && NormalizeItemId(it) == keyId);
                        if (existing != null)
                        {
                            existing.Stack -= taken;
                            if (existing.Stack <= 0)
                                list.Remove(existing);
                        }
                    }

                    who.modData[saveKey] = "true";

                    if (playerChestCache.TryGetValue(who.UniqueMultiplayerID, out var updated))
                        SetMenuInventory(newGrab, updated);
                };
            }

            if (e.OldMenu is ItemGrabMenu oldGrab && oldGrab.context is Chest oldChest && oldChest.Name == ChestKey)
            {
                playerChestCache.Remove(Game1.player.UniqueMultiplayerID);
                try 
                { 
                    SetMenuInventory(oldGrab, new List<Item>()); 
                } 
                catch { }
            }
        }

        private List<Item> BuildItemsForPlayer(Farmer player)
        {
            var list = new List<Item>();

            foreach (var (id, amount) in Rewards)
            {
                try
                {
                    Item createdItem = CreateItem(id, amount);
                    if (createdItem == null)
                        continue;

                    string key = NormalizeItemId(createdItem);
                    string saveKey = $"playerid.{player.UniqueMultiplayerID}.{ChestKey}.{key}";

                    if (!player.modData.ContainsKey(saveKey))
                        list.Add(createdItem);
                }
                catch (Exception ex)
                {
                    Monitor?.Log($"Fehler beim Erstellen von Item '{id}': {ex.Message}", LogLevel.Warn);
                }
            }

            return list;
        }

        private Item CreateItem(string id, int amount)
        {
            // 1. Numerische ID (Vanilla Object)
            if (int.TryParse(id, out int numericId))
            {
                try
                {
                    var obj = new StardewValley.Object(numericId, amount);
                    obj.Stack = amount;
                    return obj;
                }
                catch { }
            }

            // 2. Suche in Game1.objectInformation (Vanilla Name)
            try
            {
                foreach (var kvp in Game1.objectInformation)
                {
                    string objectName = kvp.Value.Split('/')[0];
                    if (string.Equals(objectName, id, StringComparison.OrdinalIgnoreCase))
                    {
                        int index = kvp.Key;
                        var obj = new StardewValley.Object(index, amount);
                        obj.Stack = amount;
                        return obj;
                    }
                }
            }
            catch { }

            // 3. Fallback: Standard-Object mit Namen
            try
            {
                var fallback = new StardewValley.Object(388, amount); // Wood
                fallback.Name = id;
                fallback.Stack = amount;
                return fallback;
            }
            catch { }

            return null;
        }

        private string NormalizeItemId(Item item)
        {
            if (item == null)
                return "unknown";

            try
            {
                var prop = item.GetType().GetProperty("ItemId");
                if (prop != null)
                {
                    var val = prop.GetValue(item) as string;
                    if (!string.IsNullOrEmpty(val))
                        return val;
                }
            }
            catch { }

            try 
            { 
                return item.ParentSheetIndex.ToString(); 
            }
            catch { }

            try 
            { 
                return item.Name ?? "unknown"; 
            }
            catch { }

            return "unknown";
        }

        private void SetMenuInventory(ItemGrabMenu menu, List<Item> items)
        {
            if (menu == null || items == null)
                return;

            try
            {
                // Methode 1: Direkt auf Chest zugreifen
                if (menu.context is Chest chest)
                {
                    var itemsField = typeof(Chest).GetField("items", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    if (itemsField != null)
                    {
                        var list = itemsField.GetValue(chest) as IList<Item>;
                        if (list != null)
                        {
                            list.Clear();
                            foreach (var it in items)
                                list.Add(it);
                            return;
                        }
                    }
                }
            }
            catch { }

            try
            {
                // Methode 2: actualInventory Field
                var t = typeof(ItemGrabMenu);
                var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
                var field = t.GetField("actualInventory", flags) ?? t.GetField("_actualInventory", flags);
                if (field != null)
                {
                    field.SetValue(menu, items);
                    return;
                }
            }
            catch { }

            try
            {
                // Methode 3: ActualInventory Property
                var t = typeof(ItemGrabMenu);
                var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
                var prop = t.GetProperty("ActualInventory", flags);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(menu, items);
                    return;
                }
            }
            catch { }
        }
    }
}