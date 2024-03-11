using Scripts.EventBus;

namespace Events
{
    public struct OnCountdownUpdated : IEvent
    {
        public readonly int Seconds;

        public OnCountdownUpdated(int seconds)
        {
            Seconds = seconds;
        }
    }
}