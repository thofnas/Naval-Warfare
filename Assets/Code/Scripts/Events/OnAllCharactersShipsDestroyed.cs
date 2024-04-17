using EventBus;

namespace Events
{
    public struct OnAllCharactersShipsDestroyed : IEvent
    {
        public readonly CharacterType LoserCharacterType;

        public OnAllCharactersShipsDestroyed(CharacterType loserCharacterType)
        {
            LoserCharacterType = loserCharacterType;
        }
    }
}