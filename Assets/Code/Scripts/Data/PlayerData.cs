using System;
using System.Collections.Generic;
using Map;
using ModestTree;
using Newtonsoft.Json;
using Themes;

namespace Data
{
    public class PlayerData
    {
        private int _money;
        private MapType _selectedMapType;
        
        private IslandsTheme _selectedIslandsTheme;
        private readonly List<IslandsTheme> _ownedIslandsThemesList;
        private OceanTheme _selectedOceanTheme;
        private readonly List<OceanTheme> _ownedOceanThemesList;

        public PlayerData()
        {
            _money = 50;
            _selectedMapType = MapType.Islands;
            
            _selectedIslandsTheme = IslandsTheme.Tropical;
            _ownedIslandsThemesList = new List<IslandsTheme> { _selectedIslandsTheme };
            _selectedOceanTheme = OceanTheme.Earth;
            _ownedOceanThemesList = new List<OceanTheme> { _selectedOceanTheme };
        }

        [JsonConstructor]
        public PlayerData(int money, IslandsTheme selectedIslandsTheme, List<IslandsTheme> ownedIslandsThemesList, OceanTheme selectedOceanTheme, List<OceanTheme> ownedOceanThemesList, MapType selectedMapType)
        {
            _money = money;
            _selectedMapType = selectedMapType;
            
            _selectedIslandsTheme = selectedIslandsTheme;
            _ownedIslandsThemesList = ownedIslandsThemesList;
            _selectedOceanTheme = selectedOceanTheme;
            _ownedOceanThemesList = ownedOceanThemesList;
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

        public MapType SelectedMapType
        {
            get => _selectedMapType;
            set
            {
                bool hasOwnedThemes = value switch
                {
                    MapType.Islands => _ownedIslandsThemesList.IsEmpty(),
                    MapType.Ocean => _ownedOceanThemesList.IsEmpty(),
                    // Add additional cases for new map types here
                    _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
                };

                if (!hasOwnedThemes)
                    throw new ArgumentException($"No owned themes for map type: {value}");

                _selectedMapType = value;
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
        
        public OceanTheme SelectedOceanTheme
        {
            get => _selectedOceanTheme;
            set
            {
                if (_ownedOceanThemesList.Contains(value) == false)
                    throw new ArgumentException(nameof(value));
                
                _selectedOceanTheme = value;
            }
        }
        
        public IEnumerable<OceanTheme> OwnedOceanThemesList => _ownedOceanThemesList;

        public void OpenOceanTheme(OceanTheme theme)
        {
            if (_ownedOceanThemesList.Contains(theme))
                throw new ArgumentException(nameof(theme));
            
            _ownedOceanThemesList.Add(theme);
        }
    }
}