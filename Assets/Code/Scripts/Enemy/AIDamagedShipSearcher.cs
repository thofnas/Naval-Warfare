using System;
using System.Collections.Generic;
using System.Linq;
using Grid;
using Utilities.Extensions;
using Zenject;

namespace Enemy
{
    public readonly struct AIDamagedShipSearcher
    {
        private readonly Level _level;

        [Inject]
        private AIDamagedShipSearcher(Level level)
        {
            _level = level;
        }

        public bool TrySearchForShip(CharacterType characterType, List<CellPosition> shotUndestroyedCellPositions,
            out CellPosition selectedCellPosition)
        {
            List<CellPosition> result = new();

            foreach (CellPosition cellPosition in shotUndestroyedCellPositions)
            {
                if (_level.IsShipDamagedUndestroyedOnCellPosition(characterType, cellPosition.GetOnAbove()))
                    if (TryFindUnshotCellInDirection(Direction.Up, characterType, cellPosition.GetOnAbove(),
                            out CellPosition unshotCellPosition))
                        result.Add(unshotCellPosition);

                if (_level.IsShipDamagedUndestroyedOnCellPosition(characterType, cellPosition.GetOnRight()))
                    if (TryFindUnshotCellInDirection(Direction.Right, characterType, cellPosition.GetOnRight(),
                            out CellPosition unshotCellPosition))
                        result.Add(unshotCellPosition);

                if (_level.IsShipDamagedUndestroyedOnCellPosition(characterType, cellPosition.GetOnBelow()))
                    if (TryFindUnshotCellInDirection(Direction.Down, characterType, cellPosition.GetOnBelow(),
                            out CellPosition unshotCellPosition))
                        result.Add(unshotCellPosition);

                if (_level.IsShipDamagedUndestroyedOnCellPosition(characterType, cellPosition.GetOnLeft()))
                    if (TryFindUnshotCellInDirection(Direction.Left, characterType, cellPosition.GetOnLeft(),
                            out CellPosition unshotCellPosition))
                        result.Add(unshotCellPosition);
            }

            if (result.IsNullOrEmpty())
                foreach (CellPosition cellPosition in shotUndestroyedCellPositions)
                    result.AddRange(_level.GetValidForShootingSidesCellPositions(characterType, cellPosition));

            selectedCellPosition = result.IsNullOrEmpty()
                ? CellPosition.Zero
                : result.Shuffle().First();

            return !result.IsNullOrEmpty();
        }

        private bool TryFindUnshotCellInDirection(Direction direction, CharacterType characterType,
            CellPosition checkingPosition, out CellPosition unshotCell)
        {
            while (true)
            {
                CellPosition nextCell = GetNextCellPosition(direction, checkingPosition);

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

        private static CellPosition GetNextCellPosition(Direction direction, CellPosition checkingPosition) =>
            direction switch
            {
                Direction.Up => checkingPosition.GetOnAbove(),
                Direction.Right => checkingPosition.GetOnRight(),
                Direction.Down => checkingPosition.GetOnBelow(),
                Direction.Left => checkingPosition.GetOnLeft(),
                _ => throw new ArgumentOutOfRangeException()
            };

        // can be represented in degrees instead
        private enum Direction
        {
            Up = 10,
            Right = 20,
            Down = 30,
            Left = 40
        }
    }
}