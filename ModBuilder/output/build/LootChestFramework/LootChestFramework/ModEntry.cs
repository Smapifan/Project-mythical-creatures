using StardewModdingAPI;

namespace LootChestFramework
{
    public sealed class ModEntry : Mod
    {
        internal static FrameworkHelper Framework = null!;

        public override void Entry(IModHelper helper)
        {
            Framework = new FrameworkHelper(helper, Monitor);
        }

        public override object GetApi()
        {
            return Framework;
        }
    }
}
