using System.Linq;
using Misc;
using Themes;
using Themes.Store;
using UnityEngine;

namespace Rewards
{
    public class IslandsThemeReward : IReward
    {
        public int Amount { get; }
        public NonEmptyString Name { get; }
        
        private readonly IslandsThemeType _themeType;
        private readonly StoreContent _storeContent;
        private readonly ThemeUnlocker _themeUnlocker;

        public IslandsThemeReward(IslandsThemeType themeType, StoreContent storeContent, ThemeUnlocker themeUnlocker)
        {
            _themeType = themeType;
            _storeContent = storeContent;
            _themeUnlocker = themeUnlocker;
            Amount = 1;
            Name = themeType + "Theme";
        }

        public void Award() =>
            _themeUnlocker.Visit(_storeContent.IslandsThemeItems.First(theme => _themeType == theme.IslandsType));
    }
}