using System.Collections.Generic;

namespace WerewolfStory.Chests
{
    public class Chest1 : BaseRewardChest
    {
        protected override string ChestKey => "RewardChest1";

        protected override List<(string id, int amount)> Rewards { get; } = new()
        {
            ("388", 10), // Wood
            ("390", 5),  // Stone
            ("Steak", 3) // String-ID Beispiel
        };
    }
}
