using EventBus;
using Grid;

namespace Events
{
    public struct OnGridCellSelected : IEvent
    {
        public readonly CharacterType CharacterType;
        public readonly CellPosition From;
        public readonly CellPosition To;

        public OnGridCellSelected(CharacterType characterType, CellPosition from, CellPosition to)
        {
            CharacterType = characterType;
            From = from;
            To = to;
        }
    }
}