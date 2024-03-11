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
        private CharactersThemes _charactersThemes;

        [Inject]
        private void Construct(CharactersThemes charactersThemes, GridSystem.Factory gridSystemFactory, GridSystemVisual.Factory gridSystemVisualFactory,
            DebugSettings debugSettings)
        {
            _gridSystemFactory = gridSystemFactory;
            _gridSystemVisualFactory = gridSystemVisualFactory;
            _debugSettings = debugSettings;
            _charactersThemes = charactersThemes;
        }

        public GridSystem Spawn(CharacterType characterType, Vector2 firstGridPosition)
        {
            GridSystem gridSystem = _gridSystemFactory.Create(characterType);
            GridSystemVisual gridSystemVisual = _gridSystemVisualFactory.Create(gridSystem, firstGridPosition, _charactersThemes.GetThemeSettings(characterType));
            gridSystem.SetGridSystemVisual(gridSystemVisual);

#if UNITY_EDITOR
            if (_debugSettings.CreateDebugObjects)
                gridSystem.CreateCellDebug(_debugSettings.GridDebugObjectPrefab);
#endif

            return gridSystem;
        }
    }
}