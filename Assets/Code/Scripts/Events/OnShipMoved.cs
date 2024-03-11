using System.Collections.Generic;
using Grid;
using Scripts.EventBus;

namespace Events
{
    public struct OnShipMoved : IEvent
    {
        public readonly Ship.Ship Ship;
        public readonly CharacterType CharacterType;
        public readonly List<CellPosition> From;
        public readonly List<CellPosition> To;

        public OnShipMoved(Ship.Ship ship, List<CellPosition> from, List<CellPosition> to)
        {
            Ship = ship;
            CharacterType = ship.GetCharacterType();
            From = from;
            To = to;
        }
    }
}