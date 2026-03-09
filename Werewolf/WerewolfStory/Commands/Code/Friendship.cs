using StardewValley;
using StardewModdingAPI;
using System;
using System.Linq;

namespace Commands.Code
{
    public class Friendship
    {
        private static IMonitor? monitor;

        public static void Initialize(IMonitor mon)
        {
            monitor = mon;
        }

        public static void SetFriendship(string command, string[] args)
        {
            if (!Context.IsWorldReady)
            {
                monitor?.Log("Game not loaded. Please load a save first.", LogLevel.Warn);
                return;
            }

            if (args.Length < 2)
            {
                monitor?.Log("Usage: friendship <NPC_ID> <value>", LogLevel.Info);
                monitor?.Log("Example: friendship Abigail 1000", LogLevel.Info);
                monitor?.Log("Value range: 0-2500 (0 hearts to 10 hearts, 250 points per heart)", LogLevel.Info);
                return;
            }

            string npcName = args[0];
            if (!int.TryParse(args[1], out int friendshipValue))
            {
                monitor?.Log($"Invalid value: {args[1]}. Please provide a valid integer.", LogLevel.Error);
                return;
            }

            // Clamp value to valid range
            if (friendshipValue < 0)
            {
                monitor?.Log($"Value too low. Setting to 0.", LogLevel.Warn);
                friendshipValue = 0;
            }
            if (friendshipValue > 2500)
            {
                monitor?.Log($"Value too high. Setting to 2500 (10 hearts).", LogLevel.Warn);
                friendshipValue = 2500;
            }

            try
            {
                // Check if NPC exists in the game
                NPC? npc = Game1.getCharacterFromName(npcName, mustBeVillager: false);
                
                if (npc == null)
                {
                    monitor?.Log($"NPC '{npcName}' not found. Make sure the NPC exists and the name is correct.", LogLevel.Error);
                    return;
                }

                // Get or create friendship data
                if (!Game1.player.friendshipData.ContainsKey(npcName))
                {
                    // Create new friendship entry if it doesn't exist
                    Game1.player.friendshipData[npcName] = new StardewValley.Friendship();
                }

                // Get current friendship data
                var friendshipData = Game1.player.friendshipData[npcName];
                int oldValue = friendshipData.Points;

                // Set the friendship points
                friendshipData.Points = friendshipValue;

                // Calculate hearts
                int hearts = friendshipValue / 250;
                monitor?.Log($"Set friendship with {npcName} from {oldValue} ({oldValue / 250} hearts) to {friendshipValue} ({hearts} hearts)", LogLevel.Info);
            }
            catch (Exception ex)
            {
                monitor?.Log($"Error setting friendship: {ex.Message}", LogLevel.Error);
            }
        }

        public static void ListNPCs(string command, string[] args)
        {
            if (!Context.IsWorldReady)
            {
                monitor?.Log("Game not loaded. Please load a save first.", LogLevel.Warn);
                return;
            }

            try
            {
                var npcs = Utility.getAllCharacters()
                    .Where(n => n.IsVillager)
                    .OrderBy(n => n.Name)
                    .ToList();

                monitor?.Log($"Available NPCs ({npcs.Count}):", LogLevel.Info);
                foreach (var npc in npcs)
                {
                    if (Game1.player.friendshipData.ContainsKey(npc.Name))
                    {
                        int points = Game1.player.friendshipData[npc.Name].Points;
                        int hearts = points / 250;
                        monitor?.Log($"  - {npc.Name}: {points} points ({hearts} hearts)", LogLevel.Info);
                    }
                    else
                    {
                        monitor?.Log($"  - {npc.Name}: No friendship data", LogLevel.Info);
                    }
                }
            }
            catch (Exception ex)
            {
                monitor?.Log($"Error listing NPCs: {ex.Message}", LogLevel.Error);
            }
        }
    }
}