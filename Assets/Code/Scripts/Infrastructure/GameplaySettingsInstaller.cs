using Grid;
using UI;
using UnityEngine;
using Utilities;
using Zenject;

namespace Infrastructure
{
    [CreateAssetMenu(fileName = "GameplaySettingsInstaller", menuName = "Installers/GameplaySettingsInstaller")]
    [ZenjectAllowDuringValidation]
    public class GameplaySettingsInstaller : ScriptableObjectInstaller<GameplaySettingsInstaller>, IValidatable
    {
        [SerializeField] private GridSystem.Settings _gridSystem;
        [SerializeField] private ShipsSpawner.Settings _shipsSpawner;
        [SerializeField] private DebugSettings _debugSettings;

        public override void InstallBindings()
        {
            Validate();
            
            Container.BindInstances(_gridSystem, _debugSettings, _shipsSpawner);
        }

        public void Validate()
        {
            if (!Validation.CheckIfNull(this, _gridSystem, nameof(_gridSystem)) ||
                !Validation.CheckIfNull(this, _shipsSpawner, nameof(_shipsSpawner)) ||
                !Validation.CheckIfNull(this, _debugSettings, nameof(_debugSettings))) return;
            
            int totalAmountOfCells = _gridSystem.Width * _gridSystem.Height;
            int biggestAxis = Mathf.Max(_gridSystem.Height, _gridSystem.Width);
            int shipsCellsAmount = 0;
                
            _shipsSpawner.ShipData.ForEach(ship =>
            {
                if (biggestAxis < ship.ShipLength)
                    Debug.LogError($"The ship of length {ship.ShipLength} can't fit in the Grid[{_gridSystem.Width}; {_gridSystem.Height}]", this);
                    
                shipsCellsAmount += ship.ShipLength;
            });
                
            if (totalAmountOfCells < shipsCellsAmount)
                Debug.LogError($"All ships can't fit inside the Grid {shipsCellsAmount} ship cells / {totalAmountOfCells} total", this);
        }
    }
}