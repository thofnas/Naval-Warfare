using EventBus;

namespace Events
{
    public struct OnBattleEnded : IEvent
    {
        public readonly bool IsWin;
        
        public OnBattleEnded(bool isWin)
        {
            IsWin = isWin;
        }
    }
}