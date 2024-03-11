using Grid;
using Scripts.EventBus;

namespace Events
{
    public struct OnShoot : IEvent
    {
        public readonly CharacterType Receiver;
        public readonly CellPosition CellPosition;

        public OnShoot(CharacterType receiver, CellPosition cellPosition)
        {
            Receiver = receiver;
            CellPosition = cellPosition;
        }
    }
}