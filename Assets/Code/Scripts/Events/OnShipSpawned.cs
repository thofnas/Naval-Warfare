using EventBus;

namespace Events
{
    public struct OnShipSpawned : IEvent
    {
        public readonly Ship.Ship Ship;
        public readonly CharacterType CharacterType;

        public OnShipSpawned(Ship.Ship ship, CharacterType characterType)
        {
            Ship = ship;
            CharacterType = characterType;
        }
    }
}