﻿using Data;
using Events;
using Rewards;
using Themes;
using Themes.Store;
using UnityEngine;

namespace AchievementSystem.Achievements
{
    public class WinFiveTimes : Achievement<OnPlayerStatisticsChanged>
    {
        public sealed override AchievementInfo Info { get; protected set; } 
        public override IReward Reward { get; }

        public WinFiveTimes(PersistentData persistentData, AchievementID id, LanguageData languageData, StoreContent storeContent, ThemeUnlocker themeUnlocker) : base(persistentData, id, languageData)
        {
            UpdateAchievementInfo();
            Reward = new IslandsThemeReward(themeType: IslandsThemeType.AI, storeContent, themeUnlocker);
        }

        protected sealed override void UpdateAchievementInfo() => Info = new AchievementInfo(TextData.WinFiveTimes_Name, TextData.WinFiveTimes_UnlockCondition);

        protected override bool AreConditionsMet(OnPlayerStatisticsChanged eventArgs)
        {
            return eventArgs.PlayerStatistics.Wins > 5;
        }

        protected override void OnUnlock()
        {
            Debug.Log($"Unlocked {Info.Name} achievement");
        }
    }
}