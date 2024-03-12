using EventBus;

namespace Events
{
    public struct OnTurnChanged : IEvent
    {
        public readonly CharacterType CharacterType;

        public OnTurnChanged(CharacterType characterType)
        {
            CharacterType = characterType;
        }
    }
}