using System;
using System.Collections.Generic;
using System.Linq;
using Grid;
using Utilities.Extensions;
using Zenject;

namespace Enemy
{
    public class AIDamagedShipSearcher
    {
        private readonly Level _level;

        [Inject]
        private AIDamagedShipSearcher(Level level)
        {
            _level = level;
        }

        public bool TrySearchForShip(CharacterType characterType, List<CellPosition> shotUndestroyedCellPositions, out CellPosition selectedCellPosition)
        {
            // Consolidate the search for unshot cells into a single list
            List<CellPosition> potentialTargets = FindPotentialTargets(characterType, shotUndestroyedCellPositions);
        
            // If no damaged, unshot cells were found, get valid shooting positions
            if (!potentialTargets.Any())
            {
                potentialTargets = shotUndestroyedCellPositions
                    .SelectMany(pos => _level.GetValidForShootingSidesCellPositions(characterType, pos))
                    .ToList();
            }
        
            // Select a random cell from the potential targets if any exist
            selectedCellPosition = potentialTargets.Any() ? potentialTargets.Shuffle().First() : CellPosition.Zero;
            
            // Return whether any potential targets were found
            return potentialTargets.Any();
        }
        
        private List<CellPosition> FindPotentialTargets(CharacterType characterType, IEnumerable<CellPosition> shotUndestroyedCellPositions)
        {
            var directions = new[] { Direction.Up, Direction.Right, Direction.Down, Direction.Left };
            var result = new List<CellPosition>();
        
            foreach (CellPosition cellPosition in shotUndestroyedCellPositions)
            {
                foreach (Direction direction in directions)
                {
                    CellPosition adjacentPosition = GetAdjacentPosition(cellPosition, direction);
                    if (_level.IsShipDamagedUndestroyedOnCellPosition(characterType, adjacentPosition) && 
                        TryFindUnshotCellInDirection(direction, characterType, adjacentPosition, out CellPosition unshotCellPosition))
                    {
                        result.Add(unshotCellPosition);
                    }
                }
            }
        
            return result;
        }
        
        private static CellPosition GetAdjacentPosition(CellPosition cellPosition, Direction direction)
        {
            // This method assumes you have a way to compute cell positions based on direction
            return direction switch
            {
                Direction.Up => cellPosition.GetOnAbove(),
                Direction.Right => cellPosition.GetOnRight(),
                Direction.Down => cellPosition.GetOnBelow(),
                Direction.Left => cellPosition.GetOnLeft(),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), $"Unsupported direction: {direction}")
            };
        }

        private bool TryFindUnshotCellInDirection(Direction direction, CharacterType characterType,
            CellPosition checkingPosition, out CellPosition unshotCell)
        {
            while (true)
            {
                CellPosition nextCell = GetAdjacentPosition(checkingPosition, direction);

                if (_level.IsCellOutOfBounds(nextCell))
                {
                    unshotCell = CellPosition.Zero;
                    return false;
                }

                if (_level.IsShipDestroyedOnCellPosition(characterType, nextCell))
                {
                    unshotCell = CellPosition.Zero;
                    return false;
                }

                // skip if was hit and no ship in there (missed)
                if (!_level.IsCellPositionUnshot(characterType, nextCell) &&
                    !_level.HasShipOnCellPosition(characterType, nextCell))
                {
                    unshotCell = CellPosition.Zero;
                    return false;
                }

                //skip damaged grid cell with a ship
                if (_level.IsShipDamagedOnCellPosition(characterType, nextCell))
                {
                    checkingPosition = nextCell;
                    continue;
                }


                if (_level.IsCellPositionValidForShooting(characterType, nextCell))
                {
                    unshotCell = nextCell;
                    return true;
                }

                checkingPosition = nextCell;
            }
        }

        private enum Direction
        {
            Up = 10,
            Right = 20,
            Down = 30,
            Left = 40
        }
    }
}