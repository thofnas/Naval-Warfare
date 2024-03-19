using System.Collections.Generic;

namespace Themes
{
    public class CharactersThemes
    {
        private readonly Dictionary<CharacterType, Theme> _themeSettingsMap;
    
        public CharactersThemes(Theme themePlayer1, Theme themePlayer2)
        {
            _themeSettingsMap = new Dictionary<CharacterType, Theme>
            {
                { CharacterType.Player, themePlayer1 },
                { CharacterType.Enemy, themePlayer2 }
            };
        }
 
        public Theme GetThemeSettings(CharacterType characterType) => _themeSettingsMap[characterType];
    }
}