using EventBus;

namespace Events
{
    public struct OnButtonClick : IEvent
    {
        public readonly bool IsSwitch;

        public OnButtonClick(bool isSwitch)
        {
            IsSwitch = isSwitch;
        }
    }
}