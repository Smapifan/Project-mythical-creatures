using StardewValley;
using StardewModdingAPI;
using System.Collections.Generic;

namespace AudioPlayer.Code
{
    public class Player
    {
        private static string? currentlyPlayingSound = null;
        private static IMonitor? monitor;

        public static void Initialize(IMonitor mon)
        {
            monitor = mon;
        }

        public static void CheckAndPlaySounds()
        {
            var matchingSounds = Sound.GetMatchingSounds();

            if (matchingSounds.Count == 0)
            {
                // No matching sounds, stop current sound if any
                if (currentlyPlayingSound != null)
                {
                    StopSound();
                }
                return;
            }

            // Get the first matching sound (you could implement priority logic here)
            string soundToPlay = matchingSounds[0];

            // If it's different from what's currently playing, switch
            if (currentlyPlayingSound != soundToPlay)
            {
                if (currentlyPlayingSound != null)
                {
                    StopSound();
                }
                PlaySound(soundToPlay);
            }
        }

        private static void PlaySound(string soundId)
        {
            try
            {
                // First try vanilla music track, then fallback to vanilla sound effect.
                Game1.changeMusicTrack(soundId);

                if (Game1.currentSong == null)
                {
                    Game1.playSound(soundId);
                    monitor?.Log($"Playing as sound effect: {soundId}", LogLevel.Trace);
                }
                else
                {
                    monitor?.Log($"Playing as music track: {soundId}", LogLevel.Trace);
                }

                currentlyPlayingSound = soundId;
            }
            catch (System.Exception ex)
            {
                monitor?.Log($"Error playing sound {soundId}: {ex.Message}", LogLevel.Error);
            }
        }

        private static void StopSound()
        {
            try
            {
                if (currentlyPlayingSound != null)
                {
                    monitor?.Log($"Stopping sound: {currentlyPlayingSound}", LogLevel.Trace);
                    Game1.changeMusicTrack("none");
                    currentlyPlayingSound = null;
                }
            }
            catch (System.Exception ex)
            {
                monitor?.Log($"Error stopping sound: {ex.Message}", LogLevel.Error);
            }
        }

        public static void ForceStop()
        {
            if (currentlyPlayingSound != null)
            {
                StopSound();
            }
        }

        public static string? GetCurrentlyPlaying()
        {
            return currentlyPlayingSound;
        }
    }
}