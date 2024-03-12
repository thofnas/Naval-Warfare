using System.Collections.Generic;
using EventBus;
using Grid;

namespace Events
{
    public struct OnShipDestroyed : IEvent
    {
        public readonly Ship.Ship Ship;
        public readonly CharacterType CharacterType;
        public readonly IReadOnlyList<CellPosition> ShipCellPositions;

        public OnShipDestroyed(Ship.Ship ship, CharacterType characterType,
            IReadOnlyList<CellPosition> shipCellPositions)
        {
            Ship = ship;
            CharacterType = characterType;
            ShipCellPositions = shipCellPositions;
        }
    }
}