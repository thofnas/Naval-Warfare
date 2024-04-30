using EventBus;

namespace Events
{
    public struct OnPlayerPrefsLanguageChanged : IEvent
    {
        public readonly string From;
        public readonly string To;

        public OnPlayerPrefsLanguageChanged(string from, string to)
        {
            From = from;
            To = to;
        }
    }
}