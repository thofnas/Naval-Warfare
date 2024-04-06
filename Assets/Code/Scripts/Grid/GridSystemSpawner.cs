using Data;
using Levels;
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
        private Level<ThemeSettings> _level;

        [Inject]
        private void Construct(PersistentData persistentData, GridSystem.Factory gridSystemFactory, GridSystemVisual.Factory gridSystemVisualFactory,
            DebugSettings debugSettings, Level<ThemeSettings> level)
        {
            _persistentData = persistentData;
            _gridSystemFactory = gridSystemFactory;
            _gridSystemVisualFactory = gridSystemVisualFactory;
            _debugSettings = debugSettings;
            _level = level;
        }

        public GridSystem Spawn(CharacterType characterType, Vector2 firstGridPosition)
        {
            ThemeSettings themeSettings = characterType == CharacterType.Enemy
                ? _level.GetAITheme()
                : _level.GetTheme(_persistentData.PlayerData.SelectedIslandsTheme);
            GridSystem gridSystem = _gridSystemFactory.Create(characterType);
            GridSystemVisual gridSystemVisual = _gridSystemVisualFactory.Create(gridSystem, firstGridPosition, themeSettings);
            
            gridSystem.SetGridSystemVisual(gridSystemVisual);

#if UNITY_EDITOR
            if (_debugSettings.CreateDebugObjects)
                gridSystem.CreateCellDebug(_debugSettings.GridDebugObjectPrefab);
#endif

            return gridSystem;
        }
    }
}