using Data;
using Events;
using Rewards;
using Themes;
using Themes.Store;
using UnityEngine;

namespace AchievementSystem.Achievements
{
    public class WinTenTimes : Achievement<OnPlayerStatisticsChanged>
    {
        public sealed override AchievementInfo Info { get; protected set; } 
        public override IReward Reward { get; }

        public WinTenTimes(PersistentData persistentData, AchievementID id, LanguageData languageData, StoreContent storeContent, ThemeUnlocker themeUnlocker) : base(persistentData, id, languageData)
        {
            UpdateAchievementInfo();
            Reward = new OceanThemeReward(themeType: OceanThemeType.AI, storeContent, themeUnlocker);
        }

        protected sealed override void UpdateAchievementInfo() => Info = new AchievementInfo(TextData.WinTenTimes_Name, TextData.WinTenTimes_UnlockCondition);

        protected override bool AreConditionsMet(OnPlayerStatisticsChanged eventArgs)
        {
            return eventArgs.PlayerStatistics.Wins > 10;
        }

        protected override void OnUnlock()
        {
            Debug.Log($"Unlocked {Info.Name} achievement");
        }
    }
}