using Grid;
using Scripts.EventBus;

namespace Events
{
    public struct OnCellHit : IEvent
    {
        public readonly CharacterType WoundedCharacterType;
        public readonly CellPosition HitCellPosition;
        public readonly Ship.Ship Ship;

        public OnCellHit(CharacterType woundedCharacterType, CellPosition hitCellPosition, Ship.Ship ship)
        {
            WoundedCharacterType = woundedCharacterType;
            HitCellPosition = hitCellPosition;
            Ship = ship;
        }
    }
}