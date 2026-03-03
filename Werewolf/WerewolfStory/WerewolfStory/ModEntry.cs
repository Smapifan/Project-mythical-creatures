using Microsoft.Xna.Framework;
using StardewModdingAPI.Events;
using WerewolfStory.Code;
using StardewModdingAPI;
using System.IO;

namespace WerewolfStory
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            string modsPath = Path.GetFullPath(Path.Combine(this.Helper.DirectoryPath, ".."));
            FileChecker.CheckAndRestoreFiles(this.Monitor, modsPath);
        }
    }
}

