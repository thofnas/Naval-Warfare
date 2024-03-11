using Scripts.EventBus;
using UnityEngine.UIElements;

namespace Events
{
    public struct OnReadyUIButtonToggled : IEvent
    {
        public readonly Toggle Toggle;
        public readonly bool IsOn;

        public OnReadyUIButtonToggled(Toggle toggle)
        {
            Toggle = toggle;
            IsOn = toggle.value;
        }
    }
}