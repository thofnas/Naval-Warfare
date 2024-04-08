using EventBus;
using Events;
using Themes;

namespace Infrastructure
{
    public class SelectedTheme
    {
        private Theme _playerTheme;

        public SelectedTheme(Theme startingTheme)
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