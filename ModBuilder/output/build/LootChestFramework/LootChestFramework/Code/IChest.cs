using Microsoft.Xna.Framework;
using StardewValley;

namespace LootChest.Framework.Code
{
    public interface IChest
    {
        string ChestKey { get; }
        Vector2 Position { get; set; }
        string MapID { get; set; }
        bool Unbreakable { get; set; }
        bool ForWorld { get; set; }
        bool ForPlayer { get; set; }
        int PlayerCount { get; set; }
        bool CanStoreItems { get; set; }

        void OnDayStarted();
        void OnMenuChanged(object menuChangedEventArgs);
        void LoadFromModData();
        void SaveToModData();
        void ClearContents();
    }
}
