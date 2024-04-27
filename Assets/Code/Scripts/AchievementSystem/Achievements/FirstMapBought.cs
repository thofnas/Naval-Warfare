using Data;
using Events;
using Rewards;
using UnityEngine;

namespace AchievementSystem.Achievements
{
    public class FirstMapBought : Achievement<OnThemeUnlocked>
    {
        public override AchievementInfo Info { get; } 
        public override IReward Reward { get; }

        public FirstMapBought(PersistentData persistentData, AchievementID id, Wallet wallet) : base(persistentData, id)
        {
            Info = new AchievementInfo("First purchase", "Buy your first map");
            Reward = new MoneyReward(10, wallet);
        }
 
        protected override bool AreConditionsMet(OnThemeUnlocked eventArgs)
        {
            return eventArgs.IsPurchasable;
        }

        protected override void OnUnlock()
        {
            Debug.Log($"Unlocked {nameof(FirstMapBought)} achievement");
        }
    }
}