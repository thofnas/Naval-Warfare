using System;
using System.Collections.Generic;
using Themes;

namespace Levels
{
    public abstract class Level<T> where T : ThemeSettings
    {
        
        protected Level(List<T> themeSettingsList)
        {
            ThemeSettings = themeSettingsList;
        }

        protected List<T> ThemeSettings { get; set; }

        public abstract T GetAITheme();

        public abstract T GetTheme(Enum themeType);
    }
    
    public class IslandsLevel : Level<IslandsThemeSettings>
    {
        public override IslandsThemeSettings GetAITheme() => ThemeSettings.Find(theme => theme.IslandsTheme == IslandsTheme.AI);
        
        public override IslandsThemeSettings GetTheme(Enum themeType)
        {
            IslandsTheme islandsTheme = (IslandsTheme)themeType;
            return ThemeSettings.Find(themeSettings => themeSettings.IslandsTheme == islandsTheme);
        }

        public IslandsLevel(List<IslandsThemeSettings> themeSettingsList) : base(themeSettingsList)
        {
        }
    }
    
    public class OceanLevel : Level<OceanThemeSetting>
    {
        public override OceanThemeSetting GetAITheme() => ThemeSettings.Find(theme => theme.OceanTheme == OceanTheme.AI);

        public override OceanThemeSetting GetTheme(Enum themeType)
        {
            OceanTheme oceanTheme = (OceanTheme)themeType;
            return ThemeSettings.Find(themeSettings => themeSettings.OceanTheme == oceanTheme);
        }

        public OceanLevel(List<OceanThemeSetting> themeSettingsList) : base(themeSettingsList)
        {
        }
    }
}