using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AudioPlayer.Code
{
    public class Sound
    {
        public static List<string> GetMatchingSounds()
        {
            var matchingSounds = new List<string>();
            var audioEntries = JsonParser.GetAudioEntries();

            if (audioEntries == null || audioEntries.Count == 0)
                return matchingSounds;

            var currentYear = Game1.year;
            var currentSeason = Game1.currentSeason;
            var currentDay = Game1.dayOfMonth;
            var currentTime = Game1.timeOfDay;

            foreach (var entry in audioEntries.Values)
            {
                if (CheckConditions(entry, currentYear, currentSeason, currentDay, currentTime))
                {
                    matchingSounds.Add(entry.SoundID);
                }
            }

            return matchingSounds;
        }

        private static bool CheckConditions(AudioEntry entry, int year, string season, int day, int time)
        {
            // Check Year condition (1 = odd years, 2 = even years)
            if (entry.Year != null && entry.Year.Count > 0)
            {
                bool yearMatch = false;
                foreach (var yearCondition in entry.Year)
                {
                    if (yearCondition == "1" && year % 2 == 1)
                    {
                        yearMatch = true;
                        break;
                    }
                    if (yearCondition == "2" && year % 2 == 0)
                    {
                        yearMatch = true;
                        break;
                    }
                }
                if (!yearMatch) return false;
            }

            // Check Season condition
            if (entry.Season != null && entry.Season.Count > 0)
            {
                if (!entry.Season.Contains(season, StringComparer.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            // Check Day condition
            if (entry.Day != null && entry.Day.Count > 0)
            {
                bool dayMatch = false;
                foreach (var dayCondition in entry.Day)
                {
                    if (int.TryParse(dayCondition, out int dayValue))
                    {
                        if (day == dayValue)
                        {
                            dayMatch = true;
                            break;
                        }
                    }
                }
                if (!dayMatch) return false;
            }

            // Check Time condition (range from start to end)
            if (entry.Time != null && entry.Time.Count >= 2)
            {
                if (int.TryParse(entry.Time[0], out int startTime) && 
                    int.TryParse(entry.Time[1], out int endTime))
                {
                    if (time < startTime || time > endTime)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}