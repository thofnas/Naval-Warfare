using System;
using System.Collections.Generic;
using System.Linq;
using EventBus;
using Events;
using UnityEngine;
using Utilities.Extensions;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Grid
{
    public struct GridSystem : IDisposable
    {
        public int Width { get; }
        public int Height { get; }
        public float CellSize { get; }
        private readonly GridCell[,] _cells;
        private readonly List<GridCell> _occupiedCells;
        private readonly List<GridCell> _unshotCells;
        private readonly List<GridCell> _shotCells;
        private readonly CharacterType _characterType;

        public GameObject Parent { get; }

        private readonly EventBinding<OnShipMoved> _shipChangedPosition;
        private readonly EventBinding<OnCellHit> _onHit;
        private GridSystemVisual _gridSystemVisual;

        [Inject]
        public GridSystem(CharacterType characterType, Settings settings)
        {
            Width = settings.Width;
            Height = settings.Height;
            CellSize = settings.CellGap;
            _cells = new GridCell[settings.Width, settings.Height];
            _occupiedCells = new List<GridCell>();
            _unshotCells = new List<GridCell>();
            _shotCells = new List<GridCell>();
            _characterType = characterType;
            _shipChangedPosition = null;
            _onHit = null;
            _gridSystemVisual = null;

            var gridSystemGameObject = new GameObject(characterType + "Grid");
            Parent = gridSystemGameObject.gameObject;

            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
            {
                var gridPosition = new CellPosition(x, y);
                var gridCell = new GridCell(characterType, gridPosition);
                _cells[x, y] = gridCell;
                _unshotCells.Add(gridCell);
            }

            _shipChangedPosition = new EventBinding<OnShipMoved>(AnyShip_OnChangedPosition);
            _onHit = new EventBinding<OnCellHit>(GridCell_OnShotTaken);

            EventBus<OnShipMoved>.Register(_shipChangedPosition);
            EventBus<OnCellHit>.Register(_onHit);
        }

        public void Dispose()
        {
            foreach (GridCell cell in _cells) cell.Dispose();

            EventBus<OnShipMoved>.Deregister(_shipChangedPosition);
            EventBus<OnCellHit>.Deregister(_onHit);
        }

        public GridCell GetGridCell(CellPosition cellPosition) => _cells[cellPosition.x, cellPosition.y];

        public Vector2 GetGridCenter() => _gridSystemVisual.GetGridCenter();

        public void MoveTo(Vector2 position) => _gridSystemVisual.MoveTo(position);

        public void MoveTo(Vector2 position, float duration) => _gridSystemVisual.MoveTo(position, duration);

        public CharacterType GetCharacterType() => _characterType;

        public GridSystemVisual GetGridSystemVisual() => _gridSystemVisual;

        public void SetGridSystemVisual(GridSystemVisual gridSystemVisual)
        {
            if (_gridSystemVisual != null)
                Debug.LogError($"{nameof(GridSystemVisual)} was already assigned for {_characterType}");

            _gridSystemVisual = gridSystemVisual;
        }

        public List<CellPosition> GetValidRandomCellPositions(Ship.Ship ship)
        {
            if (TryGetValidCellPositions(GetWorldCellPosition(GetRandomCellPosition()), ship,
                    out List<CellPosition> cellPositions))
                return cellPositions;

            return GetValidRandomCellPositions(ship);
        }

        public bool IsCellPositionValidForShooting(CellPosition cellPosition)
        {
            if (!TryGetCell(cellPosition, out GridCell gridCell)) return false;

            if (gridCell.WasHit) return false;

            if (gridCell.HasShip())
                if (gridCell.Ship.IsDestroyed())
                    return false;

            return true;
        }

        public List<CellPosition> GetValidForShootingSidesCellPositions(CellPosition cellPosition)
        {
            var validCellPositions = new List<CellPosition>();

            // Define the positions clockwise (above, right, below, left)
            var potentialPositions = new List<CellPosition>
            {
                cellPosition.GetOnAbove(),
                cellPosition.GetOnRight(),
                cellPosition.GetOnBelow(),
                cellPosition.GetOnLeft()
            };

            foreach (CellPosition position in potentialPositions)
                if (IsCellPositionValidForShooting(position))
                    validCellPositions.Add(position);

            return validCellPositions;
        }

#if UNITY_EDITOR
        public void CreateCellDebug(GridCellDebug gridCellDebugPrefab)
        {
            var debug = new GameObject("Debug");

            debug.SetParent(Parent.transform);

            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
            {
                var cellPosition = new CellPosition(x, y);

                GridCellDebug gridCellDebug = Object.Instantiate(gridCellDebugPrefab,
                    GetWorldCellPosition(cellPosition), Quaternion.identity, debug.transform);

                TryGetCell(cellPosition, out GridCell gridCell);

                gridCellDebug.SetGridCell(gridCell);
            }
        }
#endif

        public Vector2 GetWorldCellPosition(CellPosition cellPosition) =>
            _gridSystemVisual.GetWorldCellPosition(cellPosition);

        public CellPosition GetCellPosition(Vector2 worldPosition) =>
            new(
                Mathf.Clamp(Mathf.RoundToInt((worldPosition.x - _gridSystemVisual.GetOffset().x) / CellSize), 0,
                    Width - 1),
                Mathf.Clamp(Mathf.RoundToInt((worldPosition.y - _gridSystemVisual.GetOffset().y) / CellSize), 0,
                    Height - 1)
            );

        public bool TryGetValidCellPositions(Vector2 worldPosition, Ship.Ship shipToMove,
            out List<CellPosition> cellPositions)
        {
            var validPositions = new List<CellPosition>();
            Vector2Int shipDimensions = shipToMove.GetShipDimensions();

            int cellX = Mathf.RoundToInt((worldPosition.x - _gridSystemVisual.GetOffset().x) / CellSize);
            int cellY = Mathf.RoundToInt((worldPosition.y - _gridSystemVisual.GetOffset().y) / CellSize);

            int minX = Mathf.Clamp(cellX - Mathf.FloorToInt(shipDimensions.x * 0.5f), 0, Width - shipDimensions.x);
            int minY = Mathf.Clamp(cellY - Mathf.FloorToInt(shipDimensions.y * 0.5f), 0, Height - shipDimensions.y);

            var currentCell = new CellPosition(minX, minY);

            // Check if the ship can be placed at the current cell
            if (CanPlaceShipAt(currentCell, shipToMove, out List<CellPosition> checkedPositions))
                validPositions.AddRange(checkedPositions);

            cellPositions = validPositions;

            return cellPositions.Count > 0;
        }

        public bool CanPlaceShipAt(CellPosition placingAtCellPosition, Ship.Ship ship,
            out List<CellPosition> checkedPositions)
        {
            checkedPositions = new List<CellPosition>();
            Vector2Int shipDimensions = ship.GetShipDimensions();

            for (var x = 0; x < shipDimensions.x; x++)
            for (var y = 0; y < shipDimensions.y; y++)
            {
                CellPosition currentCellPosition = new(placingAtCellPosition.x + x, placingAtCellPosition.y + y);

                if (!TryGetCell(currentCellPosition, out GridCell gridCell))
                    return false;

                if (_occupiedCells.Contains(gridCell))
                    if (gridCell.Ship != ship)
                        return false;

                checkedPositions.Add(currentCellPosition);
            }

            return true;
        }

        public CellPosition GetRandomCellPosition() => new(Random.Range(0, Width), Random.Range(0, Height));

        public CellPosition GetRandomUnshotCellPosition() =>
            _unshotCells[Random.Range(0, _unshotCells.Count)].CellPosition;

        public List<CellPosition> GetShotCellPositions() =>
            _shotCells.Select(shotCell => shotCell.CellPosition).ToList();

        public IEnumerable<CellPosition> GetShipHitCellPositions() =>
            _shotCells
                .Where(shotCell => shotCell.HasShip())
                .Where(cellWithShip => cellWithShip.WasHit)
                .Select(damagedCellWithShip => damagedCellWithShip.CellPosition)
                .ToList();

        public bool IsCellPositionOutOfBounds(CellPosition cellPosition) =>
            cellPosition.x < 0 ||
            cellPosition.x >= Width ||
            cellPosition.y < 0 ||
            cellPosition.y >= Height;

        public bool TryGetCell(CellPosition cellPosition, out GridCell gridCell)
        {
            if (IsCellPositionOutOfBounds(cellPosition))
            {
                gridCell = null;
                return false;
            }

            gridCell = _cells[cellPosition.x, cellPosition.y];
            return true;
        }

        private void AnyShip_OnChangedPosition(OnShipMoved e)
        {
            if (e.Ship.GetCharacterType() != _characterType) return;

            foreach (CellPosition cellPosition in e.From)
            {
                TryGetCell(cellPosition, out GridCell gridCell);
                _occupiedCells.Remove(gridCell);
            }

            foreach (CellPosition cellPosition in e.To)
            {
                TryGetCell(cellPosition, out GridCell gridCell);
                _occupiedCells.Add(gridCell);
            }
        }

        private void GridCell_OnShotTaken(OnCellHit e)
        {
            if (e.WoundedCharacterType != GetCharacterType()) return;

            _unshotCells.Remove(GetGridCell(e.HitCellPosition));
            _shotCells.Add(GetGridCell(e.HitCellPosition));
        }

        [Serializable]
        public class Settings
        {
            [Range(1, 10)] public int Width = 10;
            [Range(1, 10)] public int Height = 10;
            [Range(1f, 1.1f)] public float CellGap = 1.05f;
        }

        public class Factory : PlaceholderFactory<CharacterType, GridSystem>
        {
        }
    }
}