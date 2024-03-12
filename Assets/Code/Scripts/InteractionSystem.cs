using System.Collections.Generic;
using EventBus;
using Events;
using Grid;
using UnityEngine;
using Utilities;
using Zenject;

public class InteractionSystem : ITickable
{
    private Level _level;
    private TurnSystem _turnSystem;
    // ugly
    private readonly Dictionary<CharacterType, CellPosition> _selectedCellPosition = new() {
    { CharacterType.Player, CellPosition.Zero },
    { CharacterType.Enemy, CellPosition.Zero }
    };

    public void Tick()
    {
        if (_turnSystem.IsPlacingShips()) return;

        // TODO: better to have a sender and then check if the sender has the right to shoot 
        if (_turnSystem.WhoseCurrentTurn() == CharacterType.Enemy) return;

        const float interactedCellMaxDistance = 1.5f;

        if (!Input.GetMouseButtonDown(0)) return;

        CharacterType characterType = _turnSystem.WhoWillTakeAHit();
        Vector2 mousePos = MouseWorld2D.GetPosition();
        CellPosition cellPosition = _level.GetCellPosition(characterType, mousePos);

        if (Vector2.Distance(mousePos, _level.GetWorldCellPosition(characterType, cellPosition)) >
            _level.GetCellSize(characterType) * interactedCellMaxDistance)
            return;

        if (_selectedCellPosition[_turnSystem.WhoWillTakeAHit()] == cellPosition)
            Shoot();
        else
            SetSelectedCell(cellPosition);
    }

    [Inject]
    private void Construct(TurnSystem turnSystem, Level level)
    {
        _turnSystem = turnSystem;
        _level = level;
    }

    public void SetSelectedCell(CellPosition cellPosition)
    {
        CharacterType whoWillTakeAHit = _turnSystem.WhoWillTakeAHit();
        
        if (_selectedCellPosition[whoWillTakeAHit] != cellPosition)
            EventBus<OnGridCellSelected>.Invoke(new OnGridCellSelected(whoWillTakeAHit,
                _selectedCellPosition[whoWillTakeAHit], cellPosition));

        _selectedCellPosition[whoWillTakeAHit] = cellPosition;
    }

    public void Shoot()
    {
        if (_turnSystem.IsPlacingShips()) return;

        EventBus<OnShoot>.Invoke(new OnShoot(_turnSystem.WhoWillTakeAHit(), _selectedCellPosition[_turnSystem.WhoWillTakeAHit()]));
    }
}