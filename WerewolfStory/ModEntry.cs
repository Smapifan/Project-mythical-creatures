using StardewModdingAPI;
using StardewModdingAPI.Events;
using WerewolfStory.Code;
using System.IO;

namespace WerewolfStory
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            // Register event handler for game launch
            Helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            // Get the mods path (i.e., the root directory where all mods are located)
            string modsPath = Path.GetFullPath(Path.Combine(this.Helper.DirectoryPath, ".."));

            // Check and restore the files
            FileChecker.CheckAndRestoreFiles(this.Monitor, modsPath);
        }
    }
}
