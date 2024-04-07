using Data;
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
        private ThemeLibrary _themeLibrary;

        [Inject]
        private void Construct(PersistentData persistentData, ThemeLibrary themeLibrary, GridSystem.Factory gridSystemFactory, GridSystemVisual.Factory gridSystemVisualFactory,
            DebugSettings debugSettings)
        {
            _persistentData = persistentData;
            _themeLibrary = themeLibrary;
            _gridSystemFactory = gridSystemFactory;
            _gridSystemVisualFactory = gridSystemVisualFactory;
            _debugSettings = debugSettings;
        }

        public GridSystem Spawn(CharacterType characterType, Vector2 firstGridPosition)
        {
            Theme theme = characterType == CharacterType.Enemy
                ? _themeLibrary.GetTheme(IslandsTheme.AI)
                : _themeLibrary.GetTheme(_persistentData.PlayerData.SelectedIslandsTheme);
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