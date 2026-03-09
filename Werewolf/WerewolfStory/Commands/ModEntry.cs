using Microsoft.Xna.Framework;
using StardewModdingAPI.Events;
using Commands.Code;
using StardewModdingAPI;
using System.IO;

namespace Commands
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            // Initialize command modules
            Luck.Initialize(this.Monitor);
            Friendship.Initialize(this.Monitor);

            // Register console commands
            helper.ConsoleCommands.Add(
                "player_luck",
                "Applies a custom luck buff to the player.\n\nUsage: player_luck <value> <time>\nExample: player_luck 5 60 (5 luck for 60 seconds)",
                Luck.ApplyLuckBuff
            );

            helper.ConsoleCommands.Add(
                "friendship",
                "Sets the friendship level with an NPC.\n\nUsage: friendship <NPC_ID> <value>\nExample: friendship Abigail 1000\nValue range: 0-2500 (250 points per heart)",
                Friendship.SetFriendship
            );

            helper.ConsoleCommands.Add(
                "list_npcs",
                "Lists all NPCs and their current friendship levels.",
                Friendship.ListNPCs
            );

            this.Monitor.Log("Commands module initialized", LogLevel.Info);
            this.Monitor.Log("Available commands: player_luck, friendship, list_npcs", LogLevel.Debug);
        }
    }
}

