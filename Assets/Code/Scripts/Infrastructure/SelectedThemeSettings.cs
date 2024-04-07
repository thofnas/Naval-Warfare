using EventBus;
using Events;
using Themes;

namespace Infrastructure
{
    public class SelectedThemeSettings
    {
        private Theme _playerTheme;

        public SelectedThemeSettings(Theme startingTheme)
        {
            _playerTheme = startingTheme;
        }
    
        public Theme PlayerTheme
        {
            get => _playerTheme;
            set
            {
                _playerTheme = value;
                EventBus<OnThemeChanged>.Invoke(new OnThemeChanged());
            }
        }
    }
}