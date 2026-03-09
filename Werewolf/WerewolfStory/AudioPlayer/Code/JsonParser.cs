using StardewModdingAPI;
using System.Collections.Generic;

namespace AudioPlayer.Code
{
    public class AudioEntry
    {
        public string Id { get; set; } = "";
        public string SoundID { get; set; } = "";
        public List<string> Year { get; set; } = new List<string>();
        public List<string> Season { get; set; } = new List<string>();
        public List<string> Day { get; set; } = new List<string>();
        public List<string> Time { get; set; } = new List<string>();
    }

    public class JsonParser
    {
        private static Dictionary<string, AudioEntry> audioEntries = new Dictionary<string, AudioEntry>();
        private const string AssetName = "Mods/Smapifan.AudioPlayer";
        private static IMonitor? monitor;

        public static void Initialize(IMonitor mon)
        {
            monitor = mon;
        }

        public static void LoadAudioData(IGameContentHelper gameContent)
        {
            try
            {
                gameContent.InvalidateCache(AssetName);
                var loaded = gameContent.Load<Dictionary<string, AudioEntry>>(AssetName);
                if (loaded == null)
                {
                    monitor?.Log($"Could not load asset: {AssetName}", LogLevel.Warn);
                    return;
                }

                audioEntries.Clear();
                foreach (var pair in loaded)
                {
                    if (pair.Value == null)
                    {
                        continue;
                    }

                    pair.Value.Id = pair.Key;
                    if (string.IsNullOrWhiteSpace(pair.Value.SoundID))
                    {
                        continue;
                    }

                    audioEntries[pair.Key] = pair.Value;
                    monitor?.Log($"Loaded audio entry: {pair.Key} -> {pair.Value.SoundID}", LogLevel.Trace);
                }

                monitor?.Log($"Loaded {audioEntries.Count} audio entries from asset {AssetName}", LogLevel.Debug);
            }
            catch (System.Exception ex)
            {
                monitor?.Log($"Error loading audio data from asset {AssetName}: {ex.Message}", LogLevel.Error);
            }
        }

        public static Dictionary<string, AudioEntry> GetAudioEntries()
        {
            return audioEntries;
        }

        public static void Clear()
        {
            audioEntries.Clear();
        }
    }
}