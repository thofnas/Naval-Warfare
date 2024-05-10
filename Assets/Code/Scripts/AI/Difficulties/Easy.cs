using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace AI.Difficulties
{
    public class Easy : IDifficulty
    {
        private readonly AIDamagedShipSearcher _aiDamagedShipSearcher;
        private readonly DebugSettings _debugSettings;
        private readonly LevelManager _levelManager;
        private readonly ShipsManager _shipsManager;
        private readonly TurnSystem _turnSystem;

        public Easy(ShipsManager shipsManager, TurnSystem turnSystem, LevelManager levelManager,
            AIDamagedShipSearcher aiDamagedShipSearcher, DebugSettings debugSettings)
        {
            _shipsManager = shipsManager;
            _turnSystem = turnSystem;
            _levelManager = levelManager;
            _aiDamagedShipSearcher = aiDamagedShipSearcher;
            _debugSettings = debugSettings;
        }

        public List<EnemyAIAction> CalculateActions()
        {
            CharacterType characterType = _turnSystem.WhoWillTakeAHit();

            int turnCount = _turnSystem.GetCharactersTurnCount(characterType);
            
            // debug
#if  UNITY_EDITOR
            if (turnCount > 0 && turnCount <= _debugSettings.FirstCellPositionsAIWillShoot.Count)
            {
                CellPosition shootingCellPosition = CellPosition.FromSerializableData(
                    _debugSettings.FirstCellPositionsAIWillShoot[turnCount - 1]);
                Debug.Log($"{nameof(characterType)} is forcefully shooting at {shootingCellPosition}");
                return new List<EnemyAIAction> { new(shootingCellPosition, 1000) };
            }
#endif

            if (_shipsManager.HasUndestroyedShip())
                if (_aiDamagedShipSearcher.TrySearchForShip(characterType,
                        _levelManager.GetHitUndestroyedCellPositions(characterType),
                        out CellPosition selectedCellPosition))
                    return new List<EnemyAIAction> { new(selectedCellPosition, 2) };

            CellPosition cellPosition = _levelManager.GetRandomUnshotCellPosition(characterType);

            return new List<EnemyAIAction> { new(cellPosition, 1) };
        }

        public int GetWinMoneyAmount() => 10;
    }
}