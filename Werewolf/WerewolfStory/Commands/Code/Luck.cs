using StardewValley;
using StardewValley.Buffs;
using StardewModdingAPI;

// ich habe ein fehler. es fehlt die Time also player_luck <value> <time> time ist sekunden
using System;

namespace Commands.Code
{
    public class Luck
    {
        private static IMonitor? monitor;

        public static void Initialize(IMonitor mon)
        {
            monitor = mon;
        }

        public static void ApplyLuckBuff(string command, string[] args)
        {
            if (!Context.IsWorldReady)
            {
                monitor?.Log("Game not loaded. Please load a save first.", LogLevel.Warn);
                return;
            }

            if (args.Length < 2)
            {
                monitor?.Log("Usage: player_luck <value> <time>", LogLevel.Info);
                monitor?.Log("Example: player_luck 5 60 (5 luck for 60 seconds)", LogLevel.Info);
                return;
            }

            if (!int.TryParse(args[0], out int luckValue))
            {
                monitor?.Log($"Invalid value: {args[0]}. Please provide a valid integer.", LogLevel.Error);
                return;
            }

            if (!int.TryParse(args[1], out int timeInSeconds))
            {
                monitor?.Log($"Invalid time: {args[1]}. Please provide a valid integer (seconds).", LogLevel.Error);
                return;
            }

            // Convert seconds to milliseconds
            int durationInMilliseconds = timeInSeconds * 1000;

            try
            {
                // Create a custom luck buff that is a 1:1 clone of the vanilla luck buff
                var buff = new Buff(
                    id: "Smapifan.Commands.CustomLuck",
                    source: "command",
                    displaySource: "Command",
                    duration: durationInMilliseconds,
                    iconTexture: Game1.buffsIcons,
                    iconSheetIndex: 8, // Luck buff icon index
                    effects: new BuffEffects()
                    {
                        LuckLevel = { luckValue }
                    },
                    displayName: "Luck"
                );

                Game1.player.applyBuff(buff);
                monitor?.Log($"Applied luck buff with value: {luckValue} for {timeInSeconds} seconds", LogLevel.Info);
            }
            catch (Exception ex)
            {
                monitor?.Log($"Error applying luck buff: {ex.Message}", LogLevel.Error);
            }
        }

        public static void RemoveLuckBuff()
        {
            try
            {
                Game1.player.buffs.Remove("Smapifan.Commands.CustomLuck");
                monitor?.Log("Removed custom luck buff", LogLevel.Info);
            }
            catch (Exception ex)
            {
                monitor?.Log($"Error removing luck buff: {ex.Message}", LogLevel.Error);
            }
        }
    }
}