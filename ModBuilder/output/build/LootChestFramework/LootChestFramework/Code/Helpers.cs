using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using StardewValley;

namespace LootChest.Framework.Code
{
    public static class Helpers
    {
        public static string GetFileHash(string path)
        {
            using var stream = File.OpenRead(path);
            using var md5 = MD5.Create();
            return Convert.ToBase64String(md5.ComputeHash(stream));
        }

        public static Item CreateItem(string id, int count)
        {
            if (int.TryParse(id, out int numericId))
                return new StardewValley.Object(numericId, count);
            else
            {
                var item = new StardewValley.Object(388, count); // fallback: wood
                item.Name = id;
                return item;
            }
        }
    }
}
