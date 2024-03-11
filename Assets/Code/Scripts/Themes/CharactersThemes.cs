using System.Collections.Generic;

namespace Themes
{
    public class CharactersThemes
    {
        private readonly Dictionary<CharacterType, ThemeSettings> _themeSettingsMap;
    
        public CharactersThemes(ThemeSettings themeSettingsPlayer1, ThemeSettings themeSettingsPlayer2)
        {
            _themeSettingsMap = new Dictionary<CharacterType, ThemeSettings>
            {
                { CharacterType.Player, themeSettingsPlayer1 },
                { CharacterType.Enemy, themeSettingsPlayer2 }
            };
        }
 
        public ThemeSettings GetThemeSettings(CharacterType characterType) => _themeSettingsMap[characterType];
    }
}