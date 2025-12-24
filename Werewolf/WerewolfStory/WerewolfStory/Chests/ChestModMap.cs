using System.Collections.Generic;

namespace WerewolfStory.Chests
{
    public class ChestModMap : BaseRewardChest
    {
        protected override string ChestKey => "RewardChestModMap";

        protected override List<(string id, int amount)> Rewards { get; } = new()
        {
            ("335", 5),  // Gold Bar
            ("337", 3),  // Refined Quartz
            ("93", 5)   // String-ID
        };
    }
}
