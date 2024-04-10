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
        
        private IslandsThemeType _selectedIslandsThemeType;
        private OceanThemeType _selectedOceanThemeType;
        private readonly List<IslandsThemeType> _ownedIslandsThemesList;
        private readonly List<OceanThemeType> _ownedOceanThemesList;

        public PlayerData()
        {
            _money = 50;
            _selectedMapType = MapType.Islands;
            
            _selectedIslandsThemeType = IslandsThemeType.Tropical;
            _ownedIslandsThemesList = new List<IslandsThemeType> { _selectedIslandsThemeType };
            _selectedOceanThemeType = OceanThemeType.Earth;
            _ownedOceanThemesList = new List<OceanThemeType> { _selectedOceanThemeType };
        }

        [JsonConstructor]
        public PlayerData(int money, IslandsThemeType selectedIslandsThemeType, List<IslandsThemeType> ownedIslandsThemesList, OceanThemeType selectedOceanThemeType, List<OceanThemeType> ownedOceanThemesList, MapType selectedMapType)
        {
            _money = money;
            _selectedMapType = selectedMapType;
            
            _selectedIslandsThemeType = selectedIslandsThemeType;
            _ownedIslandsThemesList = ownedIslandsThemesList;
            _selectedOceanThemeType = selectedOceanThemeType;
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
                    MapType.Islands => !_ownedIslandsThemesList.IsEmpty(),
                    MapType.Ocean => !_ownedOceanThemesList.IsEmpty(),
                    // Add additional cases for new map types here
                    _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
                };

                if (!hasOwnedThemes)
                    throw new ArgumentException($"No owned themes for map type: {value}");

                _selectedMapType = value;
            }
        }

        public IslandsThemeType SelectedIslandsThemeType
        {
            get => _selectedIslandsThemeType;
            set
            {
                if (_ownedIslandsThemesList.Contains(value) == false)
                    throw new ArgumentException(nameof(value));
                
                _selectedIslandsThemeType = value;
            }
        }

        public IEnumerable<IslandsThemeType> OwnedIslandsThemesList => _ownedIslandsThemesList;
        
        public void OpenIslandsTheme(IslandsThemeType themeType)
        {
            if (_ownedIslandsThemesList.Contains(themeType))
                throw new ArgumentException(nameof(themeType));
            
            _ownedIslandsThemesList.Add(themeType);
        }
        
        public OceanThemeType SelectedOceanThemeType
        {
            get => _selectedOceanThemeType;
            set
            {
                if (_ownedOceanThemesList.Contains(value) == false)
                    throw new ArgumentException(nameof(value));
                
                _selectedOceanThemeType = value;
            }
        }
        
        public IEnumerable<OceanThemeType> OwnedOceanThemesList => _ownedOceanThemesList;

        public void OpenOceanTheme(OceanThemeType themeType)
        {
            if (_ownedOceanThemesList.Contains(themeType))
                throw new ArgumentException(nameof(themeType));
            
            _ownedOceanThemesList.Add(themeType);
        }
    }
}