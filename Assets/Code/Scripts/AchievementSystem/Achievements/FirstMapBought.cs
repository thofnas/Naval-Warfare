using Data;
using Events;
using Rewards;
using UnityEngine;

namespace AchievementSystem.Achievements
{
    public class FirstMapBought : Achievement<OnThemeUnlocked>
    {
        public sealed override AchievementInfo Info { get; protected set; } 
        public override IReward Reward { get; }

        public FirstMapBought(PersistentData persistentData, AchievementID id, Wallet wallet, LanguageData languageData) : base(persistentData, id, languageData)
        {
            UpdateAchievementInfo();
            Reward = new MoneyReward(10, wallet);
        }

        protected sealed override void UpdateAchievementInfo() => 
            Info = new AchievementInfo(TextData.FirstMapBought_Name, TextData.FirstMapBought_UnlockCondition);

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