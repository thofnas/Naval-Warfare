using EventBus;
using Events;
using Levels;
using Themes;

namespace Infrastructure
{
    public class SelectedSettings
    {
        public LevelType SelectedLevelType { get; }
        
        private ThemeSettings _playerThemeSettings;

        public SelectedSettings(ThemeSettings playerThemeSettings, LevelType selectedLevelType)
        {
            _playerThemeSettings = playerThemeSettings;
            SelectedLevelType = selectedLevelType;
        }

        public ThemeSettings PlayerThemeSettings
        {
            get => _playerThemeSettings;
            set
            {
                _playerThemeSettings = value;
                EventBus<OnThemeChanged>.Invoke(new OnThemeChanged());
            }
        }
    }
}