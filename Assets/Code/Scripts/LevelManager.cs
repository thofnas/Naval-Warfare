using System;
using System.Collections.Generic;
using System.Linq;
using Grid;
using UnityEngine;
using Zenject;

public class LevelManager : IInitializable, IDisposable
{
    private readonly Dictionary<CharacterType, GridSystem> _gridSystems = new();
    private readonly GridSystemSpawner _gridSystemSpawner;
    private readonly ShipsSpawner _shipsSpawner;
    private Vector2 _firstPlayerGridPosition;
    private Vector2 _firstEnemyGridPosition;
    
    [Inject]
    private LevelManager(GridSystemSpawner gridSystemSpawner, ShipsSpawner shipsSpawner)
    {
        _gridSystemSpawner = gridSystemSpawner;
        _shipsSpawner = shipsSpawner;
    }

    public void Initialize()
    {
        _gridSystems.Clear();
        
        Vector2[] firstGridPositions = CameraController.GetHalvesCenterPositions();
        _firstPlayerGridPosition = firstGridPositions[0];
        _firstEnemyGridPosition = firstGridPositions[1];
        
        GridSystem playerGrid = _gridSystemSpawner.Spawn(CharacterType.Player, _firstPlayerGridPosition);
        GridSystem enemyGrid = _gridSystemSpawner.Spawn(CharacterType.Enemy, _firstEnemyGridPosition);

        _shipsSpawner.SpawnShipsFor(playerGrid);
        _shipsSpawner.SpawnShipsFor(enemyGrid);
        
        const int enemyGridPositionMultiplier = 4;
        playerGrid.MoveTo(_firstPlayerGridPosition);
        enemyGrid.MoveTo(_firstEnemyGridPosition * enemyGridPositionMultiplier);

        _gridSystems.Add(CharacterType.Player, playerGrid);
        _gridSystems.Add(CharacterType.Enemy, enemyGrid);
    }

    public void Dispose()
    {
        foreach (KeyValuePair<CharacterType, GridSystem> keyValuePair in _gridSystems) 
            keyValuePair.Value.Dispose();
    }

    public void MoveGridsToBattle()
    {
        _gridSystems[CharacterType.Player].MoveTo(_firstPlayerGridPosition, 1f);
        _gridSystems[CharacterType.Enemy].MoveTo(_firstEnemyGridPosition, 1f);
    }

    public int GetGridSystemHeight(CharacterType characterType) => _gridSystems[characterType].Height;
    
    public int GetGridSystemWidth(CharacterType characterType) => _gridSystems[characterType].Width;
    
    public Ship.Ship GetShipAtCellPosition(CharacterType characterType, CellPosition cellPosition) =>
        TryGetGridCell(characterType, cellPosition, out GridCell gridCell)
            ? gridCell.Ship
            : null;

    public bool WasGridCellHit(CharacterType characterType, CellPosition cellPosition) =>
        TryGetGridCell(characterType, cellPosition, out GridCell gridCell) && gridCell.WasHit;

    public bool HasShipOnCellPosition(CharacterType characterType, CellPosition cellPosition) =>
        TryGetGridCell(characterType, cellPosition, out GridCell gridCell) && gridCell.HasShip();

    public bool IsShipDestroyedOnCellPosition(CharacterType characterType, CellPosition cellPosition)
    {
        if (!TryGetGridCell(characterType, cellPosition, out GridCell gridCell))
            return false;

        return gridCell.HasShip() && gridCell.Ship.IsDestroyed();
    }

    public bool IsShipDamagedOnCellPosition(CharacterType characterType, CellPosition cellPosition)
    {
        if (!TryGetGridCell(characterType, cellPosition, out GridCell gridCell))
            return false;

        return gridCell.HasShip() &&
               gridCell.WasHit;
    }

    public bool IsShipDamagedUndestroyedOnCellPosition(CharacterType characterType, CellPosition cellPosition)
    {
        if (!TryGetGridCell(characterType, cellPosition, out GridCell gridCell))
            return false;

        return gridCell.HasShip() &&
               gridCell.WasHit &&
               !gridCell.Ship.IsDestroyed();
    }

    public Vector2 GetWorldCellPosition(CharacterType characterType, CellPosition cellPosition) => 
        _gridSystems[characterType].GetWorldCellPosition(cellPosition);

    public CellPosition GetCellPosition(CharacterType characterType, Vector2 worldPosition) =>
        _gridSystems[characterType].GetCellPosition(worldPosition);

    public bool TryGetValidGridCellPositions(CharacterType characterType, Vector2 worldPosition, Ship.Ship shipToMove,
        out List<CellPosition> cellPositions)
    {
        bool isValid = _gridSystems[characterType]
            .TryGetValidCellPositions(worldPosition, shipToMove, out List<CellPosition> result);
        cellPositions = result;
        return isValid;
    }

    public List<CellPosition> GetValidRandomCellPositions(Ship.Ship ship) =>
        _gridSystems[ship.GetCharacterType()].GetValidRandomCellPositions(ship);

    public void MoveTo(CharacterType characterType, Vector2 to) => _gridSystems[characterType].MoveTo(to);

    public void MoveTo(CharacterType characterType, Vector2 to, float duration) =>
        _gridSystems[characterType].MoveTo(to, duration);

    public bool CanPlaceShipAt(CharacterType characterType, CellPosition cellPosition, Ship.Ship ship) =>
        _gridSystems[characterType].CanPlaceShipAt(cellPosition, ship, out _);

    public float GetCellSize(CharacterType characterType) => _gridSystems[characterType].CellSize;

    public CellPosition GetRandomUnshotCellPosition(CharacterType characterType) =>
        _gridSystems[characterType].GetRandomUnshotCellPosition();

    public List<CellPosition> GetHitUndestroyedCellPositions(CharacterType characterType) =>
        _gridSystems[characterType]
            .GetShipHitCellPositions()
            .Where(hitCellPosition => !IsShipDestroyedOnCellPosition(characterType, hitCellPosition))
            .ToList();

    public bool IsCellPositionValidForShooting(CharacterType characterType, CellPosition cellPosition) =>
        _gridSystems[characterType].IsCellPositionValidForShooting(cellPosition);

    public bool IsCellPositionUnshot(CharacterType characterType, CellPosition cellPosition)
    {
        if (!TryGetGridCell(characterType, cellPosition, out GridCell gridCell))
            return false;
        return !gridCell.WasHit;
    }

    public bool IsCellOutOfBounds(CellPosition cellPosition) =>
        _gridSystems.First().Value.IsCellPositionOutOfBounds(cellPosition);

    public List<CellPosition> GetValidForShootingSidesCellPositions(CharacterType characterType,
        CellPosition cellPosition) =>
        _gridSystems[characterType].GetValidForShootingSidesCellPositions(cellPosition);

    private bool TryGetGridCell(CharacterType characterType, CellPosition cellPosition, out GridCell gridCell)
    {
        gridCell = null;

        if (!_gridSystems[characterType].TryGetCell(cellPosition, out GridCell cell))
            return false;

        gridCell = cell;
        return true;
    }
}