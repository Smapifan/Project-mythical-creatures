using System.Collections.Generic;

namespace WerewolfStory.Chests
{
    public class Chest2 : BaseRewardChest
    {
        protected override string ChestKey => "RewardChest2";

        protected override List<(string id, int amount)> Rewards { get; } = new()
        {
            ("382", 10), // Coal
            ("334", 5),  // Iron Bar
            ("33", 3) // String-ID Beispiel
        };
    }
}
