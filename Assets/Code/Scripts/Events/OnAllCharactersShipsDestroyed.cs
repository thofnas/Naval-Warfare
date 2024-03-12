using EventBus;

namespace Events
{
    public struct OnAllCharactersShipsDestroyed : IEvent
    {
        public readonly CharacterType LostCharacterType;

        public OnAllCharactersShipsDestroyed(CharacterType lostCharacterType)
        {
            LostCharacterType = lostCharacterType;
        }
    }
}