using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Themes;

namespace Data
{
    public class PlayerData
    {
        private IslandsTheme _selectedIslandsTheme;

        private List<IslandsTheme> _ownedIslandsThemesList;

        private int _money;

        public PlayerData()
        {
            _money = 50;

            _selectedIslandsTheme = IslandsTheme.Tropical;
            _ownedIslandsThemesList = new List<IslandsTheme> { _selectedIslandsTheme };
        }

        [JsonConstructor]
        public PlayerData(IslandsTheme selectedIslandsTheme, List<IslandsTheme> ownedIslandsThemesList, int money)
        {
            _selectedIslandsTheme = selectedIslandsTheme;
            _ownedIslandsThemesList = ownedIslandsThemesList;
            _money = money;
        }

        public int Money
        {
            get => _money;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _money = value;
            }
        }

        public IslandsTheme SelectedIslandsTheme
        {
            get => _selectedIslandsTheme;
            set
            {
                if (_ownedIslandsThemesList.Contains(value) == false)
                    throw new ArgumentException(nameof(value));
                
                _selectedIslandsTheme = value;
            }
        }

        public IEnumerable<IslandsTheme> OwnedIslandsThemesList => _ownedIslandsThemesList;

        public void OpenIslandsTheme(IslandsTheme theme)
        {
            if (_ownedIslandsThemesList.Contains(theme))
                throw new ArgumentException(nameof(theme));
            
            _ownedIslandsThemesList.Add(theme);
        }
    }
}