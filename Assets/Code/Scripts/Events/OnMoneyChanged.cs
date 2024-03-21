using EventBus;

namespace Events
{
    public struct OnMoneyChanged : IEvent
    {
        public readonly int From;
        public readonly int To;

        public OnMoneyChanged(int from, int to)
        {
            From = from;
            To = to;
        }
    }
}