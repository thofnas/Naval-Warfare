using System.Linq;
using Misc;
using Themes;
using Themes.Store;

namespace Rewards
{
    public class OceanThemeReward : IReward
    {
        public int Amount { get; }
        public NonEmptyString Name { get; }
                 
        private readonly OceanThemeType _themeType;
        private readonly StoreContent _storeContent;
        private readonly ThemeUnlocker _themeUnlocker;
         
        public OceanThemeReward(OceanThemeType themeType, StoreContent storeContent, ThemeUnlocker themeUnlocker)
        {
            _themeType = themeType;
            _storeContent = storeContent;
            _themeUnlocker = themeUnlocker;
            Amount = 1;
            Name = $"Ocean {themeType} Theme";
        }
         
        public void Award() =>
            _themeUnlocker.Visit(_storeContent.OceanThemeItems.First(theme => _themeType == theme.OceanType));
    }
}