using System.Collections.Generic;
using EventBus;
using Events;
using Grid;
using States.GameplayStates;
using UnityEngine;
using Utilities;
using Zenject;

public class InteractionSystem : ITickable
{
    private LevelManager _levelManager;
    private TurnSystem _turnSystem;
    // ugly
    private readonly Dictionary<CharacterType, CellPosition> _selectedCellPosition = new() {
    { CharacterType.Player, CellPosition.Zero },
    { CharacterType.Enemy, CellPosition.Zero }
    };

    private GameplayManager _gameplayManager;

    [Inject]
    private void Construct(TurnSystem turnSystem, LevelManager levelManager, GameplayManager gameplayManager)
    {
        _turnSystem = turnSystem;
        _levelManager = levelManager;
        _gameplayManager = gameplayManager;
    }

    public void Tick()
    {
        if (_gameplayManager.IsCurrentState(typeof(PlacingShips))) 
            return;
        
        if (_gameplayManager.IsCurrentState(typeof(BattleResults)))
            return;

        if (_turnSystem.WhoseCurrentTurn() == CharacterType.Enemy) return;
        
        const float interactedCellMaxDistance = 1.5f;

        if (!Input.GetMouseButtonDown(0)) return;

        CharacterType characterType = _turnSystem.WhoWillTakeAHit();
        Vector2 mousePos = MouseWorld2D.GetPosition();
        CellPosition cellPosition = _levelManager.GetCellPosition(characterType, mousePos);

        if (Vector2.Distance(mousePos, _levelManager.GetWorldCellPosition(characterType, cellPosition)) >
            _levelManager.GetCellSize(characterType) * interactedCellMaxDistance)
            return;

        if (_selectedCellPosition[_turnSystem.WhoWillTakeAHit()] == cellPosition)
            Shoot();
        else
            SetSelectedCell(cellPosition);
    }

    public void SetSelectedCell(CellPosition cellPosition)
    {
        if (_gameplayManager.IsCurrentState(typeof(BattleResults)))
            return;
        
        CharacterType whoWillTakeAHit = _turnSystem.WhoWillTakeAHit();
        
        if (_selectedCellPosition[whoWillTakeAHit] != cellPosition)
            EventBus<OnGridCellSelected>.Invoke(new OnGridCellSelected(whoWillTakeAHit,
                _selectedCellPosition[whoWillTakeAHit], cellPosition));

        _selectedCellPosition[whoWillTakeAHit] = cellPosition;
    }

    public void Shoot()
    {
        if (_gameplayManager.IsCurrentState(typeof(PlacingShips))) 
            return;        
        
        if (_gameplayManager.IsCurrentState(typeof(BattleResults)))
            return;

        EventBus<OnShoot>.Invoke(new OnShoot(_turnSystem.WhoWillTakeAHit(), _selectedCellPosition[_turnSystem.WhoWillTakeAHit()]));
    }
}