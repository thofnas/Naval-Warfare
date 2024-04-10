using Data;
using Infrastructure;
using Map;
using Themes;
using UnityEngine;
using Zenject;

namespace Grid
{
    public class GridSystemSpawner
    {
        private DebugSettings _debugSettings;
        private GridSystem.Factory _gridSystemFactory;
        private GridSystemVisual.Factory _gridSystemVisualFactory;
        private PersistentData _persistentData;
        private MapLibrary _mapLibrary;
        private SelectedTheme _selectedTheme;

        [Inject]
        private void Construct(PersistentData persistentData, MapLibrary mapLibrary, SelectedTheme selectedTheme, GridSystem.Factory gridSystemFactory, GridSystemVisual.Factory gridSystemVisualFactory,
            DebugSettings debugSettings)
        {
            _persistentData = persistentData;
            _mapLibrary = mapLibrary;
            _selectedTheme = selectedTheme;
            _gridSystemFactory = gridSystemFactory;
            _gridSystemVisualFactory = gridSystemVisualFactory;
            _debugSettings = debugSettings;
        }

        public GridSystem Spawn(CharacterType characterType, Vector2 firstGridPosition)
        {
            Theme theme = characterType == CharacterType.Enemy
                ? _mapLibrary.Maps[_persistentData.PlayerData.SelectedMapType].AITheme
                : _selectedTheme.PlayerTheme;
            GridSystem gridSystem = _gridSystemFactory.Create(characterType);
            GridSystemVisual gridSystemVisual = _gridSystemVisualFactory.Create(gridSystem, firstGridPosition, theme);
            
            gridSystem.SetGridSystemVisual(gridSystemVisual);

#if UNITY_EDITOR
            if (_debugSettings.CreateDebugObjects)
                gridSystem.CreateCellDebug(_debugSettings.GridDebugObjectPrefab);
#endif

            return gridSystem;
        }
    }
}